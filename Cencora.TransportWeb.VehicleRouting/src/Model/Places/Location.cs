// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Flags;
using Cencora.TransportWeb.Common.Id;

namespace Cencora.TransportWeb.VehicleRouting.Model.Places;

/// <summary>
/// Represents a location.
/// </summary>
public sealed class Location : IEquatable<Location>, IComparable<Location>
{
    /// <summary>
    /// The identifier of the location.
    /// </summary>
    public Id Id { get; }

    /// <summary>
    /// The maximal vehicle capacity of the location.
    /// </summary>
    public long? MaximalVehicleCapacity { get; }

    /// <summary>
    /// The flags of the location.
    /// </summary>
    public IReadOnlyFlagContainer Flags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> class.
    /// </summary>
    /// <param name="id">The identifier of the location.</param>
    public Location(Id id)
    {
        Id = id;
        Flags = new FlagContainer();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> class.
    /// </summary>
    /// <param name="id">The identifier of the location.</param>
    /// <param name="maximalVehicleCapacity">The maximal vehicle capacity of the location.</param>
    public Location(Id id, long? maximalVehicleCapacity)
    {
        Id = id;
        MaximalVehicleCapacity = maximalVehicleCapacity;
        Flags = new FlagContainer();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> class.
    /// </summary>
    /// <param name="id">The identifier of the location.</param>
    /// <param name="flags">The flags of the location.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="flags"/> is <see langword="null"/>.</exception>
    public Location(Id id, IReadOnlyFlagContainer flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        Id = id;
        Flags = flags;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> class.
    /// </summary>
    /// <param name="id">The identifier of the location.</param>
    /// <param name="maximalVehicleCapacity">The maximal vehicle capacity of the location.</param>
    /// <param name="flags">The flags of the location.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="flags"/> is <see langword="null"/>.</exception>
    public Location(Id id, long? maximalVehicleCapacity, IReadOnlyFlagContainer flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        Id = id;
        Flags = flags;
        MaximalVehicleCapacity = maximalVehicleCapacity;
    }

    /// <inheritdoc/>
    public bool Equals(Location? other)
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
        return obj is Location other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Id;
    }

    /// <inheritdoc/>
    public int CompareTo(Location? other)
    {
        // If a location has no maximal vehicle capacity, it is considered to have an infinite maximal vehicle capacity.
        // As we cannot compare infinite values, we need to adjust the maximal vehicle
        // capacity to the maximum value of the data type.
        // This way, we can compare the maximal vehicle capacities of the locations.
        var adjustedMaximalVehicleCapacity = MaximalVehicleCapacity ?? long.MaxValue;
        var otherAdjustedMaximalVehicleCapacity = other?.MaximalVehicleCapacity ?? long.MaxValue;

        return adjustedMaximalVehicleCapacity.CompareTo(otherAdjustedMaximalVehicleCapacity);
    }

    /// <summary>
    /// Checks if the location has a maximal vehicle capacity.
    /// </summary>
    /// <returns><see langword="true"/> if the location has a maximal vehicle capacity; otherwise, <see langword="false"/>.</returns>
    public bool HasMaximalVehicleCapacity => MaximalVehicleCapacity.HasValue;

    /// <summary>
    /// Converts a <see cref="Location"/> to a <see cref="Id"/>.
    /// </summary>
    /// <param name="location">The <see cref="Location"/> to convert.</param>
    /// <returns>The <see cref="Id"/> of the <see cref="Location"/>.</returns>
    public static explicit operator Id(Location location)
    {
        return location.Id;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Location"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Location"/> to compare.</param>
    /// <param name="right">The second <see cref="Location"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Location? left, Location? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Location"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Location"/> to compare.</param>
    /// <param name="right">The second <see cref="Location"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Location? left, Location? right)
    {
        return !(left == right);
    }
    
    /// <summary>
    /// Determines whether the first specified instance of <see cref="Location"/> is less than the second specified instance.
    /// </summary>
    /// <param name="left">The first <see cref="Location"/> to compare.</param>
    /// <param name="right">The second <see cref="Location"/> to compare.</param>
    /// <returns><see langword="true"/> if the first instance is less than the second instance; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Location? left, Location? right)
    {
        return Comparer<Location>.Default.Compare(left, right) < 0;
    }
    
    /// <summary>
    /// Determines whether the first specified instance of <see cref="Location"/> is greater than the second specified instance.
    /// </summary>
    /// <param name="left">The first <see cref="Location"/> to compare.</param>
    /// <param name="right">The second <see cref="Location"/> to compare.</param>
    /// <returns><see langword="true"/> if the first instance is greater than the second instance; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Location? left, Location? right)
    {
        return Comparer<Location>.Default.Compare(left, right) > 0;
    }
    
    /// <summary>
    /// Determines whether the first specified instance of <see cref="Location"/> is less than or equal to the second specified instance.
    /// </summary>
    /// <param name="left">The first <see cref="Location"/> to compare.</param>
    /// <param name="right">The second <see cref="Location"/> to compare.</param>
    /// <returns><see langword="true"/> if the first instance is less than or equal to the second instance; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Location? left, Location? right)
    {
        return Comparer<Location>.Default.Compare(left, right) <= 0;
    }
    
    /// <summary>
    /// Determines whether the first specified instance of <see cref="Location"/> is greater than or equal to the second specified instance.
    /// </summary>
    /// <param name="left">The first <see cref="Location"/> to compare.</param>
    /// <param name="right">The second <see cref="Location"/> to compare.</param>
    /// <returns><see langword="true"/> if the first instance is greater than or equal to the second instance; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Location? left, Location? right)
    {
        return Comparer<Location>.Default.Compare(left, right) >= 0;
    }
}
