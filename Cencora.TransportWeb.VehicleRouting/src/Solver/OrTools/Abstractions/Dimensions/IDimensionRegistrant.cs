// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Represents a dimension registrant.
/// </summary>
internal interface IDimensionRegistrant
{
    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The registered dimension.</returns>
    SolverDimension RegisterDimension(ISingleCapacityDimension dimension);
    
    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The registered dimension.</returns>
    SolverDimension RegisterDimension(IMultiCapacityDimension dimension);
}