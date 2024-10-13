// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Represents a unary transit callback.
/// </summary>
public interface IUnaryTransitCallback
{
    /// <summary>
    /// The callback.
    /// </summary>
    /// <param name="node">The node.</param>
    long Callback(Node node);
}