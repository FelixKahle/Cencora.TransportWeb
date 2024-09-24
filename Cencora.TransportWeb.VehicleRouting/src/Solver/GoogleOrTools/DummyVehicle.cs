// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;
using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;

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
    /// Returns the Id of the vehicle.
    /// </summary>
    internal Id Id => Vehicle.Id;

    /// <summary>
    /// The start node of the vehicle.
    /// </summary>
    internal Node StartNode { get; }

    /// <summary>
    /// The end node of the vehicle.
    /// </summary>
    internal Node EndNode { get; }

    /// <summary>
    /// Gets the time window in which the vehicle is available.
    /// </summary>
    public ValueRange? AvailableTimeWindow { get; set; }

    /// <summary>
    /// Gets the fixed cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The fixed cost is a fixed amount of cost that is applied
    /// always, no matter how the vehicle is used.
    /// </remarks>
    public long FixedCost { get; }

    /// <summary>
    /// Gets the base cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The base cost is a fixed amount of cost that is applied
    /// as soon as the vehicle is used.
    /// </remarks>
    public long BaseCost { get; }

    /// <summary>
    /// Gets the distance cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The distance cost is applied per distance unit.
    /// </remarks>
    public long DistanceCost { get; }

    /// <summary>
    /// Gets the time cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The time cost is applied per time unit.
    /// </remarks>
    public long TimeCost { get; }

    /// <summary>
    /// Gets the weight cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The weight cost is applied per weight unit.
    /// </remarks>
    public long WeightCost { get; }

    /// <summary>
    /// Gets the weight per distance cost of the vehicle.
    /// </summary>
    /// <remarks>
    /// The weight per distance cost is applied per weight multiplied by distance unit.
    /// </remarks>
    public long CostPerWeightDistance { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyVehicle"/> class.
    /// </summary>
    /// <param name="index">The index of the vehicle.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="startNode">The start node of the vehicle.</param>
    /// <param name="endNode">The end node of the vehicle.</param>
    /// <param name="availableTimeWindow">The time window in which the vehicle is available.</param>
    /// <param name="fixedCost">The fixed cost of the vehicle.</param>
    /// <param name="baseCost">The base cost of the vehicle.</param>
    /// <param name="distanceCost">The distance cost of the vehicle.</param>
    /// <param name="timeCost">The time cost of the vehicle.</param>
    /// <param name="weightCost">The weight cost of the vehicle.</param>
    /// <param name="costPerWeightDistance">The weight per distance cost of the vehicle.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/>, <paramref name="startNode"/> or <paramref name="endNode"/> is <see langword="null"/>.</exception>
    internal DummyVehicle(int index, Vehicle vehicle, Node startNode, Node endNode, ValueRange? availableTimeWindow, long fixedCost, long baseCost, long distanceCost, long timeCost, long weightCost, long costPerWeightDistance)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(startNode, nameof(startNode));
        ArgumentNullException.ThrowIfNull(endNode, nameof(endNode));

        Index = index;
        Vehicle = vehicle;
        StartNode = startNode;
        EndNode = endNode;
        AvailableTimeWindow = availableTimeWindow;
        FixedCost = fixedCost;
        BaseCost = baseCost;
        DistanceCost = distanceCost;
        TimeCost = timeCost;
        WeightCost = weightCost;
        CostPerWeightDistance = costPerWeightDistance;
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
