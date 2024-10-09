// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Builder for <see cref="VehicleStop"/> instances.
/// </summary>
public class VehicleStopBuilder
{
    private int _index;
    private Location? _location;
    private Vehicle _vehicle;
    private readonly HashSet<Shipment> _pickups = new();
    private readonly HashSet<Shipment> _deliveries = new();
    private ValueRange _arrivalTimeWindow;
    private ValueRange _departureTimeWindow;
    private ValueRange _waitingTime;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleStopBuilder"/> class.
    /// </summary>
    /// <param name="index">The index of the stop.</param>
    /// <param name="vehicle">The vehicle that stops at the stop.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than or equal to zero.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    public VehicleStopBuilder(int index, Vehicle vehicle)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(index, nameof(index));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        
        _index = index;
        _vehicle = vehicle;
    }
    
    /// <summary>
    /// Adds a index to the the stop.
    /// </summary>
    /// <param name="index">The index of the stop.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than or equal to zero.</exception>
    public VehicleStopBuilder WithIndex(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(index, nameof(index));
        
        _index = index;
        return this;
    }
    
    /// <summary>
    /// Adds a location to the stop.
    /// </summary>
    /// <param name="location">The location of the stop.</param>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithLocation(Location? location)
    {
        _location = location;
        return this;
    }
    
    /// <summary>
    /// Removes the location from the stop.
    /// </summary>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithoutLocation()
    {
        _location = null;
        return this;
    }
    
    /// <summary>
    /// Adds a vehicle to the stop.
    /// </summary>
    /// <param name="vehicle">The vehicle that stops at the stop.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    public VehicleStopBuilder WithVehicle(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        
        _vehicle = vehicle;
        return this;
    }
    
    /// <summary>
    /// Adds a pickup to the stop.
    /// </summary>
    /// <param name="pickup">The pickup to add.</param>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithPickup(Shipment? pickup)
    {
        if (pickup is null)
        {
            return this;
        }
        
        _pickups.Add(pickup);
        return this;
    }
    
    /// <summary>
    /// Adds a collection of pickups to the stop.
    /// </summary>
    /// <param name="pickups">The pickups to add.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pickups"/> is <see langword="null"/>.</exception>
    public VehicleStopBuilder WithPickups(IEnumerable<Shipment> pickups)
    {
        ArgumentNullException.ThrowIfNull(pickups, nameof(pickups));

        foreach (var pickup in pickups)
        {
            WithPickup(pickup);
        }

        return this;
    }
    
    /// <summary>
    /// Adds a collection of pickups to the stop.
    /// </summary>
    /// <param name="pickups">The pickups to add.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pickups"/> is <see langword="null"/>.</exception>
    public VehicleStopBuilder WithPickups(IReadOnlySet<Shipment> pickups)
    {
        ArgumentNullException.ThrowIfNull(pickups, nameof(pickups));

        foreach (var pickup in pickups)
        {
            WithPickup(pickup);
        }

        return this;
    }
    
    /// <summary>
    /// Adds a pickup to the stop.
    /// </summary>
    /// <param name="pickup">The pickup to add.</param>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithoutPickup(Shipment? pickup)
    {
        if (pickup is null)
        {
            return this;
        }
        
        _pickups.Remove(pickup);
        return this;
    }
    
    /// <summary>
    /// Removes a collection of pickups from the stop.
    /// </summary>
    /// <param name="pickups">The pickups to remove.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pickups"/> is <see langword="null"/>.</exception>
    public VehicleStopBuilder WithoutPickups(IEnumerable<Shipment> pickups)
    {
        ArgumentNullException.ThrowIfNull(pickups, nameof(pickups));

        foreach (var pickup in pickups)
        {
            WithoutPickup(pickup);
        }

        return this;
    }
    
    /// <summary>
    /// Removes all pickups from the stop.
    /// </summary>
    public VehicleStopBuilder WithoutPickups()
    {
        _pickups.Clear();
        return this;
    }
    
    /// <summary>
    /// Adds a delivery to the stop.
    /// </summary>
    /// <param name="delivery">The delivery to add.</param>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithDelivery(Shipment? delivery)
    {
        if (delivery is null)
        {
            return this;
        }
        
        _deliveries.Add(delivery);
        return this;
    }
    
    /// <summary>
    /// Adds a collection of deliveries to the stop.
    /// </summary>
    /// <param name="deliveries">The deliveries to add.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="deliveries"/> is <see langword="null"/>.</exception>
    public VehicleStopBuilder WithDeliveries(IEnumerable<Shipment> deliveries)
    {
        ArgumentNullException.ThrowIfNull(deliveries, nameof(deliveries));

        foreach (var delivery in deliveries)
        {
            WithDelivery(delivery);
        }

        return this;
    }
    
    /// <summary>
    /// Adds a collection of deliveries to the stop.
    /// </summary>
    /// <param name="deliveries">The deliveries to add.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="deliveries"/> is <see langword="null"/>.</exception>
    public VehicleStopBuilder WithDeliveries(IReadOnlySet<Shipment> deliveries)
    {
        ArgumentNullException.ThrowIfNull(deliveries, nameof(deliveries));

        foreach (var delivery in deliveries)
        {
            WithDelivery(delivery);
        }

        return this;
    }
    
    /// <summary>
    /// Removes a delivery from the stop.
    /// </summary>
    /// <param name="delivery">The delivery to remove.</param>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithoutDelivery(Shipment? delivery)
    {
        if (delivery is null)
        {
            return this;
        }
        
        _deliveries.Remove(delivery);
        return this;
    }
    
    /// <summary>
    /// Removes a collection of deliveries from the stop.
    /// </summary>
    /// <param name="deliveries">The deliveries to remove.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="deliveries"/> is <see langword="null"/>.</exception>
    public VehicleStopBuilder WithoutDeliveries(IEnumerable<Shipment> deliveries)
    {
        ArgumentNullException.ThrowIfNull(deliveries, nameof(deliveries));

        foreach (var delivery in deliveries)
        {
            WithoutDelivery(delivery);
        }

        return this;
    }
    
    /// <summary>
    /// Removes all deliveries from the stop.
    /// </summary>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithoutDeliveries()
    {
        _deliveries.Clear();
        return this;
    }
    
    /// <summary>
    /// Adds an arrival time window to the stop.
    /// </summary>
    /// <param name="arrivalTimeWindow">The arrival time window of the stop.</param>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithArrivalTimeWindow(ValueRange arrivalTimeWindow)
    {
        _arrivalTimeWindow = arrivalTimeWindow;
        return this;
    }
    
    /// <summary>
    /// Adds a departure time window to the stop.
    /// </summary>
    /// <param name="departureTimeWindow">The departure time window of the stop.</param>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithDepartureTimeWindow(ValueRange departureTimeWindow)
    {
        _departureTimeWindow = departureTimeWindow;
        return this;
    }
    
    /// <summary>
    /// Adds a waiting time to the stop.
    /// </summary>
    /// <param name="waitingTime">The waiting time of the stop.</param>
    /// <returns>The builder instance.</returns>
    public VehicleStopBuilder WithWaitingTime(ValueRange waitingTime)
    {
        _waitingTime = waitingTime;
        return this;
    }
    
    /// <summary>
    /// Builds a <see cref="VehicleStop"/> instance.
    /// </summary>
    /// <returns>The built <see cref="VehicleStop"/> instance.</returns>
    public VehicleStop Build()
    {
        return new VehicleStop(_index, _location, _vehicle, _pickups, _deliveries, _arrivalTimeWindow, _departureTimeWindow, _waitingTime);
    }
}