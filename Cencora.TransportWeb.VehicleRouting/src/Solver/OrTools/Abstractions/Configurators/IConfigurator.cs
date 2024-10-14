// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;

/// <summary>
/// Interface for a configurator.
/// </summary>
internal interface IConfigurator
{
    /// <summary>
    /// Configures the specified state.
    /// </summary>
    void Configure(SolverState state);
}