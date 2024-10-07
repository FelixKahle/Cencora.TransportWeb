// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Model;

/// <summary>
/// Represents a vehicle routing problem.
/// </summary>
public sealed class Problem
{
    /// <summary>
    /// All locations that are part of the problem.
    /// </summary>
    public IReadOnlySet<Location> Locations { get; }

    /// <summary>
    /// Returns the number of locations of the problem.
    /// </summary>
    public int LocationCount => Locations.Count;

    /// <summary>
    /// All shipments that are part of the problem.
    /// </summary>
    public IReadOnlySet<Vehicle> Vehicles { get; }

    /// <summary>
    /// Returns the number of vehicles of the problem.
    /// </summary>
    public int VehicleCount => Vehicles.Count;

    /// <summary>
    /// All vehicles that are part of the problem.
    /// </summary>
    public IReadOnlySet<Shipment> Shipments { get; }

    /// <summary>
    /// Returns the number of shipments of the problem.
    /// </summary>
    public int ShipmentCount => Shipments.Count;

    /// <summary>
    /// The directed route matrix that contains the travel information between locations.
    /// </summary>
    public IReadOnlyDirectedRouteMatrix DirectedRouteMatrix { get; }

    /// <summary>
    /// The maximum time a vehicle is allowed to wait at a location.
    /// </summary>
    /// <remarks>
    /// This is basically some sort of idle time for the vehicle.
    /// If a vehicle is at a location and would arrive to early at the next location, it has to wait.
    /// This is the maximum time it is allowed to wait at a location.
    /// </remarks>
    public long? MaxVehicleWaitingTime { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="Problem"/> class.
    /// </summary>
    /// <param name="locations">All locations that are part of the problem.</param>
    /// <param name="vehicles">All vehicles that are part of the problem.</param>
    /// <param name="shipments">All shipments that are part of the problem.</param>
    /// <param name="matrix">The directed route matrix that contains the travel information between locations.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="locations"/>, <paramref name="vehicles"/>, <paramref name="shipments"/> or <paramref name="matrix"/> is <see langword="null"/>.</exception>
    public Problem(IReadOnlySet<Location> locations, IReadOnlySet<Vehicle> vehicles, IReadOnlySet<Shipment> shipments, IReadOnlyDirectedRouteMatrix matrix)
    {
        ArgumentNullException.ThrowIfNull(locations, nameof(locations));
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));
        ArgumentNullException.ThrowIfNull(shipments, nameof(shipments));
        ArgumentNullException.ThrowIfNull(matrix, nameof(matrix));

        Locations = locations;
        Vehicles = vehicles;
        Shipments = shipments;
        DirectedRouteMatrix = matrix;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Problem"/> class.
    /// </summary>
    /// <param name="locations">All locations that are part of the problem.</param>
    /// <param name="vehicles">All vehicles that are part of the problem.</param>
    /// <param name="shipments">All shipments that are part of the problem.</param>
    /// <param name="maxVehicleWaitingTime">The maximum time a vehicle is allowed to wait at a location.</param>
    /// <param name="matrix">The directed route matrix that contains the travel information between locations.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="locations"/>, <paramref name="vehicles"/>, <paramref name="shipments"/> or <paramref name="matrix"/> is <see langword="null"/>.</exception>
    public Problem(IReadOnlySet<Location> locations, IReadOnlySet<Vehicle> vehicles, IReadOnlySet<Shipment> shipments, IReadOnlyDirectedRouteMatrix matrix, long? maxVehicleWaitingTime)
    {
        ArgumentNullException.ThrowIfNull(locations, nameof(locations));
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));
        ArgumentNullException.ThrowIfNull(shipments, nameof(shipments));
        ArgumentNullException.ThrowIfNull(matrix, nameof(matrix));

        Locations = locations;
        Vehicles = vehicles;
        Shipments = shipments;
        DirectedRouteMatrix = matrix;

        // Make sure the max vehicle waiting time is not negative.
        MaxVehicleWaitingTime = maxVehicleWaitingTime is not null ? Math.Max(0, maxVehicleWaitingTime.Value) : null;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var maxWaitingTimeText = MaxVehicleWaitingTime.HasValue
            ? $"Max Vehicle Waiting Time: {MaxVehicleWaitingTime.Value}"
            : "Max Vehicle Waiting Time: None";

        return $"Problem with {Locations.Count} Locations, {Vehicles.Count} Vehicles, {Shipments.Count} Shipments. {maxWaitingTimeText}.";
    }
}
