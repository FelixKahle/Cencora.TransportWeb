// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Evaluates the cost of an arc.
/// </summary>
internal interface IArcCostEvaluator : ICallback
{
    /// <summary>
    /// Gets the cost of the arc between the specified nodes for the specified vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="fromNode">The from node.</param>
    /// <param name="toNode">The to node.</param>
    /// <returns>The cost of the arc between the specified nodes for the specified vehicle.</returns>
    long GetCost(DummyVehicle vehicle, Node fromNode, Node toNode);
}