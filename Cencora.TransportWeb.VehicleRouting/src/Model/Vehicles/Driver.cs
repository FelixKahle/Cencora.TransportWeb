// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Flags;
using Cencora.TransportWeb.Common.Id;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Represents a driver.
/// </summary>
public readonly struct Driver : IEquatable<Driver>
{
    /// <summary>
    /// Gets the id of the driver.
    /// </summary>
    public Id Id { get; }

    /// <summary>
    /// Gets the fixed cost of the driver.
    /// </summary>
    public long? FixedCost { get; }

    /// <summary>
    /// Gets the flags of the driver.
    /// </summary>
    public IReadOnlyFlagContainer Flags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Driver"/> struct.
    /// </summary>
    /// <param name="id">The id of the driver.</param>
    public Driver(Id id)
    {
        Id = id;
        FixedCost = null;
        Flags = new FlagContainer();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Driver"/> struct.
    /// </summary>
    /// <param name="id">The id of the driver.</param>
    /// <param name="fixedCost">The fixed cost of the driver.</param>
    public Driver(Id id, long? fixedCost)
    {
        Id = id;
        FixedCost = fixedCost;
        Flags = new FlagContainer();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Driver"/> struct.
    /// </summary>
    /// <param name="id">The id of the driver.</param>
    /// <param name="flags">The flags of the driver.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="flags"/> is <see langword="null"/>.</exception>
    public Driver(Id id, IReadOnlyFlagContainer flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        Id = id;
        FixedCost = null;
        Flags = flags;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Driver"/> struct.
    /// </summary>
    /// <param name="id">The id of the driver.</param>
    /// <param name="fixedCost">The fixed cost of the driver.</param>
    /// <param name="flags">The flags of the driver.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="flags"/> is <see langword="null"/>.</exception>
    public Driver(Id id, long? fixedCost, IReadOnlyFlagContainer flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        Id = id;
        FixedCost = fixedCost;
        Flags = flags;
    }

    /// <inheritdoc/>
    public bool Equals(Driver other)
    {
        return Id.Equals(other.Id);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Driver other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Driver: {Id}";
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Driver"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Driver"/> to compare.</param>
    /// <param name="right">The second <see cref="Driver"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Driver left, Driver right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Driver"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Driver"/> to compare.</param>
    /// <param name="right">The second <see cref="Driver"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Driver left, Driver right)
    {
        return !left.Equals(right);
    }
}
