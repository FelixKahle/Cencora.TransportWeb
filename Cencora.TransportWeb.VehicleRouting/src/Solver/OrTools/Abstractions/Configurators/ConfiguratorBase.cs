// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;

/// <summary>
/// Base class for configurators.
/// </summary>
internal abstract class ConfiguratorBase : IConfigurator
{
    /// <summary>
    /// The state.
    /// </summary>
    private protected SolverState State { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfiguratorBase"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    private protected ConfiguratorBase(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        State = state;
    }

    /// <inheritdoc/>
    public abstract void Configure(SolverState state);
}