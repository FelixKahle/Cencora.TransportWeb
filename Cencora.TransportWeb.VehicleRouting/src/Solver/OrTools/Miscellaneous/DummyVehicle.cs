// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Flags;
using Cencora.TransportWeb.Common.Id;
using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Miscellaneous;

/// <summary>
/// Represents a dummy vehicle.
/// </summary>
internal sealed class DummyVehicle : IEquatable<DummyVehicle>
{
    /// <summary>
    /// The index of the dummy vehicle in the solver vehicle list.
    /// </summary>
    internal int Index { get; }

    /// <summary>
    /// The vehicle.
    /// </summary>
    internal Vehicle Vehicle { get; }

    /// <summary>
    /// The shift this dummy vehicle represents.
    /// </summary>
    internal Shift Shift { get; }

    /// <summary>
    /// Returns the Id of the vehicle.
    /// </summary>
    internal Id Id => Vehicle.Id;

    /// <summary>
    /// Gets the time window in which the vehicle is available.
    /// </summary>
    internal ValueRange AvailableTimeWindow => Shift.ShiftTimeWindow;
    
    /// <summary>
    /// Gets the breaks for the vehicle.
    /// </summary>
    internal IReadOnlySet<Break> Breaks => Shift.Breaks;

    /// <summary>
    /// Gets the fixed cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The fixed cost is a fixed amount of cost that is applied
    /// always, no matter how the vehicle is used.
    /// </remarks>
    internal long FixedCost { get; }

    /// <summary>
    /// Gets the base cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The base cost is a fixed amount of cost that is applied
    /// as soon as the vehicle is used.
    /// </remarks>
    internal long BaseCost { get; }

    /// <summary>
    /// Gets the distance cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The distance cost is applied per distance unit.
    /// </remarks>
    internal long DistanceCost { get; }

    /// <summary>
    /// Gets the time cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The time cost is applied per time unit.
    /// </remarks>
    internal long TimeCost { get; }

    /// <summary>
    /// Gets the weight cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The weight cost is applied per weight unit.
    /// </remarks>
    internal long WeightCost { get; }

    /// <summary>
    /// Gets the weight per distance cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The weight per distance cost is applied per weight multiplied by distance unit.
    /// </remarks>
    internal long CostPerWeightDistance { get; }

    /// <summary>
    /// Gets the maximum weight the vehicle can carry.
    /// </summary>
    internal long MaxWeight { get; }
    
    /// <summary>
    /// Gets the maximum total weight the vehicle can carry.
    /// </summary>
    /// <remarks>
    /// The maximum total weight is the sum of all weights a vehicle is allowed to carry.
    /// </remarks>
    internal long MaxTotalWeight { get; }

    /// <summary>
    /// Gets the maximum duration the vehicle can drive.
    /// </summary>
    internal long MaxDuration { get; }

    /// <summary>
    /// Gets the maximum distance the vehicle can drive.
    /// </summary>
    internal long MaxDistance { get; }

    /// <summary>
    /// The flags of the vehicle.
    /// </summary>
    internal IReadOnlyFlagContainer Flags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyVehicle"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="shift">The shift this dummy vehicle represents.</param>
    /// <param name="fixedCost">The fixed cost of the vehicle.</param>
    /// <param name="baseCost">The base cost of the vehicle.</param>
    /// <param name="distanceCost">The distance cost of the vehicle.</param>
    /// <param name="timeCost">The time cost of the vehicle.</param>
    /// <param name="weightCost">The weight cost of the vehicle.</param>
    /// <param name="costPerWeightDistance">The weight per distance cost of the vehicle.</param>
    /// <param name="maxWeight">The maximum weight the vehicle can carry.</param>
    /// <param name="maxTotalWeight">The maximum total weight the vehicle can carry.</param>
    /// <param name="maxDuration">The maximum duration the vehicle can drive.</param>
    /// <param name="maxDistance">The maximum distance the vehicle can drive.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> or <paramref name="shift"/> is <see langword="null"/>.</exception>
    internal DummyVehicle(int index, Vehicle vehicle, Shift shift, long fixedCost, long baseCost, long distanceCost, long timeCost, long weightCost, long costPerWeightDistance, long maxWeight, long maxTotalWeight, long maxDuration, long maxDistance)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(shift, nameof(shift));

        Index = index;
        Vehicle = vehicle;
        Shift = shift;
        FixedCost = fixedCost;
        BaseCost = baseCost;
        DistanceCost = distanceCost;
        TimeCost = timeCost;
        WeightCost = weightCost;
        CostPerWeightDistance = costPerWeightDistance;
        MaxWeight = Math.Max(0, maxWeight);
        MaxTotalWeight = Math.Max(0, maxTotalWeight);
        MaxDuration = Math.Max(0, maxDuration);
        MaxDistance = Math.Max(0, maxDistance);
        Flags = CreateFlagContainerFromGroups(vehicle.Flags, shift.Flags);
    }

    /// <inheritdoc />
    public bool Equals(DummyVehicle? other)
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
    public override bool Equals(object? obj)
    {
        return obj is DummyVehicle other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Index;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"DummyVehicle: {Vehicle.Id} ({Index})";
    }

    /// <summary>
    /// Converts a <see cref="DummyVehicle"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="dummyVehicle">The <see cref="DummyVehicle"/> to convert.</param>
    /// <returns>The index of the <see cref="DummyVehicle"/>.</returns>
    public static implicit operator int(DummyVehicle dummyVehicle)
    {
        return dummyVehicle.Index;
    }

    /// <summary>
    /// Converts a <see cref="DummyVehicle"/> to a <see cref="Vehicle"/>.
    /// </summary>
    /// <param name="dummyVehicle">The <see cref="DummyVehicle"/> to convert.</param>
    /// <returns>The <see cref="Vehicle"/> of the <see cref="DummyVehicle"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="dummyVehicle"/> parameter is null.</exception>
    public static implicit operator Vehicle(DummyVehicle dummyVehicle)
    {
        ArgumentNullException.ThrowIfNull(dummyVehicle, nameof(dummyVehicle));
        
        return dummyVehicle.Vehicle;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="DummyVehicle"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="DummyVehicle"/> to compare.</param>
    /// <param name="right">The second <see cref="DummyVehicle"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> represent the same <see cref="DummyVehicle"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(DummyVehicle? left, DummyVehicle? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="DummyVehicle"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="DummyVehicle"/> to compare.</param>
    /// <param name="right">The second <see cref="DummyVehicle"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> do not represent the same <see cref="DummyVehicle"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(DummyVehicle? left, DummyVehicle? right)
    {
        return !(left == right);
    }
    
    /// <summary>
    /// Creates and initializes a <see cref="FlagContainer"/> from one or more collections of flags.
    /// </summary>
    /// <param name="flags">A variable number of collections containing <see cref="Flag"/> objects.</param>
    /// <returns>A <see cref="FlagContainer"/> that contains all the flags from the provided collections.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="flags"/> parameter is null.</exception>
    /// <remarks>
    /// This method initializes a new <see cref="FlagContainer"/> by summing the number of flags in all the provided collections
    /// and preallocating the necessary space for the flags in the container. It then adds each collection of flags to the container.
    /// </remarks>
    private static FlagContainer CreateFlagContainerFromGroups(params IEnumerable<Flag>[] flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        var totalFlagCount = flags.Sum(flagGroup => flagGroup.Count());
        var flagContainer = new FlagContainer(totalFlagCount);
        foreach (var flagGroup in flags)
        {
            flagContainer.AddFlags(flagGroup);
        }

        return flagContainer;
    }
}
