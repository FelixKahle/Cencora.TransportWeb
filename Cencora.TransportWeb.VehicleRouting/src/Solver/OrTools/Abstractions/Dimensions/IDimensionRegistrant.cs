// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Registrant for dimensions.
/// </summary>
internal interface IDimensionRegistrant
{
    /// <summary>
    /// Registers a single capacity dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <returns><langword>true</langword> if the dimension was registered successfully; otherwise, <langword>false</langword>.</returns>
    bool RegisterDimension(ISingleCapacityDimension dimension);
    
    /// <summary>
    /// Registers a multi capacity dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <returns><langword>true</langword> if the dimension was registered successfully; otherwise, <langword>false</langword>.</returns>
    bool RegisterDimension(IMultiCapacityDimension dimension);
}