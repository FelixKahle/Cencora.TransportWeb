// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;
using Cencora.TransportWeb.VehicleRouting.Common;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicle;

/// <summary>
/// Builder for <see cref="Driver"/>.
/// </summary>
public sealed class DriverBuilder : FlaggedBuilder
{
    private Id _id;
    private long? _fixedCost;

    /// <summary>
    /// Adds an id to the driver.
    /// </summary>
    /// <param name="id">The id to add.</param>
    /// <returns>The builder.</returns>
    public DriverBuilder WithId(Id id)
    {
        _id = id;
        return this;
    }

    /// <summary>
    /// Adds an automatic id to the driver.
    /// </summary>
    /// <returns>The builder.</returns>
    public DriverBuilder WithAutomaticId()
    {
        _id = Id.New();
        return this;
    }

    /// <summary>
    /// Adds a fixed cost to the driver.
    /// </summary>
    /// <param name="fixedCost">The fixed cost to add.</param>
    /// <returns>The builder.</returns>
    public DriverBuilder WithFixedCost(long fixedCost)
    {
        _fixedCost = fixedCost;
        return this;
    }

    /// <summary>
    /// Removes the fixed cost from the driver.
    /// </summary>
    /// <returns>The builder.</returns>
    public DriverBuilder WithoutFixedCost()
    {
        _fixedCost = null;
        return this;
    }

    public Driver Build()
    {
        return new Driver(_id, _fixedCost, BuildFlags());
    }
}
