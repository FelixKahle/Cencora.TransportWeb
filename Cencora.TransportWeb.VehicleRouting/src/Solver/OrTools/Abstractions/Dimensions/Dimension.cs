// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Represents a dimension in the routing model.
/// </summary>
internal readonly struct Dimension : IEquatable<Dimension>, IDisposable
{
    /// <summary>
    /// The name of the dimension.
    /// </summary>
    internal string Name { get; }
    
    /// <summary>
    /// The internal routing dimension.
    /// </summary>
    internal RoutingDimension InternalDimension { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dimension"/> struct.
    /// </summary>
    /// <param name="name">The name of the dimension.</param>
    /// <param name="internalDimension">The internal routing dimension.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is <see langword="null"/> or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="internalDimension"/> is <see langword="null"/>.</exception>
    internal Dimension(string name, RoutingDimension internalDimension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentNullException.ThrowIfNull(internalDimension, nameof(internalDimension));
        
        Name = name;
        InternalDimension = internalDimension;
    }

    /// <inheritdoc/>
    public bool Equals(Dimension other)
    {
        return Name.Equals(other.Name, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Dimension other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        InternalDimension.Dispose();
    }
}