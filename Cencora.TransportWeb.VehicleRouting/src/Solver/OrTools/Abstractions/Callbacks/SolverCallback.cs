// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// A solver callback.
/// </summary>
internal readonly struct SolverCallback : IEquatable<SolverCallback>
{
    /// <summary>
    /// Gets the callback function provider.
    /// </summary>
    internal ICallback Callback { get; }
    
    /// <summary>
    /// Gets the index of the solver callback.
    /// </summary>
    internal int Index { get; } = -1;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverCallback"/> struct.
    /// </summary>
    /// <param name="callback">The callback function</param>
    /// <param name="index">The callback function</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is negative.</exception>
    internal SolverCallback(ICallback callback, int index)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        
        Callback = callback;
        Index = index;
    }

    /// <inheritdoc/>
    public bool Equals(SolverCallback other)
    {
        return Index.Equals(other.Index);
    }
    
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is SolverCallback other && Equals(other);
    }
    
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Index.GetHashCode();
    }
    
    /// <inheritdoc/>
    public override string ToString()
    {
        return Index.ToString();
    }
    
    /// <summary>
    /// Implicitly converts a <see cref="SolverCallback"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="solverCallback">The callback to convert.</param>
    /// <returns>The index of the solverCallback.</returns>
    public static implicit operator int (SolverCallback solverCallback)
    {
        return solverCallback.Index;
    }
    
    /// <summary>
    /// Determines whether two <see cref="SolverCallback"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="SolverCallback"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverCallback"/> to compare.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(SolverCallback left, SolverCallback right)
    {
        return left.Equals(right);
    }
    
    /// <summary>
    /// Determines whether two <see cref="SolverCallback"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="SolverCallback"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverCallback"/> to compare.</param>
    /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(SolverCallback left, SolverCallback right)
    {
        return !left.Equals(right);
    }
}