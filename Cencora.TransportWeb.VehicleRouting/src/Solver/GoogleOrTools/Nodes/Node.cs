// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Represents a node in the vehicle routing problem.
/// </summary>
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
    /// Gets the location of the node.
    /// </summary>
    /// <returns>The location of the node.</returns>
    /// <remarks>
    /// This can be <see langword="null"/>. This will make the node
    /// to an arbitrary node.
    /// </remarks>
    internal abstract Location? GetLocation();

    /// <summary>
    /// Gets a value indicating whether the node is arbitrary.
    /// </summary>
    /// <returns><see langword="true"/> if the node is arbitrary; otherwise, <see langword="false"/>.</returns>
    internal bool IsArbitrary => GetLocation() is null;

    /// <summary>
    /// Gets the shipment of the node.
    /// </summary>
    /// <returns>The shipment of the node.</returns>
    /// <remarks>
    /// This can be <see langword="null"/>. If this returns <see langword="null"/>,
    /// the node does not have a shipment.
    /// </remarks>
    internal abstract Shipment? GetShipment();

    /// <summary>
    /// Gets a value indicating whether the node has a shipment.
    /// </summary>
    /// <returns><see langword="true"/> if the node has a shipment; otherwise, <see langword="false"/>.</returns>
    internal bool HasShipment => GetShipment() is not null;

    /// <summary>
    /// Gets the dummy vehicle of the node.
    /// </summary>
    /// <returns>The dummy vehicle of the node.</returns>
    /// <remarks>
    /// This can be <see langword="null"/>. If this returns <see langword="null"/>,
    /// the node does not have a dummy vehicle.
    /// </remarks>
    internal abstract DummyVehicle? GetDummyVehicle();

    /// <summary>
    /// Gets a value indicating whether the node has a dummy vehicle.
    /// </summary>
    /// <returns><see langword="true"/> if the node has a dummy vehicle; otherwise, <see langword="false"/>.</returns>
    internal bool HasDummyVehicle => GetDummyVehicle() is not null;

    /// <summary>
    /// Gets the demand of the node.
    /// </summary>
    /// <returns>The demand of the node.</returns>
    /// <remarks>
    /// This can be negative if a shipment is dropped off at the node.
    /// </remarks>
    internal abstract long GetWeightDemand();

    /// <summary>
    /// Gets the time demand of the node.
    /// </summary>
    internal abstract long GetTimeDemand();

    /// <summary>
    /// Gets the time window of the node.
    /// </summary>
    /// <returns>The time window of the node.</returns>
    /// <remarks>
    /// This can be <see langword="null"/>. If this returns <see langword="null"/>,
    /// the node does not have a time window.
    /// </remarks>
    internal abstract ValueRange? GetTimeWindow();

    /// <summary>
    /// Gets a value indicating whether the node has a time window.
    /// </summary>
    /// <returns><see langword="true"/> if the node has a time window; otherwise, <see langword="false"/>.</returns>
    internal bool HasTimeWindow => GetTimeWindow() is not null;

    /// <summary>
    /// Converts a <see cref="Node"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="node">The <see cref="Node"/> to convert.</param>
    /// <returns>The index of the <see cref="Node"/>.</returns>
    public static implicit operator int(Node node)
    {
        return node.Index;
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
