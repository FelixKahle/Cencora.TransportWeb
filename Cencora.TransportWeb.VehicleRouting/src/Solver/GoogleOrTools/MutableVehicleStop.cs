// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Represents a mutable vehicle stop.
/// </summary>
/// <remarks>
/// This is used to create and modify vehicle stops in the solver.
/// It is later converted to an immutable vehicle stop.
/// </remarks>
internal sealed class MutableVehicleStop : IEquatable<MutableVehicleStop>, IComparable<MutableVehicleStop>
{
    /// <summary>
    /// Gets the index of the vehicle stop.
    /// </summary>
    internal int Index { get; }

    /// <summary>
    /// Gets the location of the vehicle stop.
    /// </summary>
    internal Location Location { get; }

    /// <summary>
    /// Gets the vehicle of the vehicle stop.
    /// </summary>
    internal Vehicle Vehicle { get; }

    /// <summary>
    /// Gets the pickups of the vehicle stop.
    /// </summary>
    internal HashSet<Shipment> Pickups { get; }

    /// <summary>
    /// Gets the deliveries of the vehicle stop.
    /// </summary>
    internal HashSet<Shipment> Deliveries { get; }

    /// <summary>
    /// Gets the arrival time window of the vehicle stop.
    /// </summary>
    internal ValueRange ArrivalTimeWindow;

    /// <summary>
    /// Gets the departure time window of the vehicle stop.
    /// </summary>
    internal ValueRange DepartureTimeWindow;

    /// <summary>
    /// Gets the waiting time of the vehicle stop.
    /// </summary>
    internal ValueRange WaitingTime;

    /// <summary>
    /// Gets the total service time of handling the pickups.
    /// </summary>
    internal long PickupsHandlingTime => Pickups.Sum(p => p.PickupHandlingTime ?? 0);

    /// <summary>
    /// Gets the total service time of handling the deliveries.
    /// </summary>
    internal long DeliveriesHandlingTime => Deliveries.Sum(p => p.DeliveryHandlingTime ?? 0);

    /// <summary>
    /// Gets the total service time of handling the pickups and deliveries.
    /// </summary>
    internal long TotalHandlingTime => PickupsHandlingTime + DeliveriesHandlingTime;

    /// <summary>
    /// Gets the total time the vehicle is stopped at the vehicle stop.
    /// </summary>
    public ValueRange TotalStopTime => TotalHandlingTime + WaitingTime;
    
    /// <summary>
    /// Creates a new instance of the <see cref="MutableVehicleStop"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle stop.</param>
    /// <param name="location">The location of the vehicle stop.</param>
    /// <param name="vehicle">The vehicle of the vehicle stop.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the vehicle stop.</param>
    /// <param name="departureTimeWindow">The departure time window of the vehicle stop.</param>
    /// <param name="waitingTime">The waiting time of the vehicle stop.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <see paramref="index"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <see paramref="location"/> or <see paramref="vehicle"/> is null.</exception>
    public MutableVehicleStop(int index, Location location, Vehicle vehicle, ValueRange arrivalTimeWindow, ValueRange departureTimeWindow, ValueRange waitingTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(location, nameof(location));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

        Index = index;
        Location = location;
        Vehicle = vehicle;
        Pickups = new HashSet<Shipment>();
        Deliveries = new HashSet<Shipment>();
        ArrivalTimeWindow = arrivalTimeWindow;
        DepartureTimeWindow = departureTimeWindow;
        WaitingTime = waitingTime;
    }
    
    /// <summary>
    /// Creates a new instance of the <see cref="MutableVehicleStop"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle stop.</param>
    /// <param name="location">The location of the vehicle stop.</param>
    /// <param name="vehicle">The vehicle of the vehicle stop.</param>
    /// <param name="pickups">The pickups of the vehicle stop.</param>
    /// <param name="deliveries">The deliveries of the vehicle stop.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the vehicle stop.</param>
    /// <param name="departureTimeWindow">The departure time window of the vehicle stop.</param>
    /// <param name="waitingTime">The waiting time of the vehicle stop.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <see paramref="index"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <see paramref="location"/>, <see paramref="vehicle"/>, <see paramref="pickups"/> or <see paramref="deliveries"/> is null.</exception>
    public MutableVehicleStop(int index, Location location, Vehicle vehicle, HashSet<Shipment> pickups, HashSet<Shipment> deliveries, ValueRange arrivalTimeWindow, ValueRange departureTimeWindow, ValueRange waitingTime)
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
    }

    /// <inheritdoc />
    public bool Equals(MutableVehicleStop? other)
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

    /// <inheritdoc />
    public int CompareTo(MutableVehicleStop? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        return Index.CompareTo(other.Index);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is MutableVehicleStop other && Equals(other);
    }
    
    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Index;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Stop {Index} of vehicle {Vehicle.Id} at {Location}";
    }
    
    /// <summary>
    /// Converts the mutable vehicle stop to a vehicle stop.
    /// </summary>
    /// <returns>The vehicle stop.</returns>
    public VehicleStop ToVehicleStop()
    {
        return new VehicleStop(Index, Location, Vehicle, Pickups, Deliveries, ArrivalTimeWindow, DepartureTimeWindow, WaitingTime);
    }
    
    /// <summary>
    /// Converts the mutable vehicle stop to a int.
    /// </summary>
    /// <param name="stop">The mutable vehicle stop.</param>
    /// <returns>The index of the mutable vehicle stop.</returns>
    public static implicit operator int(MutableVehicleStop stop)
    {
        return stop.Index;
    }
    
    /// <summary>
    /// Converts the mutable vehicle stop to a vehicle stop.
    /// </summary>
    /// <param name="stop">The mutable vehicle stop.</param>
    /// <returns>The vehicle stop.</returns>
    public static implicit operator VehicleStop(MutableVehicleStop stop)
    {
        return stop.ToVehicleStop();
    }
    
    /// <summary>
    /// Determines whether two mutable vehicle stops are equal.
    /// </summary>
    /// <param name="left">The first mutable vehicle stop.</param>
    /// <param name="right">The second mutable vehicle stop.</param>
    /// <returns><see langword="true"/> if the mutable vehicle stops are equal, <see langword="false"/> otherwise.</returns>
    public static bool operator ==(MutableVehicleStop? left, MutableVehicleStop? right)
    {
        return Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether two mutable vehicle stops are not equal.
    /// </summary>
    /// <param name="left">The first mutable vehicle stop.</param>
    /// <param name="right">The second mutable vehicle stop.</param>
    /// <returns><see langword="true"/> if the mutable vehicle stops are not equal, <see langword="false"/> otherwise.</returns>
    public static bool operator !=(MutableVehicleStop? left, MutableVehicleStop? right)
    {
        return !Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether the first mutable vehicle stop is less than the second mutable vehicle stop.
    /// </summary>
    /// <param name="left">The first mutable vehicle stop.</param>
    /// <param name="right">The second mutable vehicle stop.</param>
    /// <returns><see langword="true"/> if the first mutable vehicle stop is less than the second mutable vehicle stop, <see langword="false"/> otherwise.</returns>
    public static bool operator <(MutableVehicleStop? left, MutableVehicleStop? right)
    {
        return Comparer<MutableVehicleStop>.Default.Compare(left, right) < 0;
    }
    
    /// <summary>
    /// Determines whether the first mutable vehicle stop is greater than the second mutable vehicle stop.
    /// </summary>
    /// <param name="left">The first mutable vehicle stop.</param>
    /// <param name="right">The second mutable vehicle stop.</param>
    /// <returns><see langword="true"/> if the first mutable vehicle stop is greater than the second mutable vehicle stop, <see langword="false"/> otherwise.</returns>
    public static bool operator >(MutableVehicleStop? left, MutableVehicleStop? right)
    {
        return Comparer<MutableVehicleStop>.Default.Compare(left, right) > 0;
    }
    
    /// <summary>
    /// Determines whether the first mutable vehicle stop is less than or equal to the second mutable vehicle stop.
    /// </summary>
    /// <param name="left">The first mutable vehicle stop.</param>
    /// <param name="right">The second mutable vehicle stop.</param>
    /// <returns><see langword="true"/> if the first mutable vehicle stop is less than or equal to the second mutable vehicle stop, <see langword="false"/> otherwise.</returns>
    public static bool operator <=(MutableVehicleStop? left, MutableVehicleStop? right)
    {
        return Comparer<MutableVehicleStop>.Default.Compare(left, right) <= 0;
    }
    
    /// <summary>
    /// Determines whether the first mutable vehicle stop is greater than or equal to the second mutable vehicle stop.
    /// </summary>
    /// <param name="left">The first mutable vehicle stop.</param>
    /// <param name="right">The second mutable vehicle stop.</param>
    /// <returns><see langword="true"/> if the first mutable vehicle stop is greater than or equal to the second mutable vehicle stop, <see langword="false"/> otherwise.</returns>
    public static bool operator >=(MutableVehicleStop? left, MutableVehicleStop? right)
    {
        return Comparer<MutableVehicleStop>.Default.Compare(left, right) >= 0;
    }
}