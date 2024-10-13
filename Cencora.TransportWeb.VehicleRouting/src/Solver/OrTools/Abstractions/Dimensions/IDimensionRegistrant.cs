// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Google.OrTools.ConstraintSolver;

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
    /// <returns>The routing dimension.</returns>
    RoutingDimension RegisterDimension(ISingleCapacityDimension dimension);
    
    /// <summary>
    /// Registers a multi capacity dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The routing dimension.</returns>
    RoutingDimension RegisterDimension(IMultiCapacityDimension dimension);
    
    /// <summary>
    /// Gets the number of registered dimensions.
    /// </summary>
    int GetDimensionCount();
}