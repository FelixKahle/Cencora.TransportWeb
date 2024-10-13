// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Implementation;

/// <summary>
/// Default implementation of the <see cref="ICallbackRegistrant"/> interface.
/// </summary>
internal sealed class DefaultCallbackRegistrant : ICallbackRegistrant
{
    /// <summary>
    /// The state.
    /// </summary>
    private readonly SolverState _state;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCallbackRegistrant"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    internal DefaultCallbackRegistrant(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        _state = state;
    }
    
    /// <inheritdoc/>
    public int RegisterCallback(ITransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        return _state.Model.RegisterTransitCallback((from, to) =>
        {
            var fromNodeIndex = _state.IndexManager.IndexToNode(from);
            var toNodeIndex = _state.IndexManager.IndexToNode(to);
            var fromNode = _state.Nodes[fromNodeIndex];
            var toNode = _state.Nodes[toNodeIndex];

            var value = callback.Callback(fromNode, toNode);
            return value;
        });
    }

    /// <inheritdoc/>
    public int RegisterCallback(IUnaryTransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        return _state.Model.RegisterUnaryTransitCallback((from) =>
        {
            var fromNodeIndex = _state.IndexManager.IndexToNode(from);
            var fromNode = _state.Nodes[fromNodeIndex];

            var value = callback.Callback(fromNode);
            return value;
        });
    }
}