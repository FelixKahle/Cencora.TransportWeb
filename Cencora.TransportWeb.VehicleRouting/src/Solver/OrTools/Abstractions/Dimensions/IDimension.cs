// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Interface for a dimension.
/// </summary>
internal interface IDimension
{
    /// <summary>
    /// The callback of the dimension.
    /// </summary>
    ICallback GetCallback();
    
    /// <summary>
    /// Gets the maximum slack of the dimension.
    /// </summary>
    long GetMaxSlack();
    
    /// <summary>
    /// Flag indicating whether the dimension should start at zero.
    /// </summary>
    bool ShouldStartAtZero();
}