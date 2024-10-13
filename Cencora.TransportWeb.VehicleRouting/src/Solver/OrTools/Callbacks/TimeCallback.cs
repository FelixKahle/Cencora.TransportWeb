// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;

/// <summary>
/// Represents a callback for the time.
/// </summary>
internal sealed class TimeCallback : TransitCallbackBase
{
    /// <summary>
    /// The route matrix.
    /// </summary>
    private readonly IReadOnlyDirectedRouteMatrix _routeMatrix;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeCallback"/> class.
    /// </summary>
    /// <param name="registrant">The callback registrant.</param>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="registrant"/> or <paramref name="routeMatrix"/> is null.</exception>
    internal TimeCallback(ICallbackRegistrant registrant, IReadOnlyDirectedRouteMatrix routeMatrix) 
        : base(registrant)
    {
        ArgumentNullException.ThrowIfNull(registrant, nameof(registrant));
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));
        
        _routeMatrix = routeMatrix;
    }
    
    /// <inheritdoc/>
    public override long Callback(Node from, Node to)
    {
        ArgumentNullException.ThrowIfNull(from, nameof(from));
        ArgumentNullException.ThrowIfNull(to, nameof(to));
        
        var fromLocation = from.GetLocation();
        var toLocation = to.GetLocation();
        
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