// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;

/// <summary>
/// Time configurator.
/// </summary>
internal sealed class TimeConfigurator : ConfiguratorBase
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
    internal TimeConfigurator(SolverState state, IReadOnlyDirectedRouteMatrix routeMatrix) 
        : base(state)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));

        TimeCallback = State.SolverInterface.RegisterCallback(new TimeCallback(routeMatrix));
        TimeDimension = State.SolverInterface.RegisterDimension(new TimeDimension(state.SolverModel, TimeCallback));
    }
    
    /// <inheritdoc/>
    public override void Configure(SolverState state)
    {
        SetupVehicleTimeCosts(state);
    }

    /// <summary>
    /// Sets up the vehicle time costs.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private void SetupVehicleTimeCosts(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            var vehicle = state.SolverModel.Vehicles[i];
            var timeCost = vehicle.TimeCost;
            
            TimeDimension.Dimension.SetSpanCostCoefficientForVehicle(timeCost, i);
        }
    }
}