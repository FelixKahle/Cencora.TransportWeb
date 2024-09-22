// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Flags;

/// <summary>
/// Represents a flag that can be used to flag certain things.
/// </summary>
/// <remarks>
/// A flag is a value that can be used to add binary states to objects.
/// For example, a flag can be used to mark an object as "read" or "open".
/// Flags are immutable and can be compared to strings, as they are essentially strings.
/// This class is for convenience and type safety as well as to prevent accidental misuse of strings.
/// </remarks>
public readonly struct Flag :
    IEquatable<Flag>, IEquatable<string>, IComparable<Flag>, IComparable<string>
{
    /// <summary>
    /// Gets the value of the flag.
    /// </summary>
    public string FlagValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Flag"/> class.
    /// </summary>
    /// <param name="flagValue">The value of the flag.</param>
    /// <exception cref="ArgumentException">Thrown when the value is null or whitespace.</exception>
    public Flag(string flagValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(flagValue, nameof(flagValue));

        FlagValue = flagValue;
    }

    /// <inheritdoc/>
    public bool Equals(Flag other)
    {
        return FlagValue.Equals(other.FlagValue, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public bool Equals(string? other)
    {
        return FlagValue.Equals(other, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Flag flag && Equals(flag);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return FlagValue.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return FlagValue;
    }

    /// <inheritdoc/>
    public int CompareTo(Flag other)
    {
        return string.Compare(FlagValue, other.FlagValue, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public int CompareTo(string? other)
    {
        return string.Compare(FlagValue, other, StringComparison.Ordinal);
    }

    /// <summary>
    /// Implicitly converts a flag to a <see cref="string"/>.
    /// </summary>
    /// <param name="flag">The flag to convert.</param>
    /// <returns>The value of the flag.</returns>
    public static implicit operator string(Flag flag)
    {
        return flag.FlagValue;
    }

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to a flag.
    /// </summary>
    /// <param name="flagValue">The value of the flag.</param>
    /// <returns>A new instance of the <see cref="Flag"/> class.</returns>
    public static implicit operator Flag(string flagValue)
    {
        return new Flag(flagValue);
    }

    /// <summary>
    /// Compares two flags for equality.
    /// </summary>
    /// <param name="left">The left flag.</param>
    /// <param name="right">The right flag.</param>
    /// <returns><see langword="true"/> if the flags are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Flag? left, Flag? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <summary>
    /// Compares two flags for inequality.
    /// </summary>
    /// <param name="left">The left flag.</param>
    /// <param name="right">The right flag.</param>
    /// <returns><see langword="true"/> if the flags are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Flag? left, Flag? right)
    {
        return !(left == right);
    }
}
