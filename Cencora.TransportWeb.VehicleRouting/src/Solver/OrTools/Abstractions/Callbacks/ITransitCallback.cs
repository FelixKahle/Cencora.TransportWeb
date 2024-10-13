// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Interface for a transit callback.
/// </summary>
internal interface ITransitCallback
{
    /// <summary>
    /// Callback for the transit.
    /// </summary>
    /// <param name="fromNode">The node from which the transit starts.</param>
    /// <param name="toNode">The node to which the transit goes.</param>
    /// <returns>The transit cost.</returns>
    long Callback(Node fromNode, Node toNode);
}