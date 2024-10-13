// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Default implementation of the <see cref="ICallbackRegistrant"/> interface.
/// </summary>
internal sealed class DefaultCallbackRegistrant : ICallbackRegistrant
{
    /// <summary>
    /// The solver interface.
    /// </summary>
    private readonly SolverInterface _solverInterface;
    
    /// <summary>
    /// The state.
    /// </summary>
    private readonly SolverState _state;

    /// <summary>
    /// The callbacks.
    /// </summary>
    private readonly List<ICallback> _callbacks;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCallbackRegistrant"/> class.
    /// </summary>
    /// <param name="solverInterface">The solver interface.</param>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="solverInterface"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    internal DefaultCallbackRegistrant(SolverInterface solverInterface, SolverState state)
    {
        ArgumentNullException.ThrowIfNull(solverInterface, nameof(solverInterface));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        _solverInterface = solverInterface;
        _state = state;
        _callbacks = new List<ICallback>();
    }
    
    /// <inheritdoc/>
    public int RegisterCallback(ITransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        _callbacks.Add(callback);
        
        return _solverInterface.Model.RegisterTransitCallback((from, to) =>
        {
            var fromNodeIndex = _solverInterface.IndexManager.IndexToNode(from);
            var toNodeIndex = _solverInterface.IndexManager.IndexToNode(to);
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
        
        _callbacks.Add(callback);
        
        return _solverInterface.Model.RegisterUnaryTransitCallback((from) =>
        {
            var fromNodeIndex = _solverInterface.IndexManager.IndexToNode(from);
            var fromNode = _state.Nodes[fromNodeIndex];

            var value = callback.Callback(fromNode);
            return value;
        });
    }
    
    /// <inheritdoc/>
    public int GetCallbackCount()
    {
        return _callbacks.Count;
    }
}