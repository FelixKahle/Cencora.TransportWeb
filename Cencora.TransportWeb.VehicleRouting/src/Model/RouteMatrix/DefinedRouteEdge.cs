// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;

/// <summary>
/// Represents a defined edge in a route matrix.
/// </summary>
public readonly struct DefinedRouteEdge : IRouteEdge, IEquatable<DefinedRouteEdge>, IComparable<DefinedRouteEdge>
{
    /// <summary>
    /// The distance of the edge.
    /// </summary>
    public long Distance { get; }

    /// <summary>
    /// The time it takes to travel the edge.
    /// </summary>
    public long Duration { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefinedRouteEdge"/> struct.
    /// </summary>
    /// <param name="distance">The distance of the edge.</param>
    /// <param name="duration">The time it takes to travel the edge.</param>
    public DefinedRouteEdge(long distance, long duration)
    {
        Distance = distance;
        Duration = duration;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"({Distance}, {Duration})";
    }

    /// <inheritdoc />
    public bool Equals(DefinedRouteEdge other)
    {
        return Distance.Equals(other.Distance) && Duration.Equals(other.Duration);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is DefinedRouteEdge other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Distance, Duration);
    }

    /// <inheritdoc />
    public int CompareTo(DefinedRouteEdge other)
    {
        var distanceComparison = Distance.CompareTo(other.Distance);
        var durationComparison = Duration.CompareTo(other.Duration);

        return distanceComparison > 0 && durationComparison > 0 ? 1 :
            distanceComparison < 0 && durationComparison < 0 ? -1 : 0;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="DefinedRouteEdge"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <param name="right">The second <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <returns><see langword="true"/> if the instances represent the same edge; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(DefinedRouteEdge left, DefinedRouteEdge right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="DefinedRouteEdge"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <param name="right">The second <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <returns><see langword="true"/> if the instances represent different edges; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(DefinedRouteEdge left, DefinedRouteEdge right)
    {
        return !left.Equals(right);
    }
    
    /// <summary>
    /// Determines whether one specified instance of <see cref="DefinedRouteEdge"/> is greater than another specified instance.
    /// </summary>
    /// <param name="left">The first <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <param name="right">The second <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(DefinedRouteEdge left, DefinedRouteEdge right)
    {
        return left.CompareTo(right) > 0;
    }
    
    /// <summary>
    /// Determines whether one specified instance of <see cref="DefinedRouteEdge"/> is less than another specified instance.
    /// </summary>
    /// <param name="left">The first <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <param name="right">The second <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(DefinedRouteEdge left, DefinedRouteEdge right)
    {
        return left.CompareTo(right) < 0;
    }
    
    /// <summary>
    /// Determines whether one specified instance of <see cref="DefinedRouteEdge"/> is greater than or equal to another specified instance.
    /// </summary>
    /// <param name="left">The first <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <param name="right">The second <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(DefinedRouteEdge left, DefinedRouteEdge right)
    {
        return left.CompareTo(right) >= 0;
    }
    
    /// <summary>
    /// Determines whether one specified instance of <see cref="DefinedRouteEdge"/> is less than or equal to another specified instance.
    /// </summary>
    /// <param name="left">The first <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <param name="right">The second <see cref="DefinedRouteEdge"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(DefinedRouteEdge left, DefinedRouteEdge right)
    {
        return left.CompareTo(right) <= 0;
    }
}
