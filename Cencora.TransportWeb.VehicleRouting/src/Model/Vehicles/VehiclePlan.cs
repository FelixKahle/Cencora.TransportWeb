// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Represents a plan of a vehicle.
/// </summary>
public sealed class VehiclePlan
{
    /// <summary>
    /// Gets the vehicle of the plan.
    /// </summary>
    public Vehicle Vehicle { get; }

    /// <summary>
    /// Gets the stops of the plan.
    /// </summary>
    public IReadOnlyList<VehicleStop> Stops { get; }

    /// <summary>
    /// Gets the trips of the plan.
    /// </summary>
    public IReadOnlyList<VehicleTrip> Trips { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehiclePlan"/> class.
    /// </summary>
    /// <param name="vehicle">The vehicle of the plan.</param>
    /// <param name="stops">The stops of the plan.</param>
    /// <param name="trips">The trips of the plan.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/>, <paramref name="stops"/> or <paramref name="trips"/> is <see langword="null"/>.</exception>
    public VehiclePlan(Vehicle vehicle, IReadOnlyList<VehicleStop> stops, IReadOnlyList<VehicleTrip> trips)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(stops, nameof(stops));
        ArgumentNullException.ThrowIfNull(trips, nameof(trips));

        Vehicle = vehicle;
        Stops = stops;
        Trips = trips;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Plan for {Vehicle.Id}";
    }
}
