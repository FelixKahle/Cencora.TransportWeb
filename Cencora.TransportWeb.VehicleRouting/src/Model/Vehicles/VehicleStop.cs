// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Model;

/// <summary>
/// Represents a stop of a vehicle.
/// </summary>
public sealed class VehicleStop
{
    /// <summary>
    /// Gets the index of the vehicle stop.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets the location of the vehicle stop.
    /// </summary>
    public Location Location { get; }

    /// <summary>
    /// Gets the vehicle of the vehicle stop.
    /// </summary>
    public Vehicle Vehicle { get; }

    /// <summary>
    /// Gets the pickups of the vehicle stop.
    /// </summary>
    public IReadOnlySet<Shipment> Pickups { get; }

    /// <summary>
    /// Gets the deliveries of the vehicle stop.
    /// </summary>
    public IReadOnlySet<Shipment> Deliveries { get; }

    /// <summary>
    /// Gets the arrival time window of the vehicle stop.
    /// </summary>
    public ValueRange ArrivalTimeWindow { get; }

    /// <summary>
    /// Gets the departure time window of the vehicle stop.
    /// </summary>
    public ValueRange DepartureTimeWindow { get; }

    /// <summary>
    /// Gets the waiting time of the vehicle stop.
    /// </summary>
    public long WaitingTime { get; }

    /// <summary>
    /// Gets the total cost of the vehicle stop.
    /// </summary>
    public long StopCost { get; }

    /// <summary>
    /// Gets the total service time of handling the pickups.
    /// </summary>
    public long PickupsHandlingTime => Pickups.Sum(p => p.PickupHandlingTime ?? 0);

    /// <summary>
    /// Gets the total service time of handling the deliveries.
    /// </summary>
    public long DeliveriesHandlingTime => Deliveries.Sum(p => p.DeliveryHandlingTime ?? 0);

    /// <summary>
    /// Gets the total service time of handling the pickups and deliveries.
    /// </summary>
    public long TotalHandlingTime => PickupsHandlingTime + DeliveriesHandlingTime;

    /// <summary>
    /// Gets the total time the vehicle is stopped at the vehicle stop.
    /// </summary>
    public long TotalStopTime => TotalHandlingTime + WaitingTime;

    /// <summary>
    /// Creates a new instance of the <see cref="VehicleStop"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle stop.</param>
    /// <param name="location">The location of the vehicle stop.</param>
    /// <param name="vehicle">The vehicle of the vehicle stop.</param>
    /// <param name="pickups">The pickups of the vehicle stop.</param>
    /// <param name="deliveries">The deliveries of the vehicle stop.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the vehicle stop.</param>
    /// <param name="departureTimeWindow">The departure time window of the vehicle stop.</param>
    /// <param name="waitingTime">The waiting time of the vehicle stop.</param>
    /// <param name="stopCost">The total cost of the vehicle stop.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="location"/>, <paramref name="vehicle"/>, <paramref name="pickups"/> or <paramref name="deliveries"/> is null.</exception>
    public VehicleStop(int index, Location location, Vehicle vehicle, IReadOnlySet<Shipment> pickups, IReadOnlySet<Shipment> deliveries, ValueRange arrivalTimeWindow, ValueRange departureTimeWindow, long waitingTime, long stopCost)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(location, nameof(location));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(pickups, nameof(pickups));
        ArgumentNullException.ThrowIfNull(deliveries, nameof(deliveries));

        Index = index;
        Location = location;
        Vehicle = vehicle;
        Pickups = pickups;
        Deliveries = deliveries;
        ArrivalTimeWindow = arrivalTimeWindow;
        DepartureTimeWindow = departureTimeWindow;
        WaitingTime = waitingTime;
        StopCost = stopCost;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Stop {Index} of vehicle {Vehicle.Id} at {Location.Id}";
    }
}
