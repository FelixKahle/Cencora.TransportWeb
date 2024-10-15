// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;
using ArcCostEvaluatorCallback = Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks.ArcCostEvaluatorCallback;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;

/// <summary>
/// Represents a configurator that sets up the arc cost evaluator.
/// </summary>
internal sealed class ArcCostEvaluatorConfigurator : IConfigurator<Dimension>
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
    public void Configure(SolverState<Dimension> state)
    {
        state.SolverInterface.SetArcCostEvaluator(new ArcCostEvaluatorCallback(_routeMatrix));
    }
}