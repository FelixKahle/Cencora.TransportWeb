// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Interface for a single capacity dimension.
/// </summary>
internal interface ISingleCapacityDimension : IDimension
{
    /// <summary>
    /// Gets the capacity of the dimension.
    /// </summary>
    long GetCapacity();
}