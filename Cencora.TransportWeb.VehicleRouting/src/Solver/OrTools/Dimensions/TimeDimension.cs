// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

/// <summary>
/// The time dimension.
/// </summary>
internal sealed class TimeDimension : MultiCapacityDimension
{
    /// <summary>
    /// The time dimension name.
    /// </summary>
    internal const string TimeDimensionName = "TimeDimension";
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeDimension"/> class.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <param name="registrant">The dimension registrant.</param>
    /// <param name="vehicles">The vehicles.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is null.</exception>
    internal TimeDimension(ICallback callback, IDimensionRegistrant registrant, IReadOnlyList<DummyVehicle> vehicles) 
        : base(callback, TimeDimensionName, long.MaxValue, GetVehicleCapacities(vehicles), false, registrant)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeDimension"/> class.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <param name="dimensionName">The dimension name.</param>
    /// <param name="registrant">The dimension registrant.</param>
    /// <param name="vehicles">The vehicles.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is null.</exception>
    internal TimeDimension(ICallback callback, string dimensionName, IDimensionRegistrant registrant, IReadOnlyList<DummyVehicle> vehicles) 
        : base(callback, dimensionName, long.MaxValue, GetVehicleCapacities(vehicles), false, registrant)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));
    }
    
    /// <summary>
    /// Computes the vehicle capacities.
    /// </summary>
    /// <param name="vehicles">The vehicles.</param>
    /// <returns>The vehicle capacities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is null.</exception>
    private static long[] GetVehicleCapacities(IReadOnlyList<DummyVehicle> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));
        
        return vehicles.Select(vehicle => vehicle.MaxDuration).ToArray();
    }
}