// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;

/// <summary>
/// Base class for configurators.
/// </summary>
internal abstract class ConfiguratorBase<TKey> : IConfigurator<TKey>
    where TKey : notnull
{
    /// <summary>
    /// The state.
    /// </summary>
    private protected SolverState<TKey> State { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfiguratorBase{T}"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private protected ConfiguratorBase(SolverState<TKey> state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        State = state;
    }

    /// <inheritdoc/>
    public abstract void Configure(SolverState<TKey> state);
}