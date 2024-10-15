// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Represents a dimension in the solver.
/// </summary>
internal struct SolverDimension : IEquatable<SolverDimension>, IDisposable
{
    /// <summary>
    /// Gets the name of the dimension.
    /// </summary>
    internal string Name { get; }
    
    /// <summary>
    /// Gets the dimension.
    /// </summary>
    internal IDimension Dimension { get; }
    
    /// <summary>
    /// Gets the dimension.
    /// </summary>
    internal RoutingDimension RoutingDimension { get; }

    /// <summary>
    /// Gets a value indicating whether the dimension has been disposed.
    /// </summary>
    internal bool Disposed { get; private set; } = false;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverDimension"/> struct.
    /// </summary>
    /// <param name="name">The name of the routingDimension.</param>
    /// <param name="dimension">The dimension.</param>
    /// <param name="routingDimension">The routingDimension.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routingDimension"/> is null.</exception>
    public SolverDimension(string name, IDimension dimension, RoutingDimension routingDimension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        ArgumentNullException.ThrowIfNull(routingDimension, nameof(routingDimension));
        
        Name = name;
        Dimension = dimension;
        RoutingDimension = routingDimension;
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

    /// <inheritdoc/>
    public void Dispose()
    {
        if (Disposed)
        {
            return;
        }
        
        RoutingDimension.Dispose();
        Disposed = true;
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
    /// Converts the specified <see cref="SolverDimension"/> to a <see cref="Google.OrTools.ConstraintSolver.RoutingDimension"/>.
    /// </summary>
    /// <param name="dimension">The dimension to convert.</param>
    /// <returns>The dimension.</returns>
    public static implicit operator RoutingDimension (SolverDimension dimension)
    {
        return dimension.RoutingDimension;
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