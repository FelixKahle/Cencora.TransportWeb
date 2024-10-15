// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

/// <summary>
/// Enumerates all dimensions used in this vehicle routing problem.
/// </summary>
internal enum Dimension
{
    /// <summary>
    /// The time dimension.
    /// </summary>
    TimeDimension,
    
    /// <summary>
    /// The node dimension used to keep track of the order of the nodes.
    /// </summary>
    NodeDimension,
}