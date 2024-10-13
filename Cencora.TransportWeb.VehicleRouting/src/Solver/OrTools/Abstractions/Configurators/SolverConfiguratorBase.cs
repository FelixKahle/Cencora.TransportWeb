// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;

/// <summary>
/// The solver configurator base class.
/// </summary>
internal abstract class SolverConfiguratorBase : ISolverConfigurator
{
    /// <summary>
    /// The solver state.
    /// </summary>
    internal SolverState State { get; }
    
    /// <summary>
    /// The callback registrant.
    /// </summary>
    internal ICallbackRegistrant CallbackRegistrant { get; }
    
    /// <summary>
    /// The dimension registrant.
    /// </summary>
    internal IDimensionRegistrant DimensionRegistrant { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverConfiguratorBase"/> class.
    /// </summary>
    /// <param name="state">The solver state.</param>
    /// <param name="callbackRegistrant">The callback registrant.</param>
    /// <param name="dimensionRegistrant">The dimension registrant.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is null.</exception>
    protected SolverConfiguratorBase(SolverState state, ICallbackRegistrant callbackRegistrant,
        IDimensionRegistrant dimensionRegistrant)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(callbackRegistrant, nameof(callbackRegistrant));
        ArgumentNullException.ThrowIfNull(dimensionRegistrant, nameof(dimensionRegistrant));
        
        State = state;
        CallbackRegistrant = callbackRegistrant;
        DimensionRegistrant = dimensionRegistrant;
    }
    
    /// <inheritdoc/>
    public abstract void Configure(SolverState state, ICallbackRegistrant callbackRegistrant,
        IDimensionRegistrant dimensionRegistrant);
}