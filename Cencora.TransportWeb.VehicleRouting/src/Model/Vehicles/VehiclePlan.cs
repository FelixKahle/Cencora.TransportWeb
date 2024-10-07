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
    /// Gets the shifts of the plan.
    /// </summary>
    public IReadOnlyList<VehicleShift> Shifts { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="VehiclePlan"/> class.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="shifts">The shifts.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> or <paramref name="shifts"/> is <see langword="null"/>.</exception>
    public VehiclePlan(Vehicle vehicle, IReadOnlyList<VehicleShift> shifts)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(shifts, nameof(shifts));

        Vehicle = vehicle;
        Shifts = shifts;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Plan for {Vehicle.Id}";
    }
}
