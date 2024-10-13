// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.MathUtils;
using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Nodes;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Configures the time of the solver.
/// </summary>
internal class TimeConfigurator : SolverConfiguratorBase
{
    /// <summary>
    /// The name of the time dimension.
    /// </summary>
    internal const string TimeDimensionName = "TimeDimension";
    
    /// <summary>
    /// The time dimension.
    /// </summary>
    internal RoutingDimension TimeDimension { get; }
    
    internal TimeCallback Callback { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeConfigurator"/> class.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routeMatrix"/> is <see langword="null"/>.</exception>
    public TimeConfigurator(SolverState state, IReadOnlyDirectedRouteMatrix routeMatrix)
        : base(state)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));
        
        Callback = new TimeCallback(state, routeMatrix);
        TimeDimension = CreateTimeDimension(state);
    }
    
    /// <inheritdoc/>
    public override void Configure(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        AddTimeWindowConstraints(state);
        SetupVehicleCosts(state);
        SetupVehicleBreaks(state);
    }
    
    /// <summary>
    /// Creates the time dimension.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <param name="maxSlack">The maximum slack time.</param>
    /// <returns>The time dimension.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private RoutingDimension CreateTimeDimension(SolverState state, long maxSlack = long.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        var adjustedMaxSlackTime = Math.Max(0, maxSlack);
        var maxTravelTimes = state.Vehicles.Select(v => v.MaxDuration).ToArray();
        var dimensionCreated = Model.AddDimensionWithVehicleCapacity(
            Callback,
            adjustedMaxSlackTime,
            maxTravelTimes,
            false,
            TimeDimensionName
        );
        VehicleRoutingSolverException.ThrowIfNot(dimensionCreated, $"Failed to create the time dimension '{TimeDimensionName}'.");
        return Model.GetMutableDimension(TimeDimensionName);
    }
    
    /// <summary>
    /// Adds the time window constraints to the solver.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private void AddTimeWindowConstraints(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        // Add time window constraints for the nodes.
        for (var i = 0; i < state.NodeCount; i++)
        {
            var node = state.Nodes[i];
            var range = node.GetTimeWindow();
            var index = IndexManager.NodeToIndex(i);
            
            // Copy the slack var to the assignment,
            // so we can later retrieve the slack time of the node.
            Model.AddToAssignment(TimeDimension.SlackVar(index));
            
            // Skip the start and end nodes.
            // Start nodes get their time windows from the vehicles
            // and end nodes do not have time windows.
            if (Model.IsStart(index) || Model.IsEnd(index))
            {
                continue;
            }
            
            // Skip nodes without time windows.
            if (range is null)
            {
                continue;
            }
            
            TimeDimension.CumulVar(index).SetRange(range.Value.Min, range.Value.Max);
        }
        
        // Add time window constraints for the vehicles.
        for (var i = 0; i < state.VehicleCount; i++)
        {
            var vehicle = state.Vehicles[i];
            var shiftTimeWindow = vehicle.AvailableTimeWindow;
            
            var index = Model.Start(i);
            TimeDimension.CumulVar(index).SetRange(shiftTimeWindow.Min, shiftTimeWindow.Max);
            Model.AddToAssignment(TimeDimension.SlackVar(index));
        }
    }
    
    /// <summary>
    /// Sets up the vehicle breaks of the solver.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private void SetupVehicleBreaks(SolverState state)
    {
        var nodeTimeDemands = GetNodeTimeDemands(state);

        var breakIndex = 0;
        for (var i = 0; i < state.VehicleCount; i++)
        {
            var vehicle = state.Vehicles[i];

            var intervalVars = new IntervalVarVector();
            foreach (var currentBreak in vehicle.Breaks)
            {
                var breakTimeWindow = currentBreak.AllowedTimeWindow;
                var breakDuration = currentBreak.Duration;
                var breakOptional = currentBreak.Option == BreakOption.Optional;
                var breakName = $"Break_{breakIndex++}";
                
                var intervalVar = Solver.MakeFixedDurationIntervalVar(breakTimeWindow.Min, breakTimeWindow.Max, breakDuration, breakOptional, breakName);
                intervalVars.Add(intervalVar);
            }
            
            TimeDimension.SetBreakIntervalsOfVehicle(intervalVars, i, nodeTimeDemands);
        }
    }
    
    /// <summary>
    /// Returns the time demands of the nodes.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <returns>The time demands of the nodes.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private long[] GetNodeTimeDemands(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        var count = Model.Size();
        var timeDemands = new long[count];
        for (var i = 0; i < count; i++)
        {
            var nodeIndex = IndexManager.IndexToNode(i);
            var node = state.Nodes[nodeIndex];
            timeDemands[i] = node.GetTimeDemand();
        }
        return timeDemands;
    }
    
    /// <summary>
    /// Sets up the vehicle costs for the time dimension.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private void SetupVehicleCosts(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        for (var i = 0; i < state.VehicleCount; i++)
        {
            var vehicle = state.Vehicles[i];
            var timeCost = vehicle.TimeCost;
            TimeDimension.SetSpanCostCoefficientForVehicle(timeCost, i);
        }
    }
}