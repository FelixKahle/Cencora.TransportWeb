// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Registers callbacks with the solver.
/// </summary>
internal sealed class DefaultCallbackRegistrant : ICallbackRegistrant
{
    private readonly SolverModel _model;
    private readonly RoutingIndexManager _indexManager;
    private readonly RoutingModel _routingModel;
    
    // We need to keep track of the callbacks to prevent
    // them from being garbage collected.
    private readonly List<SolverCallback> _callbacks = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCallbackRegistrant"/> class.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="indexManager">The index manager.</param>
    /// <param name="routingModel">The routing model.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="model"/>, <paramref name="indexManager"/>, or <paramref name="routingModel"/> is <see langword="null"/>.</exception>
    public DefaultCallbackRegistrant(SolverModel model, RoutingIndexManager indexManager, RoutingModel routingModel)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        ArgumentNullException.ThrowIfNull(indexManager, nameof(indexManager));
        ArgumentNullException.ThrowIfNull(routingModel, nameof(routingModel));

        _model = model;
        _indexManager = indexManager;
        _routingModel = routingModel;
    }

    /// <inheritdoc/>
    public SolverCallback RegisterCallback(ITransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));

        var createdCallbackIndex = _routingModel.RegisterTransitCallback((fromIndex, toIndex) =>
        {
            var fromNode = _indexManager.IndexToNode(_model, fromIndex);
            var toNode = _indexManager.IndexToNode(_model, toIndex);

            return callback.GetTransit(fromNode, toNode);
        });

        var callbackInstance = new SolverCallback(callback, createdCallbackIndex);
        _callbacks.Add(callbackInstance);
        return callbackInstance;
    }

    /// <inheritdoc/>
    public SolverCallback RegisterCallback(IUnaryTransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));

        var createdCallbackIndex = _routingModel.RegisterUnaryTransitCallback(index =>
        {
            var node = _indexManager.IndexToNode(_model, index);

            return callback.GetTransit(node);
        });
        
        var callbackInstance = new SolverCallback(callback, createdCallbackIndex);
        _callbacks.Add(callbackInstance);
        return callbackInstance;
    }
    
    /// <inheritdoc/>
    public int CallbackCount => _callbacks.Count;

    /// <inheritdoc/>
    public void Dispose()
    {
        _callbacks.Clear();
        _callbacks.TrimExcess();
        
        // Routing index manager and routing model are managed by the solver interface
        // and should not be disposed here.
    }
}
