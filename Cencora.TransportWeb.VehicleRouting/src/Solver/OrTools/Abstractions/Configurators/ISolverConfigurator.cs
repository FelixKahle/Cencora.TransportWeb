// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;

/// <summary>
/// Interface for configurators that configure the solver.
/// </summary>
internal interface ISolverConfigurator
{
    /// <summary>
    /// Configures the solver.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    void Configure(SolverState state);
}