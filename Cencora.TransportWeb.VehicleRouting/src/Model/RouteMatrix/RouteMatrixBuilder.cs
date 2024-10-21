// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Collections.Immutable;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;

/// <summary>
/// A builder for creating a directed route matrix.
/// </summary>
public sealed class RouteMatrixBuilder
{
    private readonly Dictionary<LocationPair, IRouteEdge> _edges;

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteMatrixBuilder"/> class.
    /// </summary>
    public RouteMatrixBuilder()
    {
        _edges = new Dictionary<LocationPair, IRouteEdge>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteMatrixBuilder"/> class with the specified capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the route matrix.</param>
    public RouteMatrixBuilder(int capacity)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        _edges = new Dictionary<LocationPair, IRouteEdge>(adjustedCapacity);
    }

    /// <summary>
    /// Adds an edge to the route matrix.
    /// </summary>
    /// <param name="fromLocation">The location from which the edge starts.</param>
    /// <param name="toLocation">The location to which the edge leads.</param>
    /// <param name="edge">The edge to add.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromLocation"/>, <paramref name="toLocation"/>, or <paramref name="edge"/> is <see langword="null"/>.</exception>
    public RouteMatrixBuilder WithEdge(Location fromLocation, Location toLocation, IRouteEdge edge)
    {
        ArgumentNullException.ThrowIfNull(fromLocation, nameof(fromLocation));
        ArgumentNullException.ThrowIfNull(toLocation, nameof(toLocation));
        ArgumentNullException.ThrowIfNull(edge, nameof(edge));

        _edges.Add(new LocationPair(fromLocation, toLocation), edge);
        return this;
    }

    /// <summary>
    /// Adds an edge to the route matrix.
    /// </summary>
    /// <param name="pair">The pair of locations that the edge connects.</param>
    /// <param name="edge">The edge to add.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pair"/> or <paramref name="edge"/> is <see langword="null"/>.</exception>
    public RouteMatrixBuilder WithEdge(LocationPair pair, IRouteEdge edge)
    {
        ArgumentNullException.ThrowIfNull(edge, nameof(edge));

        return WithEdge(pair.From, pair.To, edge);
    }

    /// <summary>
    /// Removes an edge from the route matrix.
    /// </summary>
    /// <param name="fromLocation">The location from which the edge starts.</param>
    /// <param name="toLocation">The location to which the edge leads.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromLocation"/> or <paramref name="toLocation"/> is <see langword="null"/>.</exception>
    public RouteMatrixBuilder WithoutEdge(Location fromLocation, Location toLocation)
    {
        ArgumentNullException.ThrowIfNull(fromLocation, nameof(fromLocation));
        ArgumentNullException.ThrowIfNull(toLocation, nameof(toLocation));

        _edges.Remove(new LocationPair(fromLocation, toLocation));
        return this;
    }

    /// <summary>
    /// Removes an edge from the route matrix.
    /// </summary>
    /// <param name="pair">The pair of locations that the edge connects.</param>
    /// <returns>The builder.</returns>
    public RouteMatrixBuilder WithoutEdge(LocationPair pair)
    {
        return WithoutEdge(pair.From, pair.To);
    }
    
    /// <summary>
    /// Builds the route matrix.
    /// </summary>
    /// <returns>The route matrix.</returns>
    public ImmutableRouteMatrix BuildImmutable()
    {
        return new ImmutableRouteMatrix(_edges.ToImmutableDictionary());
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return "RouteMatrixBuilder";
    }
}
