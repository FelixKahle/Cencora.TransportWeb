// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Interface for a transit callback.
/// </summary>
internal interface ITransitCallback
{
    /// <summary>
    /// The callback.
    /// </summary>
    /// <param name="from">The node to start from.</param>
    /// <param name="to">The node to end at.</param>
    /// <returns>The transit time between the two nodes.</returns>
    long Callback(Node from, Node to);
}