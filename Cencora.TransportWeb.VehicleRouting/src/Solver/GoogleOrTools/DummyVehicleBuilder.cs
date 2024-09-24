// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Builder for a dummy vehicle.
/// </summary>
/// <remarks>
/// Note that you need to call <see cref="WithIndex"/>, <see cref="WithVehicle"/>, <see cref="WithShift"/>,
/// <see cref="WithStartNode"/> and <see cref="WithEndNode"/> before calling <see cref="Build"/>.
/// You can also use the constructor to set the index, vehicle, shift, start node and end node.
/// </remarks>
internal sealed class DummyVehicleBuilder
{
    private int _index = -1;
    private Vehicle? _vehicle;
    private Shift? _shift;
    private Node? _startNode;
    private Node? _endNode;
    private long _fixedCost;
    private long _baseCost;
    private long _distanceCost;
    private long _timeCost;
    private long _weightCost;
    private long _costPerWeightDistance;
    private ValueRange? _availableTimeWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyVehicleBuilder"/> class.
    /// </summary>
    internal DummyVehicleBuilder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyVehicleBuilder"/> class.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="shift">The shift.</param>
    /// <param name="startNode">The start node.</param>
    /// <param name="endNode">The end node.</param>
    internal DummyVehicleBuilder(int index, Vehicle vehicle, Shift shift, Node startNode, Node endNode)
    {
        _index = index;
        _vehicle = vehicle;
        _shift = shift;
        _startNode = startNode;
        _endNode = endNode;
    }

    /// <summary>
    /// Adds the index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithIndex(int index)
    {
        _index = index;
        return this;
    }

    /// <summary>
    /// Adds the vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithVehicle(Vehicle vehicle)
    {
        _vehicle = vehicle;
        return this;
    }

    /// <summary>
    /// Adds the shift.
    /// </summary>
    /// <param name="shift">The shift.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithShift(Shift shift)
    {
        _shift = shift;
        return this;
    }

    /// <summary>
    /// Adds the start node.
    /// </summary>
    /// <param name="startNode">The start node.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithStartNode(Node startNode)
    {
        _startNode = startNode;
        return this;
    }

    /// <summary>
    /// Adds the end node.
    /// </summary>
    /// <param name="endNode">The end node.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithEndNode(Node endNode)
    {
        _endNode = endNode;
        return this;
    }

    /// <summary>
    /// Adds the fixed cost.
    /// </summary>
    /// <param name="fixedCost">The fixed cost.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithFixedCost(long fixedCost)
    {
        _fixedCost = fixedCost;
        return this;
    }

    /// <summary>
    /// Adds the base cost.
    /// </summary>
    /// <param name="baseCost">The base cost.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithBaseCost(long baseCost)
    {
        _baseCost = baseCost;
        return this;
    }

    /// <summary>
    /// Adds the distance cost.
    /// </summary>
    /// <param name="distanceCost">The distance cost.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithDistanceCost(long distanceCost)
    {
        _distanceCost = distanceCost;
        return this;
    }

    /// <summary>
    /// Adds the time cost.
    /// </summary>
    /// <param name="timeCost">The time cost.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithTimeCost(long timeCost)
    {
        _timeCost = timeCost;
        return this;
    }

    /// <summary>
    /// Adds the weight cost.
    /// </summary>
    /// <param name="weightCost">The weight cost.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithWeightCost(long weightCost)
    {
        _weightCost = weightCost;
        return this;
    }

    /// <summary>
    /// Adds the cost per weight distance.
    /// </summary>
    /// <param name="costPerWeightDistance">The cost per weight distance.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithCostPerWeightDistance(long costPerWeightDistance)
    {
        _costPerWeightDistance = costPerWeightDistance;
        return this;
    }

    /// <summary>
    /// Adds an available time window to the vehicle.
    /// </summary>
    /// <param name="availableTimeWindow">The available time window.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithAvailableTimeWindow(ValueRange availableTimeWindow)
    {
        _availableTimeWindow = availableTimeWindow;
        return this;
    }

    /// <summary>
    /// Removes the available time window.
    /// </summary>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithoutAvailableTimeWindow()
    {
        _availableTimeWindow = null;
        return this;
    }

    /// <summary>
    /// Builds the vehicle.
    /// </summary>
    /// <returns>The vehicle.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a required property is not set.</exception>
    internal DummyVehicle Build()
    {
        var index = _index >= 0 ? _index : throw new InvalidOperationException("Index must be set.");
        var vehicle = _vehicle ?? throw new InvalidOperationException("Vehicle must be set.");
        var shift = _shift ?? throw new InvalidOperationException("Shift must be set.");
        var startNode = _startNode ?? throw new InvalidOperationException("Start node must be set.");
        var endNode = _endNode ?? throw new InvalidOperationException("End node must be set.");

        return new DummyVehicle(index, vehicle, shift, startNode, endNode, _availableTimeWindow, _fixedCost, _baseCost, _distanceCost, _timeCost, _weightCost, _costPerWeightDistance);
    }
}
