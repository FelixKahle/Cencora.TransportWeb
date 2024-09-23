// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Common;

/// <summary>
/// Represents a value range.
/// </summary>
public readonly struct ValueRange : IEquatable<ValueRange>, IFormattable, IComparable<ValueRange>
{
    /// <summary>
    /// The minimum value of the range.
    /// </summary>
    public long Min { get; }

    /// <summary>
    /// The maximum value of the range.
    /// </summary>
    public long Max { get; }

    /// <summary>
    /// The difference between the minimum and maximum value.
    /// </summary>
    public long Difference => Max - Min;

    /// <summary>
    /// The length of the range.
    /// </summary>
    /// <remarks>
    /// The length of the range is the absolute value of the difference between the minimum and maximum value.
    /// </remarks>
    public long Length => Math.Abs(Difference);

    /// <summary>
    /// Determines if the range is a single value.
    /// </summary>
    /// <remarks>
    /// A range is considered a single value if the minimum and maximum value are equal.
    /// </remarks>
    public bool IsSingleValue => Min == Max;

    /// <summary>
    /// The midpoint of the range.
    /// </summary>
    public long Midpoint => Min + (Max - Min) / 2;

    /// <summary>
    /// The precise midpoint of the range.
    /// </summary>
    public double PreciseMidpoint => Min + (Max - Min) / 2.0;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueRange"/> struct.
    /// </summary>
    public ValueRange()
    {
        Min = 0;
        Max = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueRange"/> struct.
    /// </summary>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    public ValueRange(long min, long max)
    {
        Min = Math.Min(min, max);
        Max = Math.Max(min, max);
    }

    /// <inheritdoc/>
    public bool Equals(ValueRange other)
    {
        return Min == other.Min && Max == other.Max;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ValueRange other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Min, Max);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[{Min}, {Max}]";
    }

    /// <inheritdoc/>
    public int CompareTo(ValueRange other)
    {
        var startComparison = Min.CompareTo(other.Min);
        return startComparison != 0 ? startComparison : Max.CompareTo(other.Max);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var min = Min.ToString(format, formatProvider);
        var max = Max.ToString(format, formatProvider);

        return $"[{min}, {max}]";
    }

    /// <summary>
    /// Checks if the range overlaps with another range.
    /// </summary>
    /// <param name="other">The other range to check for overlap.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(ValueRange other)
    {
        return Min <= other.Max && Max >= other.Min;
    }

    /// <summary>
    /// Checks if the range overlaps with another range.
    /// </summary>
    /// <param name="other">The other range to check for overlap.</param>
    /// <param name="minimumOverlap">The minimum overlap required for the ranges to be considered overlapping.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(ValueRange other, long minimumOverlap)
    {
        var adjustedMinimumOverlap = Math.Max(0, minimumOverlap);
        var overlap = Math.Min(Max, other.Max) - Math.Max(Min, other.Min);
        return overlap >= 0 && overlap >= adjustedMinimumOverlap;
    }

    /// <summary>
    /// Checks if the range contains a value.
    /// </summary>
    /// <param name="value">The value to check for containment.</param>
    /// <returns><see langword="true"/> if the range contains the value; otherwise, <see langword="false"/>.</returns>
    public bool Contains(long value)
    {
        return value >= Min && value <= Max;
    }

    /// <summary>
    /// Checks if the range contains a value.
    /// </summary>
    /// <param name="value">The value to check for containment.</param>
    /// <param name="threshold">The threshold to use for the check.</param>
    /// <returns><see langword="true"/> if the range contains the value; otherwise, <see langword="false"/>.</returns>
    public bool Contains(long value, long threshold)
    {
        var adjustedThreshold = Math.Max(0, threshold);
        return value >= Min - adjustedThreshold && value <= Max + adjustedThreshold;
    }

    /// <summary>
    /// Checks if the range contains another range.
    /// </summary>
    /// <param name="other">The other range to check for containment.</param>
    /// <returns><see langword="true"/> if the range contains the other range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(ValueRange other)
    {
        return Min <= other.Min && Max >= other.Max;
    }

    /// <summary>
    /// Checks if the range contains another range.
    /// </summary>
    /// <param name="other">The other range to check for containment.</param>
    /// <param name="threshold">The threshold to use for the check.</param>
    /// <returns><see langword="true"/> if the range contains the other range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(ValueRange other, long threshold)
    {
        var adjustedThreshold = Math.Max(0, threshold);
        return Min - adjustedThreshold <= other.Min && Max + adjustedThreshold >= other.Max;
    }

    /// <summary>
    /// Returns the intersection of the range with another range if it exists.
    /// </summary>
    /// <param name="other">The other range to intersect with.</param>
    /// <returns>The intersection of the ranges if it exists; otherwise, <see langword="null"/>.</returns>
    public ValueRange? Intersection(ValueRange other)
    {
        if (!Overlaps(other))
        {
            return null;
        }

        var min = Math.Max(Min, other.Min);
        var max = Math.Min(Max, other.Max);
        return new ValueRange(min, max);
    }

    /// <summary>
    /// Checks two <see cref="ValueRange"/> instances for equality.
    /// </summary>
    /// <param name="left">The first <see cref="ValueRange"/> instance.</param>
    /// <param name="right">The second <see cref="ValueRange"/> instance.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ValueRange left, ValueRange right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks two <see cref="ValueRange"/> instances for inequality.
    /// </summary>
    /// <param name="left">The first <see cref="ValueRange"/> instance.</param>
    /// <param name="right">The second <see cref="ValueRange"/> instance.</param>
    /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ValueRange left, ValueRange right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Compares two <see cref="ValueRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="ValueRange"/> instance.</param>
    /// <param name="right">The second <see cref="ValueRange"/> instance.</param>
    /// <returns><see langword="true"/> if the first instance is less than the second instance; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(ValueRange left, ValueRange right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Compares two <see cref="ValueRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="ValueRange"/> instance.</param>
    /// <param name="right">The second <see cref="ValueRange"/> instance.</param>
    /// <returns><see langword="true"/> if the first instance is less than or equal to the second instance; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(ValueRange left, ValueRange right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Compares two <see cref="ValueRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="ValueRange"/> instance.</param>
    /// <param name="right">The second <see cref="ValueRange"/> instance.</param>
    /// <returns><see langword="true"/> if the first instance is greater than the second instance; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(ValueRange left, ValueRange right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Compares two <see cref="ValueRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="ValueRange"/> instance.</param>
    /// <param name="right">The second <see cref="ValueRange"/> instance.</param>
    /// <returns><see langword="true"/> if the first instance is greater than or equal to the second instance; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(ValueRange left, ValueRange right)
    {
        return left.CompareTo(right) >= 0;
    }
}
