// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;

/// <summary>
/// Represents a configurator that sets up the arc cost evaluator.
/// </summary>
internal sealed class ArcCostEvaluatorConfigurator : IConfigurator
{
    private readonly IReadOnlyDirectedRouteMatrix _routeMatrix;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ArcCostEvaluatorConfigurator"/> class.
    /// </summary>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routeMatrix"/> is <see langword="null"/>.</exception>
    internal ArcCostEvaluatorConfigurator(IReadOnlyDirectedRouteMatrix routeMatrix)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));
        
        _routeMatrix = routeMatrix;
    }
    
    /// <inheritdoc/>
    public void Configure(SolverState state)
    {
        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            var vehicle = state.SolverModel.Vehicles[i];
            
            var distanceCost = vehicle.DistanceCost;
            var timeCost = vehicle.TimeCost;
            
            var callbackClass = new ArcCostEvaluatorCallback(_routeMatrix, distanceCost, timeCost);
            var callback = state.SolverInterface.RegisterCallback(callbackClass);
            
            state.SolverInterface.RoutingModel.SetArcCostEvaluatorOfVehicle(callback, i);
        }
    }
}