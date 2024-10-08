// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Represents a vehicle stop in the solver.
/// </summary>
/// <remarks>
/// This is used to later convert this into a <see cref="VehicleStop"/>,
/// which is immutable and thus can not be used directly by the solver.
/// </remarks>
internal sealed class SolverVehicleStop : IEquatable<SolverVehicleStop>, IComparable<SolverVehicleStop>
{
    /// <summary>
    /// Gets the index of the vehicle stop.
    /// </summary>
    internal int Index { get; }

    /// <summary>
    /// Gets the location of the vehicle stop.
    /// </summary>
    internal Location? Location;

    /// <summary>
    /// Gets the vehicle of the vehicle stop.
    /// </summary>
    internal Vehicle Vehicle;

    /// <summary>
    /// Gets the pickups of the vehicle stop.
    /// </summary>
    internal HashSet<Shipment> Pickups;

    /// <summary>
    /// Gets the deliveries of the vehicle stop.
    /// </summary>
    internal HashSet<Shipment> Deliveries;

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
    /// Initializes a new instance of the <see cref="SolverVehicleStop"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle stop.</param>
    /// <param name="location">The location of the vehicle stop.</param>
    /// <param name="vehicle">The vehicle of the vehicle stop.</param>
    /// <param name="pickups">The pickups of the vehicle stop.</param>
    /// <param name="deliveries">The deliveries of the vehicle stop.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the vehicle stop.</param>
    /// <param name="departureTimeWindow">The departure time window of the vehicle stop.</param>
    /// <param name="waitingTime">The waiting time of the vehicle stop.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or zero.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/>, <paramref name="pickups"/>, or <paramref name="deliveries"/> is <see langword="null"/>.</exception>
    internal SolverVehicleStop(int index, Location? location, Vehicle vehicle, HashSet<Shipment> pickups,
        HashSet<Shipment> deliveries, ValueRange arrivalTimeWindow, ValueRange departureTimeWindow,
        ValueRange waitingTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(index, nameof(index));
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

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverVehicleStop"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle stop.</param>
    /// <param name="location">The location of the vehicle stop.</param>
    /// <param name="vehicle">The vehicle of the vehicle stop.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the vehicle stop.</param>
    /// <param name="departureTimeWindow">The departure time window of the vehicle stop.</param>
    /// <param name="waitingTime">The waiting time of the vehicle stop.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or zero.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="location"/> or <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    internal SolverVehicleStop(int index, Location location, Vehicle vehicle, ValueRange arrivalTimeWindow, ValueRange departureTimeWindow,
        ValueRange waitingTime)
        : this (index, location, vehicle, [], [], arrivalTimeWindow, departureTimeWindow, waitingTime)
    {
    }
    
    /// <summary>
    /// Converts the <see cref="SolverVehicleStop"/> to a <see cref="VehicleStop"/>.
    /// </summary>
    /// <returns>The converted vehicle stop.</returns>
    internal VehicleStop ToVehicleStop()
    {
        return new VehicleStop(Index, Location, Vehicle, Pickups, Deliveries, ArrivalTimeWindow, DepartureTimeWindow, WaitingTime);
    }

    /// <inheritdoc/>
    public bool Equals(SolverVehicleStop? other)
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
    public int CompareTo(SolverVehicleStop? other)
    {
        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        return Index.CompareTo(other.Index);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is SolverVehicleStop other && Equals(other);
    }
    
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Index);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="SolverVehicleStop"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="SolverVehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverVehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(SolverVehicleStop? left, SolverVehicleStop? right)
    {
        return Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="SolverVehicleStop"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="SolverVehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverVehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(SolverVehicleStop? left, SolverVehicleStop? right)
    {
        return !Equals(left, right);
    }
    
    /// <summary>
    /// Converts a <see cref="SolverVehicleStop"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="stop">The vehicle stop to convert.</param>
    /// <returns>The index of the vehicle stop.</returns>
    public static implicit operator int(SolverVehicleStop stop)
    {
        return stop.Index;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="SolverVehicleStop"/> is less than another specified <see cref="SolverVehicleStop"/>.
    /// </summary>
    /// <param name="left">The first <see cref="SolverVehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverVehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(SolverVehicleStop left, SolverVehicleStop right)
    {
        return left.CompareTo(right) < 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="SolverVehicleStop"/> is greater than another specified <see cref="SolverVehicleStop"/>.
    /// </summary>
    /// <param name="left">The first <see cref="SolverVehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverVehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(SolverVehicleStop left, SolverVehicleStop right)
    {
        return left.CompareTo(right) > 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="SolverVehicleStop"/> is less than or equal to another specified <see cref="SolverVehicleStop"/>.
    /// </summary>
    /// <param name="left">The first <see cref="SolverVehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverVehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(SolverVehicleStop left, SolverVehicleStop right)
    {
        return left.CompareTo(right) <= 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="SolverVehicleStop"/> is greater than or equal to another specified <see cref="SolverVehicleStop"/>.
    /// </summary>
    /// <param name="left">The first <see cref="SolverVehicleStop"/> to compare.</param>
    /// <param name="right">The second <see cref="SolverVehicleStop"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(SolverVehicleStop left, SolverVehicleStop right)
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