// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;

/// <summary>
/// A read-only directed route matrix is a matrix that maps a pair of locations to a route edge.
/// </summary>
public interface IReadOnlyDirectedRouteMatrix
{
    /// <summary>
    /// Gets the locations of the route matrix.
    /// </summary>
    public IReadOnlySet<IRouteEdge> Edges { get; }

    /// <summary>
    /// Gets the edges of the route matrix.
    /// </summary>
    public IReadOnlySet<Location> Locations { get; }

    /// <summary>
    /// Gets the route edge between two locations.
    /// </summary>
    /// <remarks>
    /// If no route edge exists between the two locations, an undefined route edge is returned.
    /// </remarks>
    /// <param name="fromLocation">The start location.</param>
    /// <param name="toLocation">The end location.</param>
    /// <returns>The route edge between the two locations.</returns>
    public IRouteEdge GetEdge(Location fromLocation, Location toLocation);

    /// <summary>
    /// Determines whether a route edge is defined between two locations.
    /// </summary>
    /// <param name="fromLocation">The start location.</param>
    /// <param name="toLocation">The end location.</param>
    /// <returns><see langword="true"/> if a route edge is defined between the two locations; otherwise, <see langword="false"/>.</returns>
    public bool HasDefinedEdge(Location fromLocation, Location toLocation);
}
