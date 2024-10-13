// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Base class for solver configurators.
/// </summary>
internal abstract class SolverConfiguratorBase : ISolverConfigurator
{
    /// <summary>
    /// The index manager.
    /// </summary>
    private protected RoutingIndexManager IndexManager { get; }
    
    /// <summary>
    /// The routing model.
    /// </summary>
    private protected RoutingModel Model { get; }
    
    /// <summary>
    /// The solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the solver is not initialized.</exception>
    private protected Google.OrTools.ConstraintSolver.Solver Solver => Model.solver() ?? throw new InvalidOperationException("The solver is not initialized.");
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverConfiguratorBase"/> class.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    internal SolverConfiguratorBase(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        IndexManager = state.IndexManager;
        Model = state.Model;
    }
    
    /// <inheritdoc/>
    public abstract void Configure(SolverState state);
}