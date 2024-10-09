// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Flags;
using Cencora.TransportWeb.Common.Id;
using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.Shipments;

/// <summary>
/// Represents a shipment.
/// </summary>
public sealed class Shipment : IEquatable<Shipment>
{
    /// <summary>
    /// Gets the id of the shipment.
    /// </summary>
    public Id Id { get; }

    /// <summary>
    /// Gets the ship units of the shipment.
    /// </summary>
    public IReadOnlySet<ShipUnit> ShipUnits { get; }

    /// <summary>
    /// Gets the pickup location of the shipment.
    /// </summary>
    public Location PickupLocation { get; }

    /// <summary>
    /// Gets the delivery location of the shipment.
    /// </summary>
    public Location DeliveryLocation { get; }

    /// <summary>
    /// Gets the penalty for not picking up the shipment.
    /// </summary>
    public long? PickupDropPenalty { get; }

    /// <summary>
    /// Gets the penalty for not delivering the shipment.
    /// </summary>
    public long? DeliveryDropPenalty { get; }

    /// <summary>
    /// Gets the pickup handling time of the shipment.
    /// </summary>
    public long? PickupHandlingTime { get; }

    /// <summary>
    /// Gets the time window for the pickup of the shipment.
    /// </summary>
    public ValueRange? PickupTimeWindow { get; }

    /// <summary>
    /// Gets the delivery handling time of the shipment.
    /// </summary>
    public long? DeliveryHandlingTime { get; }

    /// <summary>
    /// Gets the weight of the shipment.
    /// </summary>
    public long? ShipmentWeight { get; }

    /// <summary>
    /// Gets the time window for the delivery of the shipment.
    /// </summary>
    public ValueRange? DeliveryTimeWindow { get; }

    /// <summary>
    /// Gets the flags of the shipment.
    /// </summary>
    public IReadOnlyFlagContainer Flags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="id">The id of the shipment.</param>
    /// <param name="pickupLocation">The pickup location of the shipment.</param>
    /// <param name="deliveryLocation">The delivery location of the shipment.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="pickupLocation"/> or <paramref name="deliveryLocation"/> is <see langword="null"/>.</exception>
    public Shipment(Id id, Location pickupLocation, Location deliveryLocation)
    {
        ArgumentNullException.ThrowIfNull(pickupLocation, nameof(pickupLocation));
        ArgumentNullException.ThrowIfNull(deliveryLocation, nameof(deliveryLocation));
        
        Id = id;
        PickupLocation = pickupLocation;
        DeliveryLocation = deliveryLocation;
        ShipUnits = new HashSet<ShipUnit>();
        Flags = new FlagContainer();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="id">The id of the shipment.</param>
    /// <param name="shipUnits">The ship units of the shipment.</param>
    /// <param name="pickupLocation">The pickup location of the shipment.</param>
    /// <param name="deliveryLocation">The delivery location of the shipment.</param>
    /// <param name="shipmentWeight">The weight of the shipment.</param>
    /// <param name="pickupDropPenalty">The penalty for not picking up the shipment.</param>
    /// <param name="deliveryDropPenalty">The penalty for not delivering the shipment.</param>
    /// <param name="pickupHandlingTime">The pickup handling time of the shipment.</param>
    /// <param name="pickupTimeWindow">The time window for the pickup of the shipment.</param>
    /// <param name="deliveryHandlingTime">The delivery handling time of the shipment.</param>
    /// <param name="deliveryTimeWindow">The time window for the delivery of the shipment.</param>
    /// <param name="flags">The flags of the shipment.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="pickupLocation"/> or <paramref name="deliveryLocation"/> is <see langword="null"/>.</exception>
    public Shipment(Id id, IReadOnlySet<ShipUnit>? shipUnits, Location pickupLocation, Location deliveryLocation, long? shipmentWeight,
        long? pickupDropPenalty, long? deliveryDropPenalty, long? pickupHandlingTime, ValueRange? pickupTimeWindow,
        long? deliveryHandlingTime, ValueRange? deliveryTimeWindow, IReadOnlyFlagContainer? flags)
    {
        ArgumentNullException.ThrowIfNull(pickupLocation, nameof(pickupLocation));
        ArgumentNullException.ThrowIfNull(deliveryLocation, nameof(deliveryLocation));
        
        Id = id;
        ShipUnits = shipUnits ?? new HashSet<ShipUnit>();
        PickupLocation = pickupLocation;
        DeliveryLocation = deliveryLocation;
        ShipmentWeight = shipmentWeight is null ? null : Math.Max(0, shipmentWeight.Value);
        PickupDropPenalty = pickupDropPenalty;
        DeliveryDropPenalty = deliveryDropPenalty;
        PickupHandlingTime = pickupHandlingTime;
        PickupTimeWindow = pickupTimeWindow;
        DeliveryHandlingTime = deliveryHandlingTime;
        DeliveryTimeWindow = deliveryTimeWindow;
        Flags = flags ?? new FlagContainer();
    }

    /// <inheritdoc/>
    public bool Equals(Shipment? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Shipment other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Id.ToString();
    }

    /// <summary>
    /// Converts a <see cref="Shipment"/> to an <see cref="Id"/>.
    /// </summary>
    /// <param name="shipment">The <see cref="Shipment"/> to convert.</param>
    /// <returns>The <see cref="Id"/> of the <see cref="Shipment"/>.</returns>
    public static implicit operator Id(Shipment shipment)
    {
        return shipment.Id;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Shipment"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Shipment"/> to compare.</param>
    /// <param name="right">The second <see cref="Shipment"/> to compare.</param>
    /// <returns><see langword="true"/> if the two <see cref="Shipment"/> instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Shipment? left, Shipment? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Shipment"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Shipment"/> to compare.</param>
    /// <param name="right">The second <see cref="Shipment"/> to compare.</param>
    /// <returns><see langword="true"/> if the two <see cref="Shipment"/> instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Shipment? left, Shipment? right)
    {
        return !Equals(left, right);
    }
}
