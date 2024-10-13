// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Interface for a single dimension.
/// </summary>
/// <remarks>
/// This dimension can carry a single capacity.
/// </remarks>
internal interface ISingleCapacityDimension : IDimension
{
    /// <summary>
    /// Gets the maximum capacity of the dimension.
    /// </summary>
    long GetCapacity();
}