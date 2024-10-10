// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Represents a break that can be taken during a route.
/// </summary>
public readonly struct Break : IEquatable<Break>, IComparable<Break>
{
    /// <summary>
    /// This range represents the time window in which the break is allowed to be taken.
    /// </summary>
    public ValueRange AllowedTimeWindow { get; }

    /// <summary>
    /// The duration of the break.
    /// </summary>
    public long Duration { get; }

    /// <summary>
    /// The location of the break.
    /// </summary>
    public Location? Location { get; }

    /// <summary>
    /// The option of the break.
    /// </summary>
    public BreakOption Option { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Break"/> struct.
    /// </summary>
    /// <param name="allowedTimeWindow">The time window in which the break is allowed to be taken.</param>
    /// <param name="duration">The duration of the break.</param>
    /// <param name="option">The option of the break.</param>
    /// <param name="location">The location of the break.</param>
    /// <remarks>
    /// The duration of the break is adjusted so that it cannot exceed the allowed time window.
    /// </remarks>
    public Break(ValueRange allowedTimeWindow, long duration, BreakOption option, Location? location = null)
    {
        // Adjust the duration so that it cannot exceed the allowed time window.
        // Furthermore, ensure that the duration is not negative.
        var adjustedDuration = Math.Min(Math.Max(0, duration), allowedTimeWindow.Length);

        AllowedTimeWindow = allowedTimeWindow;
        Duration = adjustedDuration;
        Option = option;
        Location = location;
    }

    /// <inheritdoc/>
    public bool Equals(Break other)
    {
        return AllowedTimeWindow.Equals(other.AllowedTimeWindow) && Duration == other.Duration && Option == other.Option;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Break other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(AllowedTimeWindow, Duration, Option);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Break: {Option}, {AllowedTimeWindow}, {Duration}";
    }

    /// <inheritdoc/>
    public int CompareTo(Break other)
    {
        return Duration.CompareTo(other.Duration);
    }

    /// <summary>
    /// Converts a break to its duration.
    /// </summary>
    /// <param name="breakObject">The break to convert.</param>
    /// <returns>The duration of the break.</returns>
    public static explicit operator long(Break breakObject)
    {
        return breakObject.Duration;
    }

    /// <summary>
    /// Determines whether two specified breaks have the same value.
    /// </summary>
    /// <param name="left">The first break to compare.</param>
    /// <param name="right">The second break to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Break left, Break right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified breaks have different values.
    /// </summary>
    /// <param name="left">The first break to compare.</param>
    /// <param name="right">The second break to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="left"/> is different from the value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Break left, Break right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Determines whether one break is less than another break.
    /// </summary>
    /// <param name="left">The first break to compare.</param>
    /// <param name="right">The second break to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Break left, Break right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Determines whether one break is greater than another break.
    /// </summary>
    /// <param name="left">The first break to compare.</param>
    /// <param name="right">The second break to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Break left, Break right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Determines whether one break is less than or equal to another break.
    /// </summary>
    /// <param name="left">The first break to compare.</param>
    /// <param name="right">The second break to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Break left, Break right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Determines whether one break is greater than or equal to another break.
    /// </summary>
    /// <param name="left">The first break to compare.</param>
    /// <param name="right">The second break to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Break left, Break right)
    {
        return left.CompareTo(right) >= 0;
    }
}
