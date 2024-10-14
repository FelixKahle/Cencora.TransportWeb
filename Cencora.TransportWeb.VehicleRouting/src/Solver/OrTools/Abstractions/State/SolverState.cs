// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// The state of the solver.
/// </summary>
internal readonly struct SolverState
{
    /// <summary>
    /// The solver model.
    /// </summary>
    public SolverModel SolverModel { get; }
    
    /// <summary>
    /// The solver interface.
    /// </summary>
    public SolverInterface SolverInterface { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverState"/> struct.
    /// </summary>
    /// <param name="solverModel">The solver model.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="solverModel"/> is <see langword="null"/>.</exception>
    public SolverState(SolverModel solverModel)
    {
        ArgumentNullException.ThrowIfNull(solverModel, nameof(solverModel));
        
        SolverModel = solverModel;
        SolverInterface = new SolverInterface(solverModel);
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverState"/> struct.
    /// </summary>
    /// <param name="solverModel">The solver model.</param>
    /// <param name="solverInterface">The solver interface.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="solverModel"/> is <see langword="null"/>.</exception>
    public SolverState(SolverModel solverModel, SolverInterface solverInterface)
    {
        ArgumentNullException.ThrowIfNull(solverModel, nameof(solverModel));
        ArgumentNullException.ThrowIfNull(solverInterface, nameof(solverInterface));
        
        SolverModel = solverModel;
        SolverInterface = solverInterface;
    }
}