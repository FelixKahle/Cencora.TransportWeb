// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Collections;
using System.Collections.Immutable;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;

/// <summary>
/// An immutable route matrix is a matrix that maps a pair of locations to a route edge.
/// </summary>
public class ImmutableRouteMatrix : IEnumerable<IRouteEdge>
{
    /// <summary>
    /// Gets the edges of the route matrix.
    /// </summary>
    public ImmutableDictionary<LocationPair, IRouteEdge> Edges { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImmutableRouteMatrix"/> class.
    /// </summary>
    public ImmutableRouteMatrix()
    {
        Edges = ImmutableDictionary<LocationPair, IRouteEdge>.Empty;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ImmutableRouteMatrix"/> class.
    /// </summary>
    /// <param name="edges">The edges of the route matrix.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="edges"/> is <see langword="null"/>.</exception>
    public ImmutableRouteMatrix(ImmutableDictionary<LocationPair, IRouteEdge> edges)
    {
        ArgumentNullException.ThrowIfNull(edges, nameof(edges));
        
        Edges = edges;
    }
    
    /// <summary>
    /// Gets an edge between two locations.
    /// </summary>
    /// <param name="fromLocation">The start location.</param>
    /// <param name="toLocation">The end location.</param>
    /// <returns>The edge between the two locations.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromLocation"/> or <paramref name="toLocation"/> is <see langword="null"/>.</exception>
    public IRouteEdge GetEdge(Location fromLocation, Location toLocation)
    {
        ArgumentNullException.ThrowIfNull(fromLocation, nameof(fromLocation));
        ArgumentNullException.ThrowIfNull(toLocation, nameof(toLocation));
        
        var edge = Edges.GetValueOrDefault(new LocationPair(fromLocation, toLocation));
        return edge ?? new UndefinedRouteEdge();
    }
    
    /// <summary>
    /// Determines whether an edge is defined between two locations.
    /// </summary>
    /// <param name="fromLocation">The start location.</param>
    /// <param name="toLocation">The end location.</param>
    /// <returns><see langword="true"/> if an edge is defined between the two locations; otherwise, <see langword="false"/>.</returns>
    public bool HasDefinedEdge(Location fromLocation, Location toLocation)
    {
        var edge = Edges.GetValueOrDefault(new LocationPair(fromLocation, toLocation));
        return edge switch
        {
            DefinedRouteEdge => true,
            _ => false
        };
    }
    
    /// <inheritdoc/>
    public IEnumerator<IRouteEdge> GetEnumerator()
    {
        return Edges
            .Values
            .GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    /// <summary>
    /// Determines whether a pair of locations is contained in the route matrix.
    /// </summary>
    /// <param name="pair">The pair of locations to check.</param>
    /// <returns><see langword="true"/> if the pair of locations is contained in the route matrix; otherwise, <see langword="false"/>.</returns>
    public bool HasPair(LocationPair pair)
    {
        return Edges.ContainsKey(pair);
    }

    /// <summary>
    /// Determines whether a pair of locations is contained in the route matrix.
    /// </summary>
    /// <param name="fromLocation">The start location.</param>
    /// <param name="toLocation">The end location.</param>
    /// <returns><see langword="true"/> if the pair of locations is contained in the route matrix; otherwise, <see langword="false"/>.</returns>
    public bool HasPair(Location fromLocation, Location toLocation)
    {
        return Edges.ContainsKey(new LocationPair(fromLocation, toLocation));
    }
}