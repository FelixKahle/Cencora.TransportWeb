// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Model;

/// <summary>
/// Represents a solution of the vehicle routing problem.
/// </summary>
public sealed class Solution
{
    /// <summary>
    /// Gets the vehicle plans of the solution.
    /// </summary>
    public IReadOnlySet<VehiclePlan> VehiclePlans { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Solution"/> class.
    /// </summary>
    /// <param name="vehiclePlans">The vehicle plans of the solution.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehiclePlans"/> is <see langword="null"/>.</exception>
    public Solution(IReadOnlySet<VehiclePlan> vehiclePlans)
    {
        ArgumentNullException.ThrowIfNull(vehiclePlans, nameof(vehiclePlans));

        VehiclePlans = vehiclePlans;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Solution with {VehiclePlans.Count} vehicle plans";
    }
}
