// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Represents a dimension with multiple capacities.
/// </summary>
internal interface IMultiCapacityDimension : IDimension
{
    /// <summary>
    /// Gets the capacities of the dimension.
    /// </summary>
    /// <returns>The capacities of the dimension.</returns>
    long[] GetCapacities();
}