// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Represents a dimension with a single capacity.
/// </summary>
internal interface ISingleCapacityDimension : IDimension
{
    /// <summary>
    /// The capacity of the dimension.
    /// </summary>
    long Capacity();
}