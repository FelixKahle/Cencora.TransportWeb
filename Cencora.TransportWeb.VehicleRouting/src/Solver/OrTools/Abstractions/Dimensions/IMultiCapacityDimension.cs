// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Interface for a vehicle capacity dimension.
/// </summary>
/// <remarks>
/// This dimension can carry multiple capacities.
/// </remarks>
internal interface IMultiCapacityDimension : IDimension
{
    /// <summary>
    /// Gets the maximum values of the dimension.
    /// </summary>
    /// <returns>The maximum values of the dimension.</returns>
    long[] GetCapacities();
}