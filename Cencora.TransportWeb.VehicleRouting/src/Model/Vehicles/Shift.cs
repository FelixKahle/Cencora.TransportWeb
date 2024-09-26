// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Flags;
using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Represents a shift.
/// </summary>
public sealed class Shift : IEquatable<Shift>
{
    /// <summary>
    /// Gets the time window of the shift.
    /// </summary>
    public ValueRange ShiftTimeWindow { get; }

    /// <summary>
    /// Gets the breaks for the shift.
    /// </summary>
    public IReadOnlySet<Break> Breaks { get; }

    /// <summary>
    /// Gets the start location of the shift.
    /// </summary>
    public Location? StartLocation { get; }

    /// <summary>
    /// Gets the end location of the shift.
    /// </summary>
    public Location? EndLocation { get; }

    /// <summary>
    /// Gets the fixed cost of the shift.
    /// </summary>
    public long? FixedCost { get; }

    /// <summary>
    /// Gets the base cost of the shift.
    /// </summary>
    public long? BaseCost { get; }

    /// <summary>
    /// Gets the time cost of the shift.
    /// </summary>
    public long? TimeCost { get; }

    /// <summary>
    /// Gets the distance cost of the shift.
    /// </summary>
    public long? DistanceCost { get; }

    /// <summary>
    /// Gets the maximum duration of the shift.
    /// </summary>
    public long? MaxDuration { get; }

    /// <summary>
    /// Gets the maximum distance of the shift.
    /// </summary>
    public long? MaxDistance { get; }

    /// <summary>
    /// Gets the flags of the shift.
    /// </summary>
    public IReadOnlyFlagContainer Flags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Shift"/> class.
    /// </summary>
    /// <param name="shiftTimeWindow">The time window of the shift.</param>
    /// <param name="breaks">The breaks for the shift.</param>
    /// <param name="startLocation">The start location of the shift.</param>
    /// <param name="endLocation">The end location of the shift.</param>
    /// <param name="fixedCost">The fixed cost of the shift.</param>
    /// <param name="baseCost">The base cost of the shift.</param>
    /// <param name="timeCost">The time cost of the shift.</param>
    /// <param name="distanceCost">The distance cost of the shift.</param>
    /// <param name="maxDuration">The maximum duration of the shift.</param>
    /// <param name="maxDistance">The maximum distance of the shift.</param>
    /// <param name="flags">The flags of the shift.</param>
    public Shift(ValueRange shiftTimeWindow, IEnumerable<Break>? breaks, Location? startLocation,
        Location? endLocation, long? fixedCost, long? baseCost, long? timeCost, long? distanceCost, long? maxDuration,
        long? maxDistance, IReadOnlyFlagContainer? flags)
    {
        ShiftTimeWindow = shiftTimeWindow;
        Breaks = InitializeBreaks(shiftTimeWindow, breaks ?? Enumerable.Empty<Break>());
        StartLocation = startLocation;
        EndLocation = endLocation;
        FixedCost = fixedCost;
        BaseCost = baseCost;
        TimeCost = timeCost;
        DistanceCost = distanceCost;
        MaxDuration = maxDuration;
        MaxDistance = maxDistance;
        Flags = flags ?? new FlagContainer();
    }

    /// <inheritdoc/>
    public bool Equals(Shift? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ShiftTimeWindow.Equals(other.ShiftTimeWindow) && Breaks.SetEquals(other.Breaks);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Shift other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(ShiftTimeWindow, Breaks);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Shift: {ShiftTimeWindow}";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Shift"/> class.
    /// </summary>
    /// <param name="shiftTimeWindow">The time window of the shift.</param>
    /// <param name="breaks">The breaks for the shift.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="breaks"/> is <see langword="null"/>.</exception>
    private static IReadOnlySet<Break> InitializeBreaks(ValueRange shiftTimeWindow, IEnumerable<Break> breaks)
    {
        ArgumentNullException.ThrowIfNull(breaks, nameof(breaks));

        // Little optimization: If the breaks are a collection, we can preallocate the hash set with the correct capacity.
        // Do not use Count() on IEnumerable, because it will iterate over the whole collection.
        var breakSet = breaks is ICollection<Break> collection ? new HashSet<Break>(collection.Count) : new HashSet<Break>();
        foreach (var currentBreak in breaks)
        {
            // If the break is not in the time window of the shift, skip it,
            // because it cannot be taken during the shift and is therefore irrelevant.
            if (shiftTimeWindow.Contains(currentBreak.AllowedTimeWindow) == false)
            {
                continue;
            }

            breakSet.Add(currentBreak);
        }

        return breakSet;
    }

    /// <summary>
    /// Determines whether two specified shifts have the same value.
    /// </summary>
    /// <param name="left">The first shift to compare, or <see langword="null"/>.</param>
    /// <param name="right">The second shift to compare, or <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Shift? left, Shift? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified shifts have different values.
    /// </summary>
    /// <param name="left">The first shift to compare, or <see langword="null"/>.</param>
    /// <param name="right">The second shift to compare, or <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="left"/> is different from the value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Shift? left, Shift? right)
    {
        return !Equals(left, right);
    }
}
