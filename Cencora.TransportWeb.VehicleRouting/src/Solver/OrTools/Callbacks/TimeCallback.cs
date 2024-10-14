// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;

/// <summary>
/// Represents a callback that calculates the transit time between two nodes.
/// </summary>
internal class TimeCallback : ITransitCallback
{
    private readonly IReadOnlyDirectedRouteMatrix _routeMatrix;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeCallback"/> class.
    /// </summary>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routeMatrix"/> is <see langword="null"/>.</exception>
    public TimeCallback(IReadOnlyDirectedRouteMatrix routeMatrix)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));
        
        _routeMatrix = routeMatrix;
    }
    
    /// <inheritdoc/>
    public long GetTransit(Node fromNode, Node toNode)
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