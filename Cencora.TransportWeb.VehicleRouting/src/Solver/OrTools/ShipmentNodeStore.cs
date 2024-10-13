// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Stores the pickup and delivery node of a shipment.
/// </summary>
internal readonly struct ShipmentNodeStore : IEquatable<ShipmentNodeStore>
{
    /// <summary>
    /// Gets the pickup node.
    /// </summary>
    public Node Pickup { get; }

    /// <summary>
    /// Gets the delivery node.
    /// </summary>
    public Node Delivery { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShipmentNodeStore"/> struct.
    /// </summary>
    /// <param name="pickup">The pickup node.</param>
    /// <param name="delivery">The delivery node.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pickup"/> or <paramref name="delivery"/> is null.</exception>
    public ShipmentNodeStore(Node pickup, Node delivery)
    {
        ArgumentNullException.ThrowIfNull(pickup, nameof(pickup));
        ArgumentNullException.ThrowIfNull(delivery, nameof(delivery));

        Pickup = pickup;
        Delivery = delivery;
    }

    /// <inheritdoc />
    public bool Equals(ShipmentNodeStore other)
    {
        return Pickup.Equals(other.Pickup) && Delivery.Equals(other.Delivery);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ShipmentNodeStore other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Pickup, Delivery);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Pickup} -> {Delivery}";
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="ShipmentNodeStore"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="ShipmentNodeStore"/> to compare.</param>
    /// <param name="right">The second <see cref="ShipmentNodeStore"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> represent the same value; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ShipmentNodeStore left, ShipmentNodeStore right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="ShipmentNodeStore"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="ShipmentNodeStore"/> to compare.</param>
    /// <param name="right">The second <see cref="ShipmentNodeStore"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> do not represent the same value; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ShipmentNodeStore left, ShipmentNodeStore right)
    {
        return !left.Equals(right);
    }
}
