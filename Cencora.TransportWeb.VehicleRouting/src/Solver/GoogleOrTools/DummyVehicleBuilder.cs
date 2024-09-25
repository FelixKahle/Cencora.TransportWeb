// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Builder for a dummy vehicle.
/// </summary>
/// <remarks>
/// Note that you need to call <see cref="WithIndex"/>, <see cref="WithVehicle"/> and <see cref="WithShift"/>,
/// before calling <see cref="Build"/>.
/// You can also use the constructor to set the index, vehicle, shift, start node and end node.
/// </remarks>
internal sealed class DummyVehicleBuilder
{
    private int _index = -1;
    private Vehicle? _vehicle;
    private Shift? _shift;
    private long _fixedCost;
    private long _baseCost;
    private long _distanceCost;
    private long _timeCost;
    private long _weightCost;
    private long _costPerWeightDistance;
    private long _maxWeight;
    private long _maxDuration;
    private long _maxDistance;

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
    internal DummyVehicleBuilder(int index, Vehicle vehicle, Shift shift)
    {
        _index = index;
        _vehicle = vehicle;
        _shift = shift;
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
    /// Adds the max weight.
    /// </summary>
    /// <param name="maxWeight">The max weight.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithMaxWeight(long maxWeight)
    {
        _maxWeight = maxWeight;
        return this;
    }

    /// <summary>
    /// Adds the max duration.
    /// </summary>
    /// <param name="maxDuration">The max duration.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithMaxDuration(long maxDuration)
    {
        _maxDuration = maxDuration;
        return this;
    }

    /// <summary>
    /// Adds the max distance.
    /// </summary>
    /// <param name="maxDistance">The max distance.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithMaxDistance(long maxDistance)
    {
        _maxDistance = maxDistance;
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

        return new DummyVehicle(index, vehicle, shift, _fixedCost, _baseCost, _distanceCost,
            _timeCost, _weightCost, _costPerWeightDistance, _maxWeight, _maxDuration, _maxDistance);
    }
}
