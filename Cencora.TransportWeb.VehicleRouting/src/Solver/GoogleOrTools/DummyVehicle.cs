// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

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
    /// The start node of the vehicle.
    /// </summary>
    internal Node StartNode { get; }

    /// <summary>
    /// The end node of the vehicle.
    /// </summary>
    internal Node EndNode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyVehicle"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="startNode">The start node of the vehicle.</param>
    /// <param name="endNode">The end node of the vehicle.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/>, <paramref name="startNode"/> or <paramref name="endNode"/> is null.</exception>
    internal DummyVehicle(int index, Vehicle vehicle, Node startNode, Node endNode)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(startNode, nameof(startNode));
        ArgumentNullException.ThrowIfNull(endNode, nameof(endNode));

        Index = index;
        Vehicle = vehicle;
        StartNode = startNode;
        EndNode = endNode;
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
        return $"DummyVehicle: {Index}";
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
        return !Equals(left, right);
    }
}
