// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Vehicles;

/// <summary>
/// Stores the start and end node of a vehicle.
/// </summary>
internal readonly struct VehicleNodeStore : IEquatable<VehicleNodeStore>
{
    /// <summary>
    /// The start node of the vehicle.
    /// </summary>
    internal VehicleNode StartNode { get; }

    /// <summary>
    /// The end node of the vehicle.
    /// </summary>
    internal VehicleNode EndNode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleNodeStore"/> struct.
    /// </summary>
    /// <param name="startNode">The start node of the vehicle.</param>
    /// <param name="endNode">The end node of the vehicle.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="startNode"/> or <paramref name="endNode"/> is <see langword="null"/>.</exception>
    internal VehicleNodeStore(VehicleNode startNode, VehicleNode endNode)
    {
        ArgumentNullException.ThrowIfNull(startNode, nameof(startNode));
        ArgumentNullException.ThrowIfNull(endNode, nameof(endNode));

        StartNode = startNode;
        EndNode = endNode;
    }

    /// <inheritdoc/>
    public bool Equals(VehicleNodeStore other)
    {
        return StartNode.Equals(other.StartNode) && EndNode.Equals(other.EndNode);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is VehicleNodeStore other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartNode, EndNode);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{StartNode} -> {EndNode}";
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="VehicleNodeStore"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleNodeStore"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleNodeStore"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> represent the same value; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(VehicleNodeStore left, VehicleNodeStore right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="VehicleNodeStore"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleNodeStore"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleNodeStore"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> do not represent the same value; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(VehicleNodeStore left, VehicleNodeStore right)
    {
        return !left.Equals(right);
    }
}
