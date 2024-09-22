// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Time;

/// <summary>
/// Represents a time window with a start and end time.
/// </summary>
public readonly struct DateTimeOffsetWindow
    : IEquatable<DateTimeOffsetWindow>, IComparable<DateTimeOffsetWindow>, IFormattable
{
    /// <summary>
    /// The start of the window.
    /// </summary>
    public DateTimeOffset Start { get; }

    /// <summary>
    /// The end of the window.
    /// </summary>
    public DateTimeOffset End { get; }

    /// <summary>
    /// A window that has both <see cref="DateTimeOffset.MinValue"/> as start and end time.
    /// </summary>
    public static readonly DateTimeOffsetWindow Min = new DateTimeOffsetWindow(DateTimeOffset.MinValue, DateTimeOffset.MinValue);

    /// <summary>
    /// A window that has both <see cref="DateTimeOffset.MaxValue"/> as start and end time.
    /// </summary>
    public static readonly DateTimeOffsetWindow Max = new DateTimeOffsetWindow(DateTimeOffset.MaxValue, DateTimeOffset.MaxValue);

    /// <summary>
    /// A window that spans has <see cref="DateTimeOffset.MinValue"/> as start time and <see cref="DateTimeOffset.MaxValue"/> as end time.
    /// </summary>
    public static readonly DateTimeOffsetWindow FullRange = new DateTimeOffsetWindow(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetWindow"/> struct.
    /// </summary>
    /// <param name="start">The start time of the window.</param>
    /// <param name="end">The end time of the window.</param>
    /// <remarks>
    /// If <paramref name="end"/> is less than <paramref name="start"/>, the values are swapped.
    /// </remarks>
    public DateTimeOffsetWindow(DateTimeOffset start, DateTimeOffset end)
    {
        Start = end < start ? end : start;
        End = end < start ? start : end;
    }

    /// <summary>
    /// Gets the duration of the window.
    /// </summary>
    public TimeSpan Duration => End - Start;

    /// <summary>
    /// Gets a value indicating whether the window is a time point.
    /// </summary>
    public bool IsTimePoint => Start == End;

    /// <inheritdoc/>
    public bool Equals(DateTimeOffsetWindow other)
    {
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is DateTimeOffsetWindow other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }

    /// <inheritdoc/>
    public int CompareTo(DateTimeOffsetWindow other)
    {
        var startComparison = Start.CompareTo(other.Start);
        return startComparison != 0 ? startComparison : End.CompareTo(other.End);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[{Start}, {End}]";
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var start = Start.ToString(format, formatProvider);
        var end = End.ToString(format, formatProvider);

        return $"[{start}, {end}]";
    }

    /// <summary>
    /// Checks if this time window overlaps with another time window.
    /// </summary>
    /// <param name="other">The other time window to check for overlap.</param>
    /// <returns><see langword="true"/> if the time windows overlap, <see langword="false"/> otherwise.</returns>
    public bool Overlaps(DateTimeOffsetWindow other)
    {
        return Start <= other.End && End >= other.Start;
    }

    /// <summary>
    /// Checks if this time window overlaps with another time window by at least the specified minimum overlap duration.
    /// </summary>
    /// <param name="other">The other time window to check for overlap.</param>
    /// <param name="minimumOverlap">The minimum overlap duration required for the time windows to be considered overlapping.</param>
    /// <returns><see langword="true"/> if the time windows overlap by at least the minimum overlap duration, <see langword="false"/> otherwise.</returns>
    public bool Overlaps(DateTimeOffsetWindow other, TimeSpan minimumOverlap)
    {
        var latestStart = Start > other.Start ? Start : other.Start;
        var earliestEnd = End < other.End ? End : other.End;
        var overlapDuration = earliestEnd - latestStart;

        if (overlapDuration <= TimeSpan.Zero)
        {
            return false;
        }

        return overlapDuration >= minimumOverlap;
    }

    /// <summary>
    /// Checks if this time window is contained within another time window.
    /// </summary>
    /// <param name="other">The other time window to check for containment.</param>
    /// <returns><see langword="true"/> if this time window is contained within the other time window, <see langword="false"/> otherwise.</returns>
    public bool Contains(DateTimeOffsetWindow other)
    {
        return Start <= other.Start && End >= other.End;
    }

    /// <summary>
    /// Checks if this time window is contained within another time window with a specified threshold.
    /// </summary>
    /// <param name="other">The other time window to check for containment.</param>
    /// <param name="threshold">The threshold to apply to the start and end times of the other time window.</param>
    /// <returns><see langword="true"/> if this time window is contained within the other time window, <see langword="false"/> otherwise.</returns>
    public bool Contains(DateTimeOffsetWindow other, TimeSpan threshold)
    {
        var adjustedStart = Start - threshold;
        var adjustedEnd = End + threshold;
        return adjustedStart <= other.Start && adjustedEnd >= other.End;
    }

    /// <summary>
    /// Checks if this time window contains a specific timestamp.
    /// </summary>
    /// <param name="timestamp">The timestamp to check for containment.</param>
    /// <returns><see langword="true"/> if this time window contains the timestamp, <see langword="false"/> otherwise.</returns>
    public bool Contains(DateTimeOffset timestamp)
    {
        return Start <= timestamp && End >= timestamp;
    }

    /// <summary>
    /// Checks if this time window contains a specific timestamp with a specified threshold.
    /// </summary>
    /// <param name="timestamp">The timestamp to check for containment.</param>
    /// <param name="threshold">The threshold to apply to the start and end times of the time window.</param>
    /// <returns><see langword="true"/> if this time window contains the timestamp, <see langword="false"/> otherwise.</returns>
    public bool Contains(DateTimeOffset timestamp, TimeSpan threshold)
    {
        var adjustedStart = Start - threshold;
        var adjustedEnd = End + threshold;
        return adjustedStart <= timestamp && adjustedEnd >= timestamp;
    }

    /// <summary>
    /// Checks if this time window contains a specific date and time.
    /// </summary>
    /// <param name="left">The left time window to compare.</param>
    /// <param name="right">The right time window to compare.</param>
    /// <returns><see langword="true"/> if this time window contains the date and time, <see langword="false"/> otherwise.</returns>
    public static bool operator ==(DateTimeOffsetWindow left, DateTimeOffsetWindow right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks if this time window does not contain a specific date and time.
    /// </summary>
    /// <param name="left">The left time window to compare.</param>
    /// <param name="right">The right time window to compare.</param>
    /// <returns><see langword="true"/> if this time window does not contain the date and time, <see langword="false"/> otherwise.</returns>
    public static bool operator !=(DateTimeOffsetWindow left, DateTimeOffsetWindow right)
    {
        return !left.Equals(right);
    }
}
