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
    /// Gets the callback.
    /// </summary>
    /// <returns>The callback of the dimension.</returns>
    SolverCallback GetCallback();
    
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <returns>The name of the dimension.</returns>
    string GetName();
    
    /// <summary>
    /// Gets the maximum slack.
    /// </summary>
    /// <returns>The maximum slack of the dimension.</returns>
    long GetMaxSlack();
    
    /// <summary>
    /// Flag indicating whether the dimension should start at zero.
    /// </summary>
    /// <returns><see langword="true"/> if the dimension should start at zero; otherwise, <see langword="false"/>.</returns>
    bool ShouldStartAtZero();
}