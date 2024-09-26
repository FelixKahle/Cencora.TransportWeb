// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;
using Cencora.TransportWeb.VehicleRouting.Common;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Builder for a vehicle.
/// </summary>
public sealed class VehicleBuilder : FlaggedBuilder
{
    private Id _id;
    private HashSet<Shift> _shifts = new HashSet<Shift>();
    private long? _fixedCost;
    private long? _baseCost;
    private long? _distanceCost;
    private long? _timeCost;
    private long? _weightCost;
    private long? _costPerWeightDistance;
    private long? _maxWeight;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleBuilder"/> class.
    /// </summary>
    public VehicleBuilder()
    {
        _id = Id.New();
    }

    /// <summary>
    /// Adds a id to the vehicle.
    /// </summary>
    /// <param name="id">The id to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithId(Id id)
    {
        _id = id;
        return this;
    }

    /// <summary>
    /// Adds an automatic id to the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithAutomaticId()
    {
        _id = Id.New();
        return this;
    }

    /// <summary>
    /// Adds a shift to the vehicle.
    /// </summary>
    /// <param name="shift">The shift to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithShift(Shift shift)
    {
        _shifts.Add(shift);
        return this;
    }

    /// <summary>
    /// Adds a shift to the vehicle.
    /// </summary>
    /// <param name="factory">The factory to create the shift.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">If the factory is null.</exception>
    public VehicleBuilder WithShift(Func<ShiftBuilder, Shift> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var builder = new ShiftBuilder();
        var shift = factory(builder);
        _shifts.Add(shift);
        return this;
    }

    /// <summary>
    /// Adds a shift to the vehicle.
    /// </summary>
    /// <param name="factory">The factory to create the shift.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">If the factory is null.</exception>
    public VehicleBuilder WithShift(Func<Shift> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var shift = factory();
        _shifts.Add(shift);
        return this;
    }

    /// <summary>
    /// Adds shifts to the vehicle.
    /// </summary>
    /// <param name="shifts">The shifts to add.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">If the shifts are null.</exception>
    public VehicleBuilder WithShifts(IEnumerable<Shift> shifts)
    {
        ArgumentNullException.ThrowIfNull(shifts, nameof(shifts));

        foreach (var shift in shifts)
        {
            _shifts.Add(shift);
        }
        return this;
    }

    /// <summary>
    /// Removes a shift from the vehicle.
    /// </summary>
    /// <param name="shift">The shift to remove.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutShift(Shift shift)
    {
        _shifts.Remove(shift);
        return this;
    }

    /// <summary>
    /// Removes shifts from the vehicle.
    /// </summary>
    /// <param name="shifts">The shifts to remove.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">If the shifts are null.</exception>
    public VehicleBuilder WithoutShifts(IEnumerable<Shift> shifts)
    {
        ArgumentNullException.ThrowIfNull(shifts, nameof(shifts));

        foreach (var shift in shifts)
        {
            _shifts.Remove(shift);
        }
        return this;
    }

    /// <summary>
    /// Removes all shifts from the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutShifts()
    {
        _shifts.Clear();
        return this;
    }

    /// <summary>
    /// Adds a fixed cost to the vehicle.
    /// </summary>
    /// <param name="fixedCost">The fixed cost to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithFixedCost(long fixedCost)
    {
        _fixedCost = fixedCost;
        return this;
    }

    /// <summary>
    /// Removes the fixed cost from the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutFixedCost()
    {
        _fixedCost = null;
        return this;
    }

    /// <summary>
    /// Adds a base cost to the vehicle.
    /// </summary>
    /// <param name="baseCost">The base cost to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithBaseCost(long baseCost)
    {
        _baseCost = baseCost;
        return this;
    }

    /// <summary>
    /// Removes the base cost from the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutBaseCost()
    {
        _baseCost = null;
        return this;
    }

    /// <summary>
    /// Adds a distance cost to the vehicle.
    /// </summary>
    /// <param name="distanceCost">The distance cost to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithDistanceCost(long distanceCost)
    {
        _distanceCost = distanceCost;
        return this;
    }

    /// <summary>
    /// Removes the distance cost from the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutDistanceCost()
    {
        _distanceCost = null;
        return this;
    }

    /// <summary>
    /// Adds a time cost to the vehicle.
    /// </summary>
    /// <param name="timeCost">The time cost to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithTimeCost(long timeCost)
    {
        _timeCost = timeCost;
        return this;
    }

    /// <summary>
    /// Removes the time cost from the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutTimeCost()
    {
        _timeCost = null;
        return this;
    }

    /// <summary>
    /// Adds a weight cost to the vehicle.
    /// </summary>
    /// <param name="weightCost">The weight cost to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithWeightCost(long weightCost)
    {
        _weightCost = weightCost;
        return this;
    }

    /// <summary>
    /// Removes the weight cost from the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutWeightCost()
    {
        _weightCost = null;
        return this;
    }

    /// <summary>
    /// Adds a cost per weight distance to the vehicle.
    /// </summary>
    /// <param name="costPerWeightDistance">The cost per weight distance to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithCostPerWeightDistance(long costPerWeightDistance)
    {
        _costPerWeightDistance = costPerWeightDistance;
        return this;
    }

    /// <summary>
    /// Removes the cost per weight distance from the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutCostPerWeightDistance()
    {
        _costPerWeightDistance = null;
        return this;
    }

    /// <summary>
    /// Adds a max weight to the vehicle.
    /// </summary>
    /// <param name="maxWeight">The max weight to add.</param>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithMaxWeight(long maxWeight)
    {
        _maxWeight = maxWeight;
        return this;
    }

    /// <summary>
    /// Removes the max weight from the vehicle.
    /// </summary>
    /// <returns>The builder.</returns>
    public VehicleBuilder WithoutMaxWeight()
    {
        _maxWeight = null;
        return this;
    }

    /// <summary>
    /// Builds the vehicle.
    /// </summary>
    /// <returns>The vehicle.</returns>
    public Vehicle Build()
    {
        return new Vehicle(_id, _shifts, _fixedCost, _baseCost, _distanceCost, _timeCost, _weightCost,
            _costPerWeightDistance, _maxWeight, BuildFlags());
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return "VehicleBuilder";
    }
}
