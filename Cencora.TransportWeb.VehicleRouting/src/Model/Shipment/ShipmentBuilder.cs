// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;
using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.Shipment;

/// <summary>
/// A builder for the <see cref="Shipment"/> class.
/// </summary>
public sealed class ShipmentBuilder : FlaggedBuilder
{
    private Id _id;
    private HashSet<ShipUnit> _shipUnits = new();
    private Location? _pickupLocation;
    private Location? _deliveryLocation;
    private long? _pickupDropPenalty;
    private long? _deliveryDropPenalty;
    private long? _pickupHandlingTime;
    private ValueRange? _pickupTimeWindow;
    private long? _deliveryHandlingTime;
    private ValueRange? _deliveryTimeWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShipmentBuilder"/> class.
    /// </summary>
    public ShipmentBuilder()
    {
        _id = Id.New();
    }

    /// <summary>
    /// Adds a id to the shipment.
    /// </summary>
    /// <param name="id">The id to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithId(Id id)
    {
        _id = id;
        return this;
    }

    /// <summary>
    /// Adds an automatic id to the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithAutomaticId()
    {
        _id = Id.New();
        return this;
    }

    /// <summary>
    /// Adds a ship unit to the shipment.
    /// </summary>
    /// <param name="unit">The ship unit to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithShipUnit(ShipUnit unit)
    {
        _shipUnits.Add(unit);
        return this;
    }

    /// <summary>
    /// Adds a ship unit to the shipment.
    /// </summary>
    /// <param name="factory">The factory to create the ship unit.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">The factory is null.</exception>
    public ShipmentBuilder WithShipUnit(Func<ShipUnitBuilder, ShipUnit> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var builder = new ShipUnitBuilder();
        var unit = factory(builder);
        _shipUnits.Add(unit);
        return this;
    }

    /// <summary>
    /// Adds a ship unit to the shipment.
    /// </summary>
    /// <param name="factory">The factory to create the ship unit.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">The factory is null.</exception>
    public ShipmentBuilder WithShipUnit(Func<ShipUnit> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var unit = factory();
        _shipUnits.Add(unit);
        return this;
    }

    /// <summary>
    /// Adds ship units to the shipment.
    /// </summary>
    /// <param name="units">The ship units to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">The units are null.</exception>
    public ShipmentBuilder WithShipUnits(IEnumerable<ShipUnit> units)
    {
        ArgumentNullException.ThrowIfNull(units, nameof(units));

        foreach (var unit in units)
        {
            _shipUnits.Add(unit);
        }
        return this;
    }

    /// <summary>
    /// Removes a ship unit from the shipment.
    /// </summary>
    /// <param name="unit">The ship unit to remove.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithoutShipUnit(ShipUnit unit)
    {
        _shipUnits.Remove(unit);
        return this;
    }

    /// <summary>
    /// Removes ship units from the shipment.
    /// </summary>
    /// <param name="units">The ship units to remove.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">The units are null.</exception>
    public ShipmentBuilder WithoutShipUnits(IEnumerable<ShipUnit> units)
    {
        ArgumentNullException.ThrowIfNull(units, nameof(units));

        foreach (var unit in units)
        {
            _shipUnits.Remove(unit);
        }
        return this;
    }

    /// <summary>
    /// Removes all ship units from the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithoutShipUnits()
    {
        _shipUnits.Clear();
        return this;
    }

    /// <summary>
    /// Adds a pickup location to the shipment.
    /// </summary>
    /// <param name="location">The pickup location to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithPickupLocation(Location location)
    {
        _pickupLocation = location;
        return this;
    }

    /// <summary>
    /// Adds a pickup location to the shipment.
    /// </summary>
    /// <param name="factory">The factory to create the pickup location.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">The factory is null.</exception>
    public ShipmentBuilder WithPickupLocation(Func<LocationBuilder, Location> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var builder = new LocationBuilder();
        var location = factory(builder);
        _pickupLocation = location;
        return this;
    }

    /// <summary>
    /// Adds a pickup location to the shipment.
    /// </summary>
    /// <param name="factory">The factory to create the pickup location.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">The factory is null.</exception>
    public ShipmentBuilder WithPickupLocation(Func<Location> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var location = factory();
        _pickupLocation = location;
        return this;
    }

    /// <summary>
    /// Adds an arbitrary pickup location to the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithArbitraryPickupLocation()
    {
        _pickupLocation = null;
        return this;
    }

    /// <summary>
    /// Adds a delivery location to the shipment.
    /// </summary>
    /// <param name="location">The delivery location to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithDeliveryLocation(Location location)
    {
        _deliveryLocation = location;
        return this;
    }

    /// <summary>
    /// Adds a delivery location to the shipment.
    /// </summary>
    /// <param name="factory">The factory to create the delivery location.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">The factory is null.</exception>
    public ShipmentBuilder WithDeliveryLocation(Func<LocationBuilder, Location> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var builder = new LocationBuilder();
        var location = factory(builder);
        _deliveryLocation = location;
        return this;
    }

    /// <summary>
    /// Adds a delivery location to the shipment.
    /// </summary>
    /// <param name="factory">The factory to create the delivery location.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">The factory is null.</exception>
    public ShipmentBuilder WithDeliveryLocation(Func<Location> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var location = factory();
        _deliveryLocation = location;
        return this;
    }

    /// <summary>
    /// Adds an arbitrary delivery location to the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithArbitraryDeliveryLocation()
    {
        _deliveryLocation = null;
        return this;
    }

    /// <summary>
    /// Adds a pickup drop penalty to the shipment.
    /// </summary>
    /// <param name="penalty">The pickup drop penalty to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithPickupDropPenalty(long penalty)
    {
        _pickupDropPenalty = penalty;
        return this;
    }

    /// <summary>
    /// Removes the pickup drop penalty from the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithoutPickupDropPenalty()
    {
        _pickupDropPenalty = null;
        return this;
    }

    /// <summary>
    /// Adds a delivery drop penalty to the shipment.
    /// </summary>
    /// <param name="penalty">The delivery drop penalty to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithDeliveryDropPenalty(long penalty)
    {
        _deliveryDropPenalty = penalty;
        return this;
    }

    /// <summary>
    /// Adds a pickup handling time to the shipment.
    /// </summary>
    /// <param name="time">The pickup handling time to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithPickupHandlingTime(long time)
    {
        _pickupHandlingTime = time;
        return this;
    }

    /// <summary>
    /// Removes the pickup handling time from the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithoutPickupHandlingTime()
    {
        _pickupHandlingTime = null;
        return this;
    }

    /// <summary>
    /// Adds a delivery handling time to the shipment.
    /// </summary>
    /// <param name="time">The delivery handling time to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithDeliveryHandlingTime(long time)
    {
        _deliveryHandlingTime = time;
        return this;
    }

    /// <summary>
    /// Removes the delivery handling time from the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithoutDeliveryHandlingTime()
    {
        _deliveryHandlingTime = null;
        return this;
    }

    /// <summary>
    /// Adds a pickup time window to the shipment.
    /// </summary>
    /// <param name="window">The pickup time window to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithPickupTimeWindow(ValueRange window)
    {
        _pickupTimeWindow = window;
        return this;
    }

    /// <summary>
    /// Removes the pickup time window from the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithoutPickupTimeWindow()
    {
        _pickupTimeWindow = null;
        return this;
    }

    /// <summary>
    /// Adds a delivery time window to the shipment.
    /// </summary>
    /// <param name="window">The delivery time window to add.</param>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithDeliveryTimeWindow(ValueRange window)
    {
        _deliveryTimeWindow = window;
        return this;
    }

    /// <summary>
    /// Removes the delivery time window from the shipment.
    /// </summary>
    /// <returns>The current instance of the <see cref="ShipmentBuilder"/> class.</returns>
    public ShipmentBuilder WithoutDeliveryTimeWindow()
    {
        _deliveryTimeWindow = null;
        return this;
    }

    /// <summary>
    /// Builds the shipment.
    /// </summary>
    /// <returns>The shipment.</returns>
    public Shipment Build()
    {
        return new Shipment(_id, _shipUnits, _pickupLocation, _deliveryLocation, _pickupDropPenalty, _deliveryDropPenalty, _pickupHandlingTime, _pickupTimeWindow, _deliveryHandlingTime, _deliveryTimeWindow);
    }
}
