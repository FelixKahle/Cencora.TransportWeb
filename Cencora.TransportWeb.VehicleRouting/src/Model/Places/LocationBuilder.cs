// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;
using Cencora.TransportWeb.VehicleRouting.Common;

namespace Cencora.TransportWeb.VehicleRouting.Model.Places;

/// <summary>
/// Builder class for creating instances of the <see cref="Location"/> class.
/// </summary>
public sealed class LocationBuilder : FlaggedBuilder
{
    private Id _id;
    private long? _maximalVehicleCapacity;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationBuilder"/> class.
    /// </summary>
    public LocationBuilder()
    {
        _id = Id.New();
    }

    /// <summary>
    /// Sets the ID of the location.
    /// </summary>
    /// <param name="id">The ID of the location.</param>
    /// <returns>The current instance of the <see cref="LocationBuilder"/> class.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is <see langword="null"/> or whitespace.</exception>
    public LocationBuilder WithId(Id id)
    {
        _id = id;
        return this;
    }

    /// <summary>
    /// Sets the ID of the location to a new GUID.
    /// </summary>
    /// <returns>The current instance of the <see cref="LocationBuilder"/> class.</returns>
    public LocationBuilder WithAutomaticId()
    {
        _id = Id.New();
        return this;
    }

    /// <summary>
    /// Sets the maximal vehicle capacity of the location.
    /// </summary>
    /// <param name="maximalVehicleCapacity">The maximal vehicle capacity of the location.</param>
    /// <returns>The current instance of the <see cref="LocationBuilder"/> class.</returns>
    public LocationBuilder WithMaximalVehicleCapacity(long? maximalVehicleCapacity)
    {
        _maximalVehicleCapacity = maximalVehicleCapacity;
        return this;
    }

    /// <summary>
    /// Removes the maximal vehicle capacity from the location.
    /// </summary>
    public LocationBuilder WithoutMaximalVehicleCapacity()
    {
        _maximalVehicleCapacity = null;
        return this;
    }

    /// <summary>
    /// Builds and returns an instance of the <see cref="Location"/> class using the specified properties.
    /// </summary>
    /// <returns>An instance of the <see cref="Location"/> class.</returns>
    public Location Build()
    {
        return new Location(_id, _maximalVehicleCapacity, BuildFlags());
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return "LocationBuilder";
    }
}
