// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Configurator for callbacks.
/// </summary>
internal interface ICallbackConfigurator
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
}