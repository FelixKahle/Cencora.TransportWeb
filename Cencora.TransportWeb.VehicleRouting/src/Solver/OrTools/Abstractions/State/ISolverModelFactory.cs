// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// Interface a factory for creating solver models.
/// </summary>
internal interface ISolverModelFactory
{
    /// <summary>
    /// Creates a new solver model.
    /// </summary>
    /// <param name="problem">The problem to create the model for.</param>
    /// <returns>The created model.</returns>
    SolverModel Create(Problem problem);
}