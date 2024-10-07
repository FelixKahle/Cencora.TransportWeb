// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// Shift for a vehicle.
/// </summary>
public class VehicleShift
{
    /// <summary>
    /// Gets the vehicle of the vehicle shift.
    /// </summary>
    public Vehicle Vehicle { get; }
    
    /// <summary>
    /// Gets the shift of the vehicle shift.
    /// </summary>
    public Shift Shift { get;  }
    
    /// <summary>
    /// Gets the stops of the vehicle shift.
    /// </summary>
    public IReadOnlyList<VehicleStop> Stops { get; }
    
    /// <summary>
    /// Gets the trips of the vehicle shift.
    /// </summary>
    public IReadOnlyList<VehicleTrip> Trips { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleShift"/> class.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="shift">The shift.</param>
    /// <param name="stops">The stops.</param>
    /// <param name="trips">The trips.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/>, <paramref name="shift"/>, <paramref name="stops"/>, or <paramref name="trips"/> is <see langword="null"/>.</exception>
    public VehicleShift(Vehicle vehicle, Shift shift, IReadOnlyList<VehicleStop> stops, IReadOnlyList<VehicleTrip> trips)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(shift, nameof(shift));
        ArgumentNullException.ThrowIfNull(stops, nameof(stops));
        ArgumentNullException.ThrowIfNull(trips, nameof(trips));

        Vehicle = vehicle;
        Shift = shift;
        Stops = stops;
        Trips = trips;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Shift for {Vehicle}";
    }
}