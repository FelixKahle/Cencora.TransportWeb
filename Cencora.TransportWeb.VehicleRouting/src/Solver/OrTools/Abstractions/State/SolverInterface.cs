// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// Represents the internal interface of the solver.
/// </summary>
internal sealed class SolverInterface : IDisposable
{
    /// <summary>
    /// Gets the index manager of the solver.
    /// </summary>
    internal RoutingIndexManager IndexManager { get; }
    
    /// <summary>
    /// Gets the model of the solver.
    /// </summary>
    internal RoutingModel RoutingModel { get; }
    
    /// <summary>
    /// The model of the solver.
    /// </summary>
    internal SolverModel SolverModel { get; }

    /// <summary>
    /// The callback registrant of the solver.
    /// </summary>
    internal ICallbackRegistrant CallbackRegistrant { get; }
    
    /// <summary>
    /// The dimension registrant of the solver.
    /// </summary>
    internal IDimensionRegistrant DimensionRegistrant { get; }

    /// <summary>
    /// The solver of the model.
    /// </summary>
    internal Google.OrTools.ConstraintSolver.Solver Solver => RoutingModel.solver() ?? throw new VehicleRoutingSolverException("Failed to get solver.");

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverInterface"/> class.
    /// </summary>
    /// <param name="model">The model of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <see langword="null"/>.</exception>
    internal SolverInterface(SolverModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        
        // Compute the vehicle node indices.
        var vehicleNodeIndices = new VehicleNodeIndices(model.Vehicles, model.VehicleNodeStores);
        
        Console.WriteLine("VehicleNodeIndices: " + string.Join(", ", vehicleNodeIndices.StartNodeIndices));
        
        // Assign the values to the properties.
        SolverModel = model;
        IndexManager = new RoutingIndexManager(model.NodeCount, model.VehicleCount, vehicleNodeIndices.StartNodeIndices, vehicleNodeIndices.EndNodeIndices);
        RoutingModel = new RoutingModel(IndexManager);
        CallbackRegistrant = new DefaultCallbackRegistrant(model, IndexManager, RoutingModel);
        DimensionRegistrant = new DefaultDimensionRegistrant(model, RoutingModel);
    }

    /// <summary>
    /// Gets the node index from the given index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The node index.</returns>
    internal int IndexToNodeIndex(long index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        
        return IndexManager.IndexToNode(index);
    }
    
    /// <summary>
    /// Gets the node from the given index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The node.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative.</exception>
    internal Node IndexToNode(long index)
    {
        return SolverModel.Nodes[IndexToNodeIndex(index)];
    }
    
    /// <summary>
    /// Gets the index from the given node.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>The index.</returns>
    internal long NodeToIndex(Node node)
    {
        ArgumentNullException.ThrowIfNull(node, nameof(node));
        
        return IndexManager.NodeToIndex(node.Index);
    }
    
    /// <summary>
    /// Registers the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns>The internal solver callback.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is <see langword="null"/>.</exception>
    internal SolverCallback RegisterCallback(ITransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        return CallbackRegistrant.RegisterCallback(callback);
    }
    
    /// <summary>
    /// Registers the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns>The internal solver callback.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is <see langword="null"/>.</exception>
    internal SolverCallback RegisterCallback(IUnaryTransitCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        return CallbackRegistrant.RegisterCallback(callback);
    }
    
    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The internal solver dimension.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension"/> is <see langword="null"/>.</exception>
    internal SolverDimension RegisterDimension(ISingleCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        
        return DimensionRegistrant.RegisterDimension(dimension);
    }
    
    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The internal solver dimension.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension"/> is <see langword="null"/>.</exception>
    internal SolverDimension RegisterDimension(IMultiCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        
        return DimensionRegistrant.RegisterDimension(dimension);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        IndexManager.Dispose();
        RoutingModel.Dispose();
    }
}