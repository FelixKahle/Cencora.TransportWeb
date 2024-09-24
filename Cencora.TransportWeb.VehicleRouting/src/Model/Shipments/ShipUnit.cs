// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;

namespace Cencora.TransportWeb.VehicleRouting.Model.Shipments;

/// <summary>
/// Represents a ship unit.
/// </summary>
public readonly struct ShipUnit : IEquatable<ShipUnit>
{
    /// <summary>
    /// The id of the ship unit.
    /// </summary>
    public Id Id { get; }

    /// <summary>
    /// The weight of the ship unit.
    /// </summary>
    public long? Weight { get; }

    /// <summary>
    /// The width of the ship unit.
    /// </summary>
    public long? Width { get; }

    /// <summary>
    /// The height of the ship unit.
    /// </summary>
    public long? Height { get; }

    /// <summary>
    /// The length of the ship unit.
    /// </summary>
    public long? Length { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShipUnit"/>.
    /// </summary>
    public ShipUnit()
    {
        Id = Id.New();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShipUnit"/>.
    /// </summary>
    /// <param name="id">The id of the ship unit.</param>
    public ShipUnit(Id id)
    {
        this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShipUnit"/>.
    /// </summary>
    /// <param name="id">The id of the ship unit.</param>
    /// <param name="weight">The weight of the ship unit.</param>
    /// <param name="width">The width of the ship unit.</param>
    /// <param name="height">The height of the ship unit.</param>
    /// <param name="length">The length of the ship unit.</param>
    public ShipUnit(Id id, long? weight, long? width, long? height, long? length)
    {
        this.Id = id;
        Weight = weight;
        Width = width;
        Height = height;
        Length = length;
    }

    /// <inheritdoc/>
    public bool Equals(ShipUnit other)
    {
        return Id.Equals(other.Id);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ShipUnit other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"ShipUnit: {Id}";
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="ShipUnit"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="ShipUnit"/> to compare.</param>
    /// <param name="right">The second <see cref="ShipUnit"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ShipUnit left, ShipUnit right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="ShipUnit"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="ShipUnit"/> to compare.</param>
    /// <param name="right">The second <see cref="ShipUnit"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ShipUnit left, ShipUnit right)
    {
        return !left.Equals(right);
    }
}
