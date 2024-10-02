// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Model;

/// <summary>
/// A builder for creating instances of the <see cref="Problem"/> class.
/// </summary>
public sealed class ProblemBuilder
{
    private readonly HashSet<Location> _locations = new HashSet<Location>();
    private readonly HashSet<Vehicle> _vehicles = new HashSet<Vehicle>();
    private readonly HashSet<Shipment> _shipments = new HashSet<Shipment>();
    private DirectedRouteMatrix? _routeMatrix;
    private long? _maxVehicleWaitingTime;

    /// <summary>
    /// Adds a location to the problem.
    /// </summary>
    /// <param name="location">The location to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="location"/> is <c>null</c>.</exception>
    public ProblemBuilder WithLocation(Location location)
    {
        ArgumentNullException.ThrowIfNull(location, nameof(location));

        _locations.Add(location);
        return this;
    }

    /// <summary>
    /// Adds a location to the problem.
    /// </summary>
    /// <param name="factory">The factory that creates the location to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <c>null</c>.</exception>
    public ProblemBuilder WithLocation(Func<LocationBuilder, Location> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var builder = new LocationBuilder();
        var location = factory(builder);
        return WithLocation(location);
    }

    /// <summary>
    /// Adds a location to the problem.
    /// </summary>
    /// <param name="factory">The factory that creates the location to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <c>null</c>.</exception>
    public ProblemBuilder WithLocation(Func<Location> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var location = factory();
        return WithLocation(location);
    }

    /// <summary>
    /// Adds locations to the problem.
    /// </summary>
    /// <param name="locations">The locations to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="locations"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="locations"/> contains <c>null</c> elements.</exception>
    public ProblemBuilder WithLocations(IEnumerable<Location> locations)
    {
        ArgumentNullException.ThrowIfNull(locations, nameof(locations));

        foreach (var location in locations)
        {
            WithLocation(location);
        }
        return this;
    }

    /// <summary>
    /// Removes a location from the problem.
    /// </summary>
    /// <param name="location">The location to remove.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    public ProblemBuilder WithoutLocation(Location location)
    {
        _locations.Remove(location);
        return this;
    }

    /// <summary>
    /// Removes locations from the problem.
    /// </summary>
    /// <param name="locations">The locations to remove.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="locations"/> is <c>null</c>.</exception>
    public ProblemBuilder WithoutLocations(IEnumerable<Location> locations)
    {
        ArgumentNullException.ThrowIfNull(locations, nameof(locations));

        foreach (var location in locations)
        {
            WithoutLocation(location);
        }
        return this;
    }

    /// <summary>
    /// Removes all locations from the problem.
    /// </summary>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    public ProblemBuilder WithoutLocations()
    {
        _locations.Clear();
        return this;
    }

    /// <summary>
    /// Adds a vehicle to the problem.
    /// </summary>
    /// <param name="vehicle">The vehicle to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is <c>null</c>.</exception>
    public ProblemBuilder WithVehicle(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

        _vehicles.Add(vehicle);
        return this;
    }

    /// <summary>
    /// Adds a vehicle to the problem.
    /// </summary>
    /// <param name="factory">The factory that creates the vehicle to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <c>null</c>.</exception>
    public ProblemBuilder WithVehicle(Func<VehicleBuilder, Vehicle> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var builder = new VehicleBuilder();
        var vehicle = factory(builder);
        return WithVehicle(vehicle);
    }

    /// <summary>
    /// Adds a vehicle to the problem.
    /// </summary>
    /// <param name="factory">The factory that creates the vehicle to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <c>null</c>.</exception>
    public ProblemBuilder WithVehicle(Func<Vehicle> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var vehicle = factory();
        return WithVehicle(vehicle);
    }

    /// <summary>
    /// Adds vehicles to the problem.
    /// </summary>
    /// <param name="vehicles">The vehicles to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> contains <c>null</c> elements.</exception>
    public ProblemBuilder WithVehicles(IEnumerable<Vehicle> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));

        foreach (var vehicle in vehicles)
        {
            WithVehicle(vehicle);
        }
        return this;
    }

    /// <summary>
    /// Removes a vehicle from the problem.
    /// </summary>
    /// <param name="vehicle">The vehicle to remove.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    public ProblemBuilder WithoutVehicle(Vehicle vehicle)
    {
        _vehicles.Remove(vehicle);
        return this;
    }

    /// <summary>
    /// Removes vehicles from the problem.
    /// </summary>
    /// <param name="vehicles">The vehicles to remove.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is <c>null</c>.</exception>
    public ProblemBuilder WithoutVehicles(IEnumerable<Vehicle> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));

        foreach (var vehicle in vehicles)
        {
            WithoutVehicle(vehicle);
        }
        return this;
    }

    /// <summary>
    /// Removes all vehicles from the problem.
    /// </summary>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    public ProblemBuilder WithoutVehicles()
    {
        _vehicles.Clear();
        return this;
    }

    /// <summary>
    /// Adds a shipment to the problem.
    /// </summary>
    /// <param name="shipment">The shipment to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shipment"/> is <c>null</c>.</exception>
    public ProblemBuilder WithShipment(Shipment shipment)
    {
        ArgumentNullException.ThrowIfNull(shipment, nameof(shipment));

        _shipments.Add(shipment);
        return this;
    }

    /// <summary>
    /// Adds a shipment to the problem.
    /// </summary>
    /// <param name="factory">The factory that creates the shipment to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <c>null</c>.</exception>
    public ProblemBuilder WithShipment(Func<ShipmentBuilder, Shipment> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var builder = new ShipmentBuilder();
        var shipment = factory(builder);
        return WithShipment(shipment);
    }

    /// <summary>
    /// Adds a shipment to the problem.
    /// </summary>
    /// <param name="factory">The factory that creates the shipment to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <c>null</c>.</exception>
    public ProblemBuilder WithShipment(Func<Shipment> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var shipment = factory();
        return WithShipment(shipment);
    }

    /// <summary>
    /// Adds shipments to the problem.
    /// </summary>
    /// <param name="shipments">The shipments to add.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shipments"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shipments"/> contains <c>null</c> elements.</exception>
    public ProblemBuilder WithShipments(IEnumerable<Shipment> shipments)
    {
        ArgumentNullException.ThrowIfNull(shipments, nameof(shipments));

        foreach (var shipment in shipments)
        {
            WithShipment(shipment);
        }
        return this;
    }

    /// <summary>
    /// Removes a shipment from the problem.
    /// </summary>
    /// <param name="shipment">The shipment to remove.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    public ProblemBuilder WithoutShipment(Shipment shipment)
    {
        _shipments.Remove(shipment);
        return this;
    }

    /// <summary>
    /// Removes shipments from the problem.
    /// </summary>
    /// <param name="shipments">The shipments to remove.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shipments"/> is <c>null</c>.</exception>
    public ProblemBuilder WithoutShipments(IEnumerable<Shipment> shipments)
    {
        ArgumentNullException.ThrowIfNull(shipments, nameof(shipments));

        foreach (var shipment in shipments)
        {
            WithoutShipment(shipment);
        }
        return this;
    }

    /// <summary>
    /// Removes all shipments from the problem.
    /// </summary>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    public ProblemBuilder WithoutShipments()
    {
        _shipments.Clear();
        return this;
    }

    /// <summary>
    /// Sets the route matrix.
    /// </summary>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routeMatrix"/> is <c>null</c>.</exception>
    public ProblemBuilder WithRouteMatrix(DirectedRouteMatrix routeMatrix)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));

        _routeMatrix = routeMatrix;
        return this;
    }

    /// <summary>
    /// Sets the route matrix.
    /// </summary>
    /// <param name="factory">The factory that creates the route matrix.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <c>null</c>.</exception>
    public ProblemBuilder WithRouteMatrix(Func<DirectedRouteMatrixBuilder, DirectedRouteMatrix> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var builder = new DirectedRouteMatrixBuilder();
        var routeMatrix = factory(builder);
        return WithRouteMatrix(routeMatrix);
    }

    /// <summary>
    /// Sets the route matrix.
    /// </summary>
    /// <param name="factory">The factory that creates the route matrix.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <c>null</c>.</exception>
    public ProblemBuilder WithRouteMatrix(Func<DirectedRouteMatrix> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        var routeMatrix = factory();
        return WithRouteMatrix(routeMatrix);
    }

    /// <summary>
    /// Sets the maximum vehicle waiting time.
    /// </summary>
    /// <param name="maxVehicleWaitingTime">The maximum vehicle waiting time.</param>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    /// <remarks>
    /// The maximum vehicle waiting time must be greater than or equal to zero.
    /// If a value less than zero is provided, the maximum vehicle waiting time will be set to zero.
    /// </remarks>
    public ProblemBuilder WithMaxVehicleWaitingTime(long maxVehicleWaitingTime)
    {
        _maxVehicleWaitingTime = Math.Max(0, maxVehicleWaitingTime);
        return this;
    }

    /// <summary>
    /// Removes the maximum vehicle waiting time.
    /// </summary>
    /// <returns>The current instance of the <see cref="ProblemBuilder"/> class.</returns>
    public ProblemBuilder WithoutMaxVehicleWaitingTime()
    {
        _maxVehicleWaitingTime = null;
        return this;
    }

    /// <summary>
    /// Builds the problem.
    /// </summary>
    /// <returns>The problem.</returns>
    public Problem Build()
    {
        var matrix = _routeMatrix ?? new DirectedRouteMatrix();
        return new Problem(_locations, _vehicles, _shipments, matrix, _maxVehicleWaitingTime);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return "ProblemBuilder";
    }
}
