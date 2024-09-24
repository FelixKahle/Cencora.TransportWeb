// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Flags;
using Cencora.TransportWeb.Common.Id;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicle;

/// <summary>
/// A vehicle.
/// </summary>
public sealed class Vehicle : IEquatable<Vehicle>
{
    /// <summary>
    /// Gets the id of the vehicle.
    /// </summary>
    public Id Id { get; }

    /// <summary>
    /// Gets the shifts of the vehicle.
    /// </summary>
    public IReadOnlySet<Shift> Shifts { get; }

    /// <summary>
    /// Gets the flags of the vehicle.
    /// </summary>
    public IReadOnlyFlagContainer Flags { get; }

    /// <summary>
    /// Gets the fixed cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The fixed cost is a fixed amount of cost that is applied
    /// always, no matter how the vehicle is used.
    /// </remarks>
    public long? FixedCost { get; }

    /// <summary>
    /// Gets the base cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The base cost is a fixed amount of cost that is applied
    /// as soon as the vehicle is used.
    /// </remarks>
    public long? BaseCost { get; }

    /// <summary>
    /// Gets the distance cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The distance cost is applied per distance unit.
    /// </remarks>
    public long? DistanceCost { get; }

    /// <summary>
    /// Gets the time cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The time cost is applied per time unit.
    /// </remarks>
    public long? TimeCost { get; }

    /// <summary>
    /// Gets the weight cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The weight cost is applied per weight unit.
    /// </remarks>
    public long? WeightCost { get; }

    /// <summary>
    /// Gets the weight per distance cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The weight per distance cost is applied per weight multiplied by distance unit.
    /// </remarks>
    public long? CostPerWeightDistance { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vehicle"/> class.
    /// </summary>
    /// <param name="id">The id of the vehicle.</param>
    public Vehicle(Id id)
    {
        Id = id;
        Shifts = new HashSet<Shift>();
        Flags = new FlagContainer();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vehicle"/> class.
    /// </summary>
    /// <param name="id">The id of the vehicle.</param>
    /// <param name="shifts">The shifts of the vehicle.</param>
    /// <param name="fixedCost">The fixed cost of the vehicle.</param>
    /// <param name="baseCost">The base cost of the vehicle.</param>
    /// <param name="distanceCost">The distance cost of the vehicle.</param>
    /// <param name="timeCost">The time cost of the vehicle.</param>
    /// <param name="weightCost">The weight cost of the vehicle.</param>
    /// <param name="costPerWeightDistance">The weight per distance cost of the vehicle.</param>
    /// <param name="flags">The flags of the vehicle.</param>
    public Vehicle(Id id, IReadOnlySet<Shift>? shifts, long? fixedCost, long? baseCost, long? distanceCost, long? timeCost, long? weightCost, long? costPerWeightDistance, IReadOnlyFlagContainer? flags)
    {
        Id = id;
        Shifts = shifts ?? new HashSet<Shift>();
        Flags = flags ?? new FlagContainer();
        FixedCost = fixedCost;
        BaseCost = baseCost;
        DistanceCost = distanceCost;
        TimeCost = timeCost;
        WeightCost = weightCost;
        CostPerWeightDistance = costPerWeightDistance;
    }

    /// <inheritdoc/>
    public bool Equals(Vehicle? other)
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
        return obj is Vehicle other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Vehicle: {Id}";
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Vehicle"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Vehicle"/> to compare.</param>
    /// <param name="right">The second <see cref="Vehicle"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Vehicle? left, Vehicle? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Vehicle"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Vehicle"/> to compare.</param>
    /// <param name="right">The second <see cref="Vehicle"/> to compare.</param>
    /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Vehicle? left, Vehicle? right)
    {
        return !Equals(left, right);
    }
}
