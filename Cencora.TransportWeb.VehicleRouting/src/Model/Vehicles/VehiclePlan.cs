// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text;

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

    /// <summary>
    /// Returns a debug string of the vehicle plan.
    /// </summary>
    /// <returns>The debug string.</returns>
    public string ToDebugString()
    {
        const string tab = "    ";
    
        var sb = new StringBuilder();
        sb.AppendLine($"- Plan for {Vehicle.Id}");
    
        foreach (var shift in Shifts)
        {
            sb.AppendLine($"{tab}- Shift {shift.ShiftTimeWindow}");
        
            var trips = shift.Trips;
            var stops = shift.Stops;
        
            for (var i = 0; i < stops.Count; i++)
            {
                sb.AppendLine($"{tab}{tab}- Stop: {stops[i]}");

                // Append trip information if there is a next stop
                if (i < trips.Count)
                {
                    sb.AppendLine($"{tab}{tab}- Trip: {trips[i]}");
                }
            }
        }
    
        return sb.ToString();
    }
}
