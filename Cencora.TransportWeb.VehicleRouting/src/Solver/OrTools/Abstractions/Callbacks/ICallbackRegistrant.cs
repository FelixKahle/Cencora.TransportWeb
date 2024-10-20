// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Interface for a callback registrant.
/// </summary>
internal interface ICallbackRegistrant : IDisposable
{
    /// <summary>
    /// Registers the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    SolverCallback RegisterCallback(ITransitCallback callback);
    
    /// <summary>
    /// Registers the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    SolverCallback RegisterCallback(IUnaryTransitCallback callback);
    
    /// <summary>
    /// Gets the number of registered callbacks.
    /// </summary>
    int CallbackCount { get; }
}