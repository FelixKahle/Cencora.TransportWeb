// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Builder for a dummy vehicle.
/// </summary>
internal sealed class DummyVehicleBuilder
{
    private int _index;
    private Vehicle _vehicle;
    private Shift _shift;
    private long _fixedCost;
    private long _baseCost;
    private long _distanceCost;
    private long _timeCost;
    private long _weightCost;
    private long _costPerWeightDistance;
    private long _maxWeight;
    private long _maxTotalWeight;
    private long _maxDuration;
    private long _maxDistance;

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyVehicleBuilder"/> class.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="shift">The shift.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    internal DummyVehicleBuilder(int index, Vehicle vehicle, Shift shift)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(shift, nameof(shift));
        
        _index = index;
        _vehicle = vehicle;
        _shift = shift;
    }

    /// <summary>
    /// Adds the index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0.</exception>
    internal DummyVehicleBuilder WithIndex(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        
        _index = index;
        return this;
    }

    /// <summary>
    /// Adds the vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    internal DummyVehicleBuilder WithVehicle(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        
        _vehicle = vehicle;
        return this;
    }

    /// <summary>
    /// Adds the shift.
    /// </summary>
    /// <param name="shift">The shift.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shift"/> is <see langword="null"/>.</exception>
    internal DummyVehicleBuilder WithShift(Shift shift)
    {
        ArgumentNullException.ThrowIfNull(shift, nameof(shift));
        
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
    /// Adds the max total weight.
    /// </summary>
    /// <param name="maxTotalWeight">The max total weight.</param>
    /// <returns>The builder.</returns>
    internal DummyVehicleBuilder WithMaxTotalWeight(long maxTotalWeight)
    {
        _maxTotalWeight = maxTotalWeight;
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
        return new DummyVehicle(_index, _vehicle, _shift, _fixedCost, _baseCost, _distanceCost,
            _timeCost, _weightCost, _costPerWeightDistance, _maxWeight, _maxTotalWeight, _maxDuration,
            _maxDistance);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"DummyVehicleBuilder";
    }
}
