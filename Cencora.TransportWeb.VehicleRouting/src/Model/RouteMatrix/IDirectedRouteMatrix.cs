// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;

/// <summary>
/// Interface for a directed route matrix.
/// </summary>
public interface IDirectedRouteMatrix : IEnumerable<IRouteEdge>, IReadOnlyDirectedRouteMatrix
{
    /// <summary>
    /// Adds a route edge to the route matrix.
    /// </summary>
    /// <param name="fromLocation">The location from which the edge starts.</param>
    /// <param name="toLocation">The location to which the edge leads.</param>
    /// <param name="edge">The edge to add.</param>
    public void AddEdge(Location fromLocation, Location toLocation, IRouteEdge edge);
}
