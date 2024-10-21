// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.MathUtils;
using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;

/// <summary>
/// Arc cost evaluator callback.
/// </summary>
internal sealed class ArcCostEvaluatorCallback : IArcCostEvaluator
{
    private readonly ImmutableRouteMatrix _routeMatrix;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ArcCostEvaluatorCallback"/> class.
    /// </summary>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routeMatrix"/> is <see langword="null"/>.</exception>
    internal ArcCostEvaluatorCallback(ImmutableRouteMatrix routeMatrix)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));
        
        _routeMatrix = routeMatrix;
    }
    
    /// <inheritdoc/>
    public long GetCost(DummyVehicle vehicle, Node fromNode, Node toNode)
    {
        var distanceCost = vehicle.DistanceCost;
        var timeCost = vehicle.TimeCost;
        
        var distance = GetDistance(fromNode, toNode);
        var duration = GetDuration(fromNode, toNode);
        
        var totalDistanceCost = MathUtils.MultiplyOrDefault(distance, distanceCost, long.MaxValue);
        var totalDurationCost = MathUtils.MultiplyOrDefault(duration, timeCost, long.MaxValue);
        
        return MathUtils.AddOrDefault(totalDistanceCost, totalDurationCost, long.MaxValue);
    }

    /// <summary>
    /// Gets the distance between two nodes.
    /// </summary>
    /// <param name="fromNode">The from node.</param>
    /// <param name="toNode">The to node.</param>
    /// <returns>The distance between the two nodes.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromNode"/> is <see langword="null"/>.</exception>
    private long GetDistance(Node fromNode, Node toNode)
    {
        ArgumentNullException.ThrowIfNull(fromNode, nameof(fromNode));
        ArgumentNullException.ThrowIfNull(toNode, nameof(toNode));
        
        var fromLocation = fromNode.GetLocation();
        var toLocation = toNode.GetLocation();
        
        // Arbitrary nodes have a distance of 0.
        if (fromLocation is null || toLocation is null)
        {
            return 0;
        }
        
        // The distance between the same location is 0.
        if (fromLocation.Equals(toLocation))
        {
            return 0;
        }
        
        return _routeMatrix.GetEdge(fromLocation, toLocation) switch
        {
            DefinedRouteEdge definedRouteEdge => definedRouteEdge.Distance,
            _ => long.MaxValue
        };
    }

    /// <summary>
    /// Gets the duration between two nodes.
    /// </summary>
    /// <param name="fromNode">The from node.</param>
    /// <param name="toNode">The to node.</param>
    /// <returns>The duration between the two nodes.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromNode"/> or <paramref name="toNode"/> is <see langword="null"/>.</exception>
    private long GetDuration(Node fromNode, Node toNode)
    {
        ArgumentNullException.ThrowIfNull(fromNode, nameof(fromNode));
        ArgumentNullException.ThrowIfNull(toNode, nameof(toNode));
        
        var fromLocation = fromNode.GetLocation();
        var toLocation = toNode.GetLocation();
        
        // Arbitrary nodes have a duration of 0.
        if (fromLocation is null || toLocation is null)
        {
            return 0;
        }
        
        // The duration between the same location is 0.
        if (fromLocation.Equals(toLocation))
        {
            return 0;
        }
        
        return _routeMatrix.GetEdge(fromLocation, toLocation) switch
        {
            DefinedRouteEdge definedRouteEdge => definedRouteEdge.Duration,
            _ => long.MaxValue
        };
    }
}