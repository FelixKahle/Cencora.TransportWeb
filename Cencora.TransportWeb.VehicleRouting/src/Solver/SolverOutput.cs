// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;

namespace Cencora.TransportWeb.VehicleRouting.Solver;

/// <summary>
/// Represents the output of a solver.
/// </summary>
public sealed class SolverOutput
{
    /// <summary>
    /// Gets the solution of the solver.
    /// </summary>
    /// <remarks>
    /// This property is <see langword="null"/> if the solver did not find a solution.
    /// </remarks>
    public Solution? Solution { get; }

    /// <summary>
    /// Gets a value indicating whether the solver has found a solution.
    /// </summary>
    public bool HasSolution => Solution is not null;

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverOutput"/> class.
    /// </summary>
    public SolverOutput()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverOutput"/> class.
    /// </summary>
    /// <param name="solution">The solution of the solver.</param>
    public SolverOutput(Solution? solution)
    {
        Solution = solution;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return HasSolution ? "GoogleOrToolsSolver output with solution" : "GoogleOrToolsSolver output without solution";
    }
}
