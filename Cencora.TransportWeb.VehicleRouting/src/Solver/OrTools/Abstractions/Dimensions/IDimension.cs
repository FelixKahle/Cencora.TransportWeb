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
    /// Gets the callback of the dimension.
    /// </summary>
    ICallback GetCallback();
    
    /// <summary>
    /// Gets the name of the dimension.
    /// </summary>
    /// <returns>The name of the dimension.</returns>
    string GetDimensionName();
    
    /// <summary>
    /// Gets the maximum slack of the dimension.
    /// </summary>
    long GetMaxSlackValue();
    
    /// <summary>
    /// Gets a flag indicating whether the dimension needs to start from zero.
    /// </summary>
    bool ShouldStartAtZero();
}