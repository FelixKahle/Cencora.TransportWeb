// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Interface for a callback.
/// </summary>
internal interface ICallback
{
    /// <summary>
    /// Gets the index of the callback.
    /// </summary>
    /// <returns>The index of the callback.</returns>
    int GetCallbackIndex();
}