// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;

/// <summary>
/// Time configurator.
/// </summary>
internal sealed class TimeConfigurator : ConfiguratorBase<Dimension>
{
    /// <summary>
    /// The time callback.
    /// </summary>
    internal SolverCallback TimeCallback { get; }
    
    /// <summary>
    /// The time dimension.
    /// </summary>
    internal SolverDimension TimeDimension { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeConfigurator"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routeMatrix"/> is <see langword="null"/>.</exception>
    internal TimeConfigurator(SolverState<Dimension> state, IReadOnlyDirectedRouteMatrix routeMatrix) 
        : base(state)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));

        TimeCallback = State.SolverInterface.RegisterCallback(new TimeCallback(routeMatrix));
        TimeDimension = State.SolverInterface.RegisterDimension(Dimension.TimeDimension, new TimeDimension(state.SolverModel, TimeCallback));
    }
    
    /// <inheritdoc/>
    public override void Configure(SolverState<Dimension> state)
    {
        SetupVehicleTimeCosts(state);
        AddTimeWindowConstraints(state);
        SetupVehicleBreaks(state);
        SetupVehicleObjectiveFunctions(state);
    }

    /// <summary>
    /// Sets up the vehicle time costs.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private void SetupVehicleTimeCosts(SolverState<Dimension> state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            var vehicle = state.SolverModel.Vehicles[i];
            var timeCost = vehicle.TimeCost;
            
            TimeDimension.RoutingDimension.SetSpanCostCoefficientForVehicle(timeCost, i);
        }
    }
    
    /// <summary>
    /// Adds time window constraints.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private void AddTimeWindowConstraints(SolverState<Dimension> state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        for (var i = 0; i < state.SolverModel.NodeCount; i++)
        {
            var node = state.SolverModel.Nodes[i];
            var range = node.GetTimeWindow();
            var nodeIndex = state.SolverInterface.NodeToIndex(node);
            
            // Copy the slack var to the assignment,
            // so we can later retrieve the slack time of the node.
            state.SolverInterface.RoutingModel.AddToAssignment(TimeDimension.RoutingDimension.SlackVar(nodeIndex));
            
            // Skip the start and end nodes.
            // Start nodes get their time windows from the vehicles
            // and end nodes do not have time windows.
            if (state.SolverInterface.RoutingModel.IsStart(nodeIndex) || state.SolverInterface.RoutingModel.IsEnd(nodeIndex))
            {
                continue;
            }
            
            // Skip nodes without time windows.
            if (range is null)
            {
                continue;
            }
            
            TimeDimension.RoutingDimension.CumulVar(nodeIndex).SetRange(range.Value.Min, range.Value.Max);
        }
        
        // Add time window constraints for the vehicles.
        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            var vehicle = state.SolverModel.Vehicles[i];
            var shiftTimeWindow = vehicle.AvailableTimeWindow;
            
            var index = state.SolverInterface.RoutingModel.Start(i);
            TimeDimension.RoutingDimension.CumulVar(index).SetRange(shiftTimeWindow.Min, shiftTimeWindow.Max);
            state.SolverInterface.RoutingModel.AddToAssignment(TimeDimension.RoutingDimension.SlackVar(index));
        }
    }
    
    /// <summary>
    /// Sets up the vehicle breaks of the solver.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private void SetupVehicleBreaks(SolverState<Dimension> state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        // Get an array with all time demands of the nodes,
        // so the solver can distinguish between travel and service times.
        var nodeTimeDemands = GetNodeTimeDemands(state);
        
        // We need to give each break a unique name,
        // so we simply use an index to create a unique name.
        var breakIndex = 0;
        
        // Add breaks for each vehicle.
        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            var vehicle = state.SolverModel.Vehicles[i];
            var intervalVars = new IntervalVarVector();
            
            foreach (var currentBreak in vehicle.Breaks)
            {
                var breakTimeWindow = currentBreak.AllowedTimeWindow;
                var breakDuration = currentBreak.Duration;
                var breakOptional = currentBreak.Option == BreakOption.Optional;
                var breakName = $"Break_{breakIndex++}";
                
                var intervalVar = state.SolverInterface.Solver.MakeFixedDurationIntervalVar(breakTimeWindow.Min, breakTimeWindow.Max, breakDuration, breakOptional, breakName);
                intervalVars.Add(intervalVar);
            }
            
            TimeDimension.RoutingDimension.SetBreakIntervalsOfVehicle(intervalVars, i, nodeTimeDemands);
        }
    }
    
    /// <summary>
    /// Gets the node time demands.
    /// </summary>
    /// <returns>The node time demands.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private long[] GetNodeTimeDemands(SolverState<Dimension> state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        // https://github.com/google/or-tools/issues/2578
        var count = state.SolverInterface.RoutingModel.Size();
        var timeDemands = new long[count];
        for (var i = 0; i < count; i++)
        {
            var nodeIndex = state.SolverInterface.IndexToNodeIndex(i);
            var node = state.SolverModel.Nodes[nodeIndex];
            timeDemands[i] = node.GetTimeDemand();
        }
        return timeDemands;
    }
    
    /// <summary>
    /// Sets the objective functions for the vehicles.
    /// </summary>
    private void SetupVehicleObjectiveFunctions(SolverState<Dimension> state)
    {
        // We want to maximize the start time to start as late as possible
        // and minimize the end time to finish as early as possible.
        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            state.SolverInterface.RoutingModel.AddVariableMaximizedByFinalizer(TimeDimension.RoutingDimension.CumulVar(state.SolverInterface.RoutingModel.Start(i)));
            state.SolverInterface.RoutingModel.AddVariableMinimizedByFinalizer(TimeDimension.RoutingDimension.CumulVar(state.SolverInterface.RoutingModel.End(i)));
        }
    }
}