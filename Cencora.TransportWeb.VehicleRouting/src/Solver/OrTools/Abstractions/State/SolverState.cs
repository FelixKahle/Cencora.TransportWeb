// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// The state of the solver.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
internal readonly struct SolverState<TKey> : IDisposable
    where TKey : notnull
{
    /// <summary>
    /// The solver model.
    /// </summary>
    internal SolverModel SolverModel { get; }
    
    /// <summary>
    /// The solver interface.
    /// </summary>
    internal SolverInterface<TKey> SolverInterface { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverState{T}"/> struct.
    /// </summary>
    /// <param name="factory">The solver model factory.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is <see langword="null"/>.</exception>
    internal SolverState(ISimpleSolverModelFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        SolverModel = factory.Create();
        SolverInterface = new SolverInterface<TKey>(SolverModel);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverState{T}"/> struct.
    /// </summary>
    /// <param name="factory">The solver model factory.</param>
    /// <param name="problem">The problem.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> or <paramref name="problem"/> is <see langword="null"/>.</exception>
    internal SolverState(ISolverModelFactory factory, Problem problem)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));

        SolverModel = factory.Create(problem);
        SolverInterface = new SolverInterface<TKey>(SolverModel);
    }
    
    /// <summary>
    /// Solves the internal model.
    /// </summary>
    /// <param name="timeLimit">The time limit.</param>
    /// <returns>The internal assignment.</returns>
    internal Assignment? Solve(TimeSpan timeLimit)
    {
        return SolverInterface.Solve(timeLimit);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        SolverInterface.Dispose();
    }
}