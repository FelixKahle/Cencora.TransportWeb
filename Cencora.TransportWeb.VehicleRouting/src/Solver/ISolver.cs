// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;

namespace Cencora.TransportWeb.VehicleRouting.Solver;

/// <summary>
/// Represents a solver for the vehicle routing problem.
/// </summary>
public interface ISolver
{
    /// <summary>
    /// Solves the vehicle routing problem.
    /// </summary>
    /// <param name="problem">The vehicle routing problem to solve.</param>
    /// <returns>The output of the solver.</returns>
    public SolverOutput Solve(Problem problem);
}
