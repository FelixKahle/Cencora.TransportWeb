// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Represents a stop of a vehicle.
/// </summary>
public sealed class VehicleStop : IEquatable<VehicleStop>, IComparable<VehicleStop>
{
    /// <summary>
    /// Gets the index of the vehicle stop.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets the location of the vehicle stop.
    /// </summary>
    public Location? Location { get; }

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
    public ValueRange WaitingTime { get; }

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
    public ValueRange TotalStopTime => TotalHandlingTime + WaitingTime;
    
    /// <summary>
    /// Creates a new instance of the <see cref="VehicleStop"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle stop.</param>
    /// <param name="location">The location of the vehicle stop.</param>
    /// <param name="vehicle">The vehicle of the vehicle stop.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the vehicle stop.</param>
    /// <param name="departureTimeWindow">The departure time window of the vehicle stop.</param>
    /// <param name="waitingTime">The waiting time of the vehicle stop.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is null.</exception>
    public VehicleStop(int index, Location? location, Vehicle vehicle, ValueRange arrivalTimeWindow, ValueRange departureTimeWindow, ValueRange waitingTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
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
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/>, <paramref name="pickups"/> or <paramref name="deliveries"/> is null.</exception>
    public VehicleStop(int index, Location? location, Vehicle vehicle, IReadOnlySet<Shipment> pickups, IReadOnlySet<Shipment> deliveries, ValueRange arrivalTimeWindow, ValueRange departureTimeWindow, ValueRange waitingTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
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

    /// <inheritdoc/>
    public bool Equals(VehicleStop? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Index.Equals(other.Index) && Vehicle.Equals(other.Vehicle);
    }
    
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is VehicleStop other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Index, Vehicle);
    }

    /// <inheritdoc/>
    public int CompareTo(VehicleStop? other)
    {
        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        return Index.CompareTo(other.Index);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="VehicleStop"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(VehicleStop? left, VehicleStop? right)
    {
        return Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="VehicleStop"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(VehicleStop? left, VehicleStop? right)
    {
        return !Equals(left, right);
    }
    
    /// <summary>
    /// Converts a <see cref="VehicleStop"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="stop">The vehicle stop to convert.</param>
    /// <returns>The index of the vehicle stop.</returns>
    public static implicit operator int(VehicleStop stop)
    {
        return stop.Index;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="VehicleStop"/> is less than another specified <see cref="VehicleStop"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(VehicleStop left, VehicleStop right)
    {
        return left.CompareTo(right) < 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="VehicleStop"/> is greater than another specified <see cref="VehicleStop"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(VehicleStop left, VehicleStop right)
    {
        return left.CompareTo(right) > 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="VehicleStop"/> is less than or equal to another specified <see cref="VehicleStop"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(VehicleStop left, VehicleStop right)
    {
        return left.CompareTo(right) <= 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="VehicleStop"/> is greater than or equal to another specified <see cref="VehicleStop"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(VehicleStop left, VehicleStop right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var location = Location is null ? "unknown location" : Location.Id.ToString();
        
        return $"Stop {Index} of vehicle {Vehicle.Id} at {location}";
    }
}
