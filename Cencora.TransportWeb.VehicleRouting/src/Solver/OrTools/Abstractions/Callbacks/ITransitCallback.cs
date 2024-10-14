// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Interface for a transit callback.
/// </summary>
internal interface ITransitCallback : ICallback
{
    /// <summary>
    /// Gets the transit between two nodes.
    /// </summary>
    long GetTransit(Node fromNode, Node toNode);
}