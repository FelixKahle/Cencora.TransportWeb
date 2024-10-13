// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Default implementation of <see cref="ICallbackRegistrant"/>.
/// </summary>
internal sealed class DefaultCallbackRegistrant : ICallbackRegistrant
{
    private readonly SolverInterface _solverInterface;
    private readonly SolverModel _solverModel;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCallbackRegistrant"/> class.
    /// </summary>
    /// <param name="solverInterface">The solver interface.</param>
    /// <param name="solverModel">The solver model.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="solverInterface"/> is <see langword="null"/>.</exception>
    internal DefaultCallbackRegistrant(SolverInterface solverInterface, SolverModel solverModel)
    {
        ArgumentNullException.ThrowIfNull(solverInterface, nameof(solverInterface));
        ArgumentNullException.ThrowIfNull(solverModel, nameof(solverModel));
        
        _solverInterface = solverInterface;
        _solverModel = solverModel;
    }
    
    /// <inheritdoc/>
    public Callback RegisterCallback(Func<Node, Node, long> callback)
    {
        var index = _solverInterface.RegisterTransitCallback((fromIndex, toIndex) =>
        {
            var fromNodeIndex = _solverInterface.IndexToNode(fromIndex);
            var toNodeIndex = _solverInterface.IndexToNode(toIndex);
            
            var fromNode = _solverModel.Nodes[fromNodeIndex];
            var toNode = _solverModel.Nodes[toNodeIndex];
            
            return callback(fromNode, toNode);
        });
        
        return new Callback(index);
    }

    /// <inheritdoc/>
    public Callback RegisterCallback(Func<Node, long> callback)
    {
        var index = _solverInterface.RegisterTransitCallback((index) =>
        {
            var nodeIndex = _solverInterface.IndexToNode(index);
            var node = _solverModel.Nodes[nodeIndex];
            
            return callback(node);
        });
        
        return new Callback(index);
    }

    /// <inheritdoc/>
    public Callback RegisterCallback(ITransitCallback transitCallback)
    {
        var index = _solverInterface.RegisterTransitCallback((fromIndex, toIndex) =>
        {
            var fromNodeIndex = _solverInterface.IndexToNode(fromIndex);
            var toNodeIndex = _solverInterface.IndexToNode(toIndex);
            
            var fromNode = _solverModel.Nodes[fromNodeIndex];
            var toNode = _solverModel.Nodes[toNodeIndex];
            
            return transitCallback.Callback(fromNode, toNode);
        });
        
        return new Callback(index);
    }

    /// <inheritdoc/>
    public Callback RegisterCallback(IUnaryTransitCallback unaryTransitCallback)
    {
        var index = _solverInterface.RegisterTransitCallback((index) =>
        {
            var nodeIndex = _solverInterface.IndexToNode(index);
            var node = _solverModel.Nodes[nodeIndex];
            
            return unaryTransitCallback.Callback(node);
        });
        
        return new Callback(index);
    }
}