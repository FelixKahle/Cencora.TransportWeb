// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;

/// <summary>
/// Represents a pair of start and end location.
/// </summary>
public readonly struct LocationPair : IEquatable<LocationPair>
{
    /// <summary>
    /// The start location.
    /// </summary>
    public Location From { get; }

    /// <summary>
    /// The end location.
    /// </summary>
    public Location To { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationPair"/> struct.
    /// </summary>
    /// <param name="from">The start location.</param>
    /// <param name="to">The end location.</param>
    public LocationPair(Location from, Location to)
    {
        From = from;
        To = to;
    }

    /// <inheritdoc/>
    public bool Equals(LocationPair other)
    {
        return From.Equals(other.From) && To.Equals(other.To);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is LocationPair other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(From, To);
    }

    /// <summary>
    /// Checks if two <see cref="LocationPair"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="LocationPair"/> instance.</param>
    /// <param name="right">The second <see cref="LocationPair"/> instance.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(LocationPair left, LocationPair right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks if two <see cref="LocationPair"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="LocationPair"/> instance.</param>
    /// <param name="right">The second <see cref="LocationPair"/> instance.</param>
    /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(LocationPair left, LocationPair right)
    {
        return !left.Equals(right);
    }
}
