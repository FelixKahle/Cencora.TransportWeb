// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

internal abstract class Node : IEquatable<Node>
{
    /// <summary>
    /// The index of the node in the list of nodes of the solver.
    /// </summary>
    internal int Index { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Node"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative.</exception>
    internal Node(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));

        Index = index;
    }

    /// <inheritdoc/>
    public bool Equals(Node? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Index.Equals(other.Index);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Node other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Index;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Node: {Index}";
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Node"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Node"/> to compare.</param>
    /// <param name="right">The second <see cref="Node"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> represent the same <see cref="Node"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Node? left, Node? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Node"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Node"/> to compare.</param>
    /// <param name="right">The second <see cref="Node"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> do not represent the same <see cref="Node"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Node? left, Node? right)
    {
        return !Equals(left, right);
    }
}
