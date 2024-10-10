// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;

/// <summary>
/// Enumerates the types of shipment nodes.
/// </summary>
internal enum ShipmentNodeType
{
    /// <summary>
    /// A pickup node.
    /// </summary>
    Pickup,

    /// <summary>
    /// A delivery node.
    /// </summary>
    Delivery
}

/// <summary>
/// Represents a node associated with a shipment.
/// </summary>
internal sealed class ShipmentNode : Node
{
    /// <summary>
    /// The shipment associated with the node.
    /// </summary>
    public Shipment Shipment { get; }

    /// <summary>
    /// The location of the node, <see langword="null"/> if the node is not associated with a location.
    /// </summary>
    public Location? Location { get; }

    /// <summary>
    /// The type of the node.
    /// </summary>
    public ShipmentNodeType Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShipmentNode"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <param name="shipment">The shipment associated with the node.</param>
    /// <param name="location">The location of the node.</param>
    /// <param name="type">The type of the node.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="shipment"/> is <see langword="null"/>.</exception>
    public ShipmentNode(int index, Shipment shipment, Location? location, ShipmentNodeType type)
        : base(index)
    {
        ArgumentNullException.ThrowIfNull(shipment, nameof(shipment));

        Shipment = shipment;
        Location = location;
        Type = type;
    }

    /// <inheritdoc/>
    internal override Location? GetLocation()
    {
        return Type switch
        {
            ShipmentNodeType.Pickup => Shipment.PickupLocation,
            ShipmentNodeType.Delivery => Shipment.DeliveryLocation,
            _ => null
        };
    }

    /// <inheritdoc/>
    internal override Shipment GetShipment()
    {
        return Shipment;
    }

    /// <inheritdoc/>
    internal override Shipment? GetPickup()
    {
        return Type switch
        {
            ShipmentNodeType.Pickup => Shipment,
            ShipmentNodeType.Delivery => null,
            _ => null
        };
    }

    /// <inheritdoc/>
    internal override Shipment? GetDelivery()
    {
        return Type switch
        {
            ShipmentNodeType.Pickup => null,
            ShipmentNodeType.Delivery => Shipment,
            _ => null
        };
    }

    /// <inheritdoc/>
    internal override DummyVehicle? GetDummyVehicle()
    {
        return null;
    }

    /// <inheritdoc/>
    internal override long GetWeightDemand()
    {
        return Type switch
        {
            ShipmentNodeType.Pickup => Shipment.ShipmentWeight ?? 0,
            // We deliver the shipment, so the weight is negative.
            ShipmentNodeType.Delivery => (Shipment.ShipmentWeight * -1) ?? 0,
            _ => 0
        };
    }

    /// <inheritdoc/>
    internal override long GetTimeDemand()
    {
        return Type switch
        {
            ShipmentNodeType.Pickup => Shipment.PickupHandlingTime ?? 0,
            ShipmentNodeType.Delivery => Shipment.DeliveryHandlingTime ?? 0,
            _ => 0
        };
    }

    /// <inheritdoc/>
    internal override ValueRange? GetTimeWindow()
    {
        return Type switch
        {
            ShipmentNodeType.Pickup => Shipment.PickupTimeWindow,
            ShipmentNodeType.Delivery => Shipment.DeliveryTimeWindow,
            _ => null
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Location is null
            ? $"{Type} node for shipment {Shipment.Id}"
            : $"{Type} node for shipment {Shipment.Id} at {Location}";
    }
}
