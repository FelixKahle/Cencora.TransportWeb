// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Interface for registering callbacks.
/// </summary>
internal interface ICallbackRegistrant
{
    /// <summary>
    /// Register a callback.
    /// </summary>
    /// <param name="callback">The callback to register.</param>
    /// <returns>The callback.</returns>
    Callback RegisterCallback(Func<Node, Node, long> callback);
    
    /// <summary>
    /// Register a callback.
    /// </summary>
    /// <param name="callback">The callback to register.</param>
    /// <returns>The callback.</returns>
    Callback RegisterCallback(Func<Node, long> callback);
    
    /// <summary>
    /// Register a callback.
    /// </summary>
    /// <param name="transitCallback">The callback to register.</param>
    /// <returns>The callback.</returns>
    Callback RegisterCallback(ITransitCallback transitCallback);
    
    /// <summary>
    /// Register a callback.
    /// </summary>
    /// <param name="unaryTransitCallback">The callback to register.</param>
    /// <returns>The callback.</returns>
    Callback RegisterCallback(IUnaryTransitCallback unaryTransitCallback);
}