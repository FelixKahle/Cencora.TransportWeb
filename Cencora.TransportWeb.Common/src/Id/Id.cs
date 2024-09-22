// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Id;

/// <summary>
/// Represents an identifier.
/// </summary>
public readonly struct Id : IEquatable<Id>, IComparable<Id>
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Id"/> struct.
    /// </summary>
    public Id()
    {
        Value = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Id"/> struct.
    /// </summary>
    /// <param name="value">The value of the identifier.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null or whitespace.</exception>
    public Id(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        Value = value;
    }

    /// <summary>
    /// Creates a new identifier.
    /// </summary>
    /// <returns>A new identifier.</returns>
    public static Id New() => new Id(Guid.NewGuid().ToString());

    /// <summary>
    /// Creates a new identifier from the specified value.
    /// </summary>
    /// <param name="value">The value of the identifier.</param>
    /// <returns>A new identifier.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null or whitespace.</exception>
    public static Id New(string value) => new Id(value);

    /// <summary>
    /// Implicitly converts the identifier to a string.
    /// </summary>
    public static implicit operator string(Id id) => id.Value;

    /// <summary>
    /// Implicitly converts a string to an identifier.
    /// </summary>
    public static implicit operator Id(string value) => new Id(value);

    /// <summary>
    /// Creates a new identifier from the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value of the identifier.</param>
    /// <returns>A new identifier.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    public static Id New<T>(T value)
    {
        var str = value?.ToString() ?? throw new ArgumentNullException(nameof(value));

        return new Id(str);
    }

    /// <inheritdoc/>
    public bool Equals(Id other)
    {
        return Value.Equals(other.Value, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Id other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value;
    }

    /// <inheritdoc/>
    public int CompareTo(Id other)
    {
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Id"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Id"/> to compare.</param>
    /// <param name="right">The second <see cref="Id"/> to compare.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Id left, Id right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Id"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Id"/> to compare.</param>
    /// <param name="right">The second <see cref="Id"/> to compare.</param>
    /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Id left, Id right)
    {
        return !left.Equals(right);
    }
}
