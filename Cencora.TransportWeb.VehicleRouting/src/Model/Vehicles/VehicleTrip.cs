// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Represents a vehicle trip.
/// </summary>
public sealed class VehicleTrip : IEquatable<VehicleTrip>, IComparable<VehicleTrip>
{
    /// <summary>
    /// Gets the index of the vehicle trip.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets the vehicle of the vehicle trip.
    /// </summary>
    public Vehicle Vehicle { get; }

    /// <summary>
    /// Gets the start location of the vehicle trip.
    /// </summary>
    public Location StartLocation { get; }

    /// <summary>
    /// Gets the end location of the vehicle trip.
    /// </summary>
    public Location EndLocation { get; }

    /// <summary>
    /// Gets the distance of the vehicle trip.
    /// </summary>
    public long Distance { get; }

    /// <summary>
    /// Gets the duration of the vehicle trip.
    /// </summary>
    public long Duration { get; }

    /// <summary>
    /// Gets the cost of the vehicle trip based on the distance.
    /// </summary>
    public long TripDistanceCost { get; }

    /// <summary>
    /// Gets the cost of the vehicle trip based on the duration.
    /// </summary>
    public long TripDurationCost { get; }

    /// <summary>
    /// Gets the total cost of the vehicle trip.
    /// </summary>
    public long TripCost => TripDistanceCost + TripDurationCost;

    /// <summary>
    /// Gets the departure time window of the vehicle trip.
    /// </summary>
    public ValueRange DepartureTimeWindow { get; }

    /// <summary>
    /// Gets the arrival time window of the vehicle trip.
    /// </summary>
    public ValueRange ArrivalTimeWindow { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleTrip"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle trip.</param>
    /// <param name="vehicle">The vehicle of the vehicle trip.</param>
    /// <param name="startLocation">The start location of the vehicle trip.</param>
    /// <param name="endLocation">The end location of the vehicle trip.</param>
    /// <param name="distance">The distance of the vehicle trip.</param>
    /// <param name="duration">The duration of the vehicle trip.</param>
    /// <param name="departureTimeWindow">The departure time window of the vehicle trip.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the vehicle trip.</param>
    /// <param name="tripDistanceCost">The cost of the vehicle trip based on the distance.</param>
    /// <param name="tripDurationCost">The cost of the vehicle trip based on the duration.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/>, <paramref name="startLocation"/> or <paramref name="endLocation"/> is null.</exception>
    public VehicleTrip(int index, Vehicle vehicle, Location startLocation, Location endLocation, long distance, long duration, ValueRange departureTimeWindow, ValueRange arrivalTimeWindow, long tripDistanceCost, long tripDurationCost)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(startLocation, nameof(startLocation));
        ArgumentNullException.ThrowIfNull(endLocation, nameof(endLocation));

        Index = index;
        Vehicle = vehicle;
        StartLocation = startLocation;
        EndLocation = endLocation;
        Distance = distance;
        Duration = duration;
        DepartureTimeWindow = departureTimeWindow;
        ArrivalTimeWindow = arrivalTimeWindow;
        TripDistanceCost = tripDistanceCost;
        TripDurationCost = tripDurationCost;
    }

    /// <inheritdoc/>
    public bool Equals(VehicleTrip? other)
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
    public int CompareTo(VehicleTrip? other)
    {
        if (ReferenceEquals(null, other))
        {
            return 1;
        }
        
        return Index.CompareTo(other.Index);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="VehicleTrip"/> are equal>
    /// </summary>
    /// <param name="left">The first <see cref="VehicleTrip"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleTrip"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(VehicleTrip? left, VehicleTrip? right)
    {
        return Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="VehicleTrip"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleTrip"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleTrip"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(VehicleTrip? left, VehicleTrip? right)
    {
        return !Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="VehicleTrip"/> is less than another specified <see cref="VehicleTrip"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleTrip"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleTrip"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(VehicleTrip left, VehicleTrip right)
    {
        return left.CompareTo(right) < 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="VehicleTrip"/> is greater than another specified <see cref="VehicleTrip"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleTrip"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleTrip"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(VehicleTrip left, VehicleTrip right)
    {
        return left.CompareTo(right) > 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="VehicleTrip"/> is less than or equal to another specified <see cref="VehicleTrip"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleTrip"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleTrip"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(VehicleTrip left, VehicleTrip right)
    {
        return left.CompareTo(right) <= 0;
    }
    
    /// <summary>
    /// Determines whether one specified <see cref="VehicleTrip"/> is greater than or equal to another specified <see cref="VehicleTrip"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VehicleTrip"/> to compare.</param>
    /// <param name="right">The second <see cref="VehicleTrip"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(VehicleTrip left, VehicleTrip right)
    {
        return left.CompareTo(right) >= 0;
    }
    
    /// <summary>
    /// Converts a <see cref="VehicleTrip"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="trip">The vehicle trip to convert.</param>
    /// <returns>The index of the vehicle trip.</returns>
    public static implicit operator int (VehicleTrip trip)
    {
        return trip.Index;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Index} trip of {Vehicle} from {StartLocation} to {EndLocation} with distance {Distance} and duration {Duration}";
    }
}
