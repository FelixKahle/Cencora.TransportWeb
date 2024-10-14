// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Implementation;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Solver using Google OR-Tools.
/// </summary>
public class OrToolsSolver : ISolver
{
    /// <summary>
    /// The model factory.
    /// </summary>
    private readonly ISolverModelFactory _solverModelFactory = new DefaultSolverModelFactory();
    
    /// <summary>
    /// The solver state.
    /// </summary>
    private SolverState? _solverState;
    
    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
        var model = _solverModelFactory.Create(problem);
        _solverState = new SolverState(model);

        return new SolverOutput();
    }
}