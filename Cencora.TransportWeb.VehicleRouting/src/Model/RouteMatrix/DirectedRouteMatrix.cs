// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Collections;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;

/// <summary>
/// A directed route matrix is a matrix that maps a pair of locations to a route edge.
/// </summary>
public sealed class DirectedRouteMatrix : IDirectedRouteMatrix
{
    private readonly Dictionary<LocationPair, IRouteEdge> _edges;

    /// <summary>
    /// Gets the edges of the route matrix.
    /// </summary>
    public HashSet<IRouteEdge> Edges => _edges
        .Values
        .ToHashSet();

    /// <summary>
    /// Gets the locations of the route matrix.
    /// </summary>
    public HashSet<Location> Locations => _edges
        .Keys
        .SelectMany(pair => new[] { pair.From, pair.To })
        .ToHashSet();

    /// <inheritdoc/>
    IReadOnlySet<IRouteEdge> IReadOnlyDirectedRouteMatrix.Edges => Edges;

    /// <inheritdoc/>
    IReadOnlySet<Location> IReadOnlyDirectedRouteMatrix.Locations => Locations;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectedRouteMatrix"/> class.
    /// </summary>
    public DirectedRouteMatrix()
    {
        _edges = new Dictionary<LocationPair, IRouteEdge>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectedRouteMatrix"/> class.
    /// </summary>
    /// <param name="edges">The edges of the route matrix.</param>
    public DirectedRouteMatrix(Dictionary<LocationPair, IRouteEdge> edges)
    {
        _edges = edges;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectedRouteMatrix"/> class.
    /// </summary>
    /// <param name="capacity">The initial capacity of the route matrix.</param>
    public DirectedRouteMatrix(int capacity)
    {
        _edges = new Dictionary<LocationPair, IRouteEdge>(capacity);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>Thrown when <paramref name="fromLocation"/> is <see langword="null"/>.
    public void AddEdge(Location fromLocation, Location toLocation, IRouteEdge edge)
    {
        ArgumentNullException.ThrowIfNull(fromLocation, nameof(fromLocation));
        ArgumentNullException.ThrowIfNull(toLocation, nameof(toLocation));

        _edges.TryAdd(new LocationPair(fromLocation, toLocation), edge);
    }

    /// <inheritdoc/>
    public IRouteEdge GetEdge(Location fromLocation, Location toLocation)
    {
        var edge = _edges.GetValueOrDefault(new LocationPair(fromLocation, toLocation));
        return edge ?? new UndefinedRouteEdge();
    }

    /// <inheritdoc/>
    public bool HasDefinedEdge(Location fromLocation, Location toLocation)
    {
        var edge = _edges.GetValueOrDefault(new LocationPair(fromLocation, toLocation));
        return edge switch
        {
            DefinedRouteEdge => true,
            _ => false
        };
    }

    /// <inheritdoc/>
    public IEnumerator<IRouteEdge> GetEnumerator()
    {
        return _edges
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
        return _edges.ContainsKey(pair);
    }

    /// <summary>
    /// Determines whether a pair of locations is contained in the route matrix.
    /// </summary>
    /// <param name="fromLocation">The start location.</param>
    /// <param name="toLocation">The end location.</param>
    /// <returns><see langword="true"/> if the pair of locations is contained in the route matrix; otherwise, <see langword="false"/>.</returns>
    public bool HasPair(Location fromLocation, Location toLocation)
    {
        return _edges.ContainsKey(new LocationPair(fromLocation, toLocation));
    }

    /// <summary>
    /// Removes the edge between two locations from the route matrix.
    /// </summary>
    public void Clear()
    {
        _edges.Clear();
    }
}
