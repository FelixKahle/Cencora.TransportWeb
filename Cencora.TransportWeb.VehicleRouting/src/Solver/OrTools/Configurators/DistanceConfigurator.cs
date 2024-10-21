// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;

/// <summary>
/// Distance configurator.
/// </summary>
internal sealed class DistanceConfigurator : IConfigurator<Dimension>
{
    private readonly ImmutableRouteMatrix _routeMatrix;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DistanceConfigurator"/> class.
    /// </summary>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routeMatrix"/> is <see langword="null"/>.</exception>
    internal DistanceConfigurator(ImmutableRouteMatrix routeMatrix) 
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));
        
        _routeMatrix = routeMatrix;
    }

    /// <inheritdoc/>
    public void Configure(SolverState<Dimension> state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        // Setup distance callback and dimension
        var callback = state.SolverInterface.RegisterCallback(new DistanceCallback(_routeMatrix));
        var distanceDimension = state.SolverInterface.RegisterDimension(Dimension.DistanceDimension, new DistanceDimension(state.SolverModel, callback));
        
        SetupVehicleDistanceCosts(state, distanceDimension);
    }
    
    /// <summary>
    /// Sets up the vehicle distance costs.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="distanceDimension">The distance dimension.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> or <paramref name="distanceDimension"/> is <see langword="null"/>.</exception>
    private void SetupVehicleDistanceCosts(SolverState<Dimension> state, SolverDimension distanceDimension)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(distanceDimension, nameof(distanceDimension));

        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            var vehicle = state.SolverModel.Vehicles[i];
            var distanceCost = vehicle.DistanceCost;
            
            distanceDimension.RoutingDimension.SetSpanCostCoefficientForVehicle(distanceCost, i);
        }
    }
}