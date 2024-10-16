// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Output;

/// <summary>
/// Factory for creating solver outputs.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
internal interface IOutputFactory<TKey>
    where TKey : notnull
{
    /// <summary>
    /// Creates a new solver output.
    /// </summary>
    /// <param name="problem">The problem.</param>
    /// <param name="state">The state.</param>
    /// <param name="assignment">The assignment.</param>
    SolverOutput CreateOutput(Problem problem, SolverState<TKey> state, Assignment? assignment);
}