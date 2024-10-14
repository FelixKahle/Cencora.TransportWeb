// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Represents a dimension in the solver.
/// </summary>
internal readonly struct SolverDimension : IEquatable<SolverDimension>
{
    /// <summary>
    /// Gets the name of the dimension.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Gets the dimension.
    /// </summary>
    public RoutingDimension Dimension { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverDimension"/> struct.
    /// </summary>
    /// <param name="name">The name of the dimension.</param>
    /// <param name="dimension">The dimension.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dimension"/> is null.</exception>
    public SolverDimension(string name, RoutingDimension dimension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        
        Name = name;
        Dimension = dimension;
    }

    /// <inheritdoc/>
    public bool Equals(SolverDimension other)
    {
        return Name.Equals(other.Name, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is SolverDimension other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Name.GetHashCode(StringComparison.Ordinal);
    }

    /// <summary>
    /// Converts the specified <see cref="SolverDimension"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="dimension">The dimension to convert.</param>
    /// <returns>The name of the dimension.</returns>
    public static implicit operator string(SolverDimension dimension)
    {
        return dimension.Name;
    }
    
    /// <summary>
    /// Converts the specified <see cref="SolverDimension"/> to a <see cref="RoutingDimension"/>.
    /// </summary>
    /// <param name="dimension">The dimension to convert.</param>
    /// <returns>The dimension.</returns>
    public static implicit operator RoutingDimension (SolverDimension dimension)
    {
        return dimension.Dimension;
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="SolverDimension"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="SolverDimension"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverDimension"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> represent the same dimension; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(SolverDimension left, SolverDimension right)
    {
        return left.Equals(right);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="SolverDimension"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="SolverDimension"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverDimension"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> do not represent the same dimension; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(SolverDimension left, SolverDimension right)
    {
        return !left.Equals(right);
    }
}