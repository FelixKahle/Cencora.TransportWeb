// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;

namespace Cencora.TransportWeb.VehicleRouting.Model.Shipment;

/// <summary>
/// A builder for the <see cref="ShipUnit"/>.
/// </summary>
public sealed class ShipUnitBuilder
{
    private Id _id;
    private long? _weight;
    private long? _width;
    private long? _height;
    private long? _length;

    /// <summary>
    /// Creates a new instance of the <see cref="ShipUnitBuilder"/>.
    /// </summary>
    public ShipUnitBuilder()
    {
        _id = Id.New();
    }

    /// <summary>
    /// Adds an id to the ship unit.
    /// </summary>
    /// <param name="id">The id of the ship unit.</param>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithId(Id id)
    {
        _id = id;
        return this;
    }

    /// <summary>
    /// Adds an automatic id to the ship unit.
    /// </summary>
    public ShipUnitBuilder WithAutomaticId()
    {
        _id = Id.New();
        return this;
    }

    /// <summary>
    /// Adds a weight to the ship unit.
    /// </summary>
    /// <param name="weight">The weight of the ship unit.</param>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithWeight(long weight)
    {
        _weight = weight;
        return this;
    }

    /// <summary>
    /// Removes the weight from the ship unit.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithoutWeight()
    {
        _weight = null;
        return this;
    }

    /// <summary>
    /// Adds a width to the ship unit.
    /// </summary>
    /// <param name="width">The width of the ship unit.</param>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithWidth(long width)
    {
        _width = width;
        return this;
    }

    /// <summary>
    /// Removes the width from the ship unit.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithoutWidth()
    {
        _width = null;
        return this;
    }

    /// <summary>
    /// Adds a height to the ship unit.
    /// </summary>
    /// <param name="height">The height of the ship unit.</param>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithHeight(long height)
    {
        _height = height;
        return this;
    }

    /// <summary>
    /// Removes the height from the ship unit.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithoutHeight()
    {
        _height = null;
        return this;
    }

    /// <summary>
    /// Adds a length to the ship unit.
    /// </summary>
    /// <param name="length">The length of the ship unit.</param>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithLength(long length)
    {
        _length = length;
        return this;
    }

    /// <summary>
    /// Removes the length from the ship unit.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShipUnitBuilder WithoutLength()
    {
        _length = null;
        return this;
    }

    /// <summary>
    /// Builds the ship unit.
    /// </summary>
    /// <returns>The ship unit.</returns>
    public ShipUnit Build()
    {
        return new ShipUnit(_id, _weight, _width, _height, _length);
    }
}
