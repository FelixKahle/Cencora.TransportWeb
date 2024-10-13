// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Registrant for callbacks.
/// </summary>
internal interface ICallbackRegistrant
{
     /// <summary>
     /// Registers a transit callback.
     /// </summary>
     /// <param name="callback">The callback.</param>
     /// <returns>The index of the callback.</returns>
    int RegisterCallback(ITransitCallback callback);
     
    /// <summary>
    /// Registers a unary transit callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns>The index of the callback.</returns> 
    int RegisterCallback(IUnaryTransitCallback callback);
    
    /// <summary>
    /// Gets the number of registered callbacks.
    /// </summary>
    int GetCallbackCount();
}