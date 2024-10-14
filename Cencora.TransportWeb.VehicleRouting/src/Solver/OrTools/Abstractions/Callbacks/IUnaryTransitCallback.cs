// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Interface for a unary transit callback.
/// </summary>
internal interface IUnaryTransitCallback : ICallback
{
    /// <summary>
    /// Gets the transit for the specified node.
    /// </summary>
    long GetTransit(Node node);
}