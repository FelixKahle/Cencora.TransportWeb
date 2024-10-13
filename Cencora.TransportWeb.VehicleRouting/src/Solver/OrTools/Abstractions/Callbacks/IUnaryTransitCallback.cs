// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Interface for a unary transit callback.
/// </summary>
internal interface IUnaryTransitCallback
{
    /// <summary>
    /// Callback for the transit.
    /// </summary>
    /// <param name="node">The node to which the transit goes.</param>
    /// <returns>The transit cost.</returns>
    long Callback(Node node);
}