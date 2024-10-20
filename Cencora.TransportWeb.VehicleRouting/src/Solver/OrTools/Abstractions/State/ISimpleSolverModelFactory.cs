// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// Simple solver model factory.
/// </summary>
internal interface ISimpleSolverModelFactory
{
    /// <summary>
    /// Creates a new solver model.
    /// </summary>
    SolverModel Create();
}