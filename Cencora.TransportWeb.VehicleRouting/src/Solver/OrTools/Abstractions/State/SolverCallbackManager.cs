// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// Manages the registration of callbacks with the solver.
/// </summary>
internal sealed class SolverCallbackManager
{
    private readonly RoutingIndexManager _indexManager;
    private readonly RoutingModel _routingModel;
    private readonly SolverModel _solverModel;
    private readonly Dictionary<ICallback, SolverCallback> _callbacks = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverCallbackManager"/> class.
    /// </summary>
    /// <param name="solverModel">The solver model.</param>
    /// <param name="indexManager">The index manager.</param>
    /// <param name="routingModel">The routing model.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexManager"/>, <paramref name="routingModel"/>, or <paramref name="solverModel"/> is <see langword="null"/>.</exception>
    internal SolverCallbackManager(SolverModel solverModel, RoutingIndexManager indexManager, RoutingModel routingModel)
    {
        ArgumentNullException.ThrowIfNull(indexManager, nameof(indexManager));
        ArgumentNullException.ThrowIfNull(routingModel, nameof(routingModel));
        ArgumentNullException.ThrowIfNull(solverModel, nameof(solverModel));
        
        _indexManager = indexManager;
        _routingModel = routingModel;
        _solverModel = solverModel;
    }

    /// <summary>
    /// Determines whether the specified callback is registered.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns><see langword="true"/> if the specified callback is registered; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is <see langword="null"/>.</exception>
    internal bool IsCallbackRegistered(ICallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        return _callbacks.ContainsKey(callback);
    }

    /// <summary>
    /// Registers the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is <see langword="null"/>.</exception>
    internal void RegisterCallback(ITransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        if (IsCallbackRegistered(callback))
        {
            return;
        }

        var createdCallback = _routingModel.RegisterTransitCallback((fromIndex, toIndex) =>
        {
            var fromNode = _solverModel.Nodes[_indexManager.IndexToNode(fromIndex)];
            var toNode = _solverModel.Nodes[_indexManager.IndexToNode(toIndex)];
            return callback.Callback(fromNode, toNode);
        });

        _callbacks[callback] = createdCallback;
    }

    /// <summary>
    /// Registers the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is <see langword="null"/>.</exception>
    internal void RegisterCallback(IUnaryTransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        if (IsCallbackRegistered(callback))
        {
            return;
        }

        var createdCallback = _routingModel.RegisterUnaryTransitCallback((fromIndex) =>
        {
            var fromNode = _solverModel.Nodes[_indexManager.IndexToNode(fromIndex)];
            return callback.Callback(fromNode);
        });

        _callbacks[callback] = createdCallback;
    }

    /// <summary>
    /// Registers the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown if the specified callback type is not supported.</exception>
    internal void RegisterCallback(ICallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        if (IsCallbackRegistered(callback))
        {
            return;
        }
        
        switch (callback)
        {
            case ITransitCallback transitCallback:
                RegisterCallback(transitCallback);
                break;
            case IUnaryTransitCallback unaryTransitCallback:
                RegisterCallback(unaryTransitCallback);
                break;
            default:
                throw new ArgumentException("Unsupported callback type.", nameof(callback));
        }
    }
    
    /// <summary>
    /// Gets the solver callback for the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns>The solver callback.</returns>
    internal SolverCallback Get(ICallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        return _callbacks[callback];
    }

    /// <summary>
    /// The number of registered callbacks.
    /// </summary>
    public int CallbackCount => _callbacks.Count;
}