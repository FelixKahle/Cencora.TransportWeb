// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Represents a dimension.
/// </summary>
internal interface IDimension
{
    /// <summary>
    /// The callback of the dimension.
    /// </summary>
    ICallback Callback { get; }
    
    /// <summary>
    /// The name of the dimension.
    /// </summary>
    string Name();
    
    /// <summary>
    /// The max slack of the dimension.
    /// </summary>
    long MaxSlack();
    
    /// <summary>
    /// True if the dimension should start at zero.
    /// </summary>
    bool ShouldStartAtZero();
}