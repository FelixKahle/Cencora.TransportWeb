// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// A callback.
/// </summary>
internal readonly struct Callback : IEquatable<Callback>
{
    /// <summary>
    /// Gets the index of the callback.
    /// </summary>
    internal int Index { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Callback"/> struct.
    /// </summary>
    /// <param name="index">The index of the callback.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is negative.</exception>
    internal Callback(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        
        Index = index;
    }

    /// <inheritdoc/>
    public bool Equals(Callback other)
    {
        return Index.Equals(other.Index);
    }
    
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Callback other && Equals(other);
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
    /// Implicitly converts a <see cref="Callback"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="callback">The callback to convert.</param>
    /// <returns>The index of the callback.</returns>
    public static implicit operator int (Callback callback)
    {
        return callback.Index;
    }
    
    /// <summary>
    /// Determines whether two <see cref="Callback"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Callback"/> to compare.</param>
    /// <param name="right">The second <see cref="Callback"/> to compare.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Callback left, Callback right)
    {
        return left.Equals(right);
    }
    
    /// <summary>
    /// Determines whether two <see cref="Callback"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Callback"/> to compare.</param>
    /// <param name="right">The second <see cref="Callback"/> to compare.</param>
    /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Callback left, Callback right)
    {
        return !left.Equals(right);
    }
}