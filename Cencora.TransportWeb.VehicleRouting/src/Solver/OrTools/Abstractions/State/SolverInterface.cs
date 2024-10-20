// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// Represents the internal interface of the solver.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
internal sealed class SolverInterface<TKey> : IDisposable
    where TKey : notnull
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
    internal IDimensionRegistry<TKey> DimensionRegistry { get; }

    /// <summary>
    /// The solver of the model.
    /// </summary>
    internal Google.OrTools.ConstraintSolver.Solver Solver => RoutingModel.solver() ?? throw new VehicleRoutingSolverException("Failed to get solver.");

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverInterface{T}"/> class.
    /// </summary>
    /// <param name="model">The model of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <see langword="null"/>.</exception>
    internal SolverInterface(SolverModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        
        // Compute the vehicle node indices.
        var vehicleNodeIndices = new VehicleNodeIndices(model.Vehicles, model.VehicleNodeStores);
        
        // Assign the values to the properties.
        SolverModel = model;
        IndexManager = new RoutingIndexManager(model.NodeCount, model.VehicleCount, vehicleNodeIndices.StartNodeIndices, vehicleNodeIndices.EndNodeIndices);
        RoutingModel = new RoutingModel(IndexManager);
        CallbackRegistrant = new DefaultCallbackRegistrant(model, IndexManager, RoutingModel);
        DimensionRegistry = new DefaultDimensionRegistry<TKey>(model, RoutingModel);
    }

    /// <summary>
    /// Gets the node index from the given index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The node index.</returns>
    private int IndexToNodeIndex(long index)
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
    /// Sets the arc cost evaluator of the vehicle.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <param name="vehicleIndex">The vehicle index.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is <see langword="null"/>.</exception>
    internal void SetArcCostEvaluatorOfVehicle(SolverCallback callback, int vehicleIndex)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        ArgumentOutOfRangeException.ThrowIfNegative(vehicleIndex, nameof(vehicleIndex));
        
        RoutingModel.SetArcCostEvaluatorOfVehicle(callback, vehicleIndex);
    }

    /// <summary>
    /// Sets the arc cost evaluator.
    /// </summary>
    /// <param name="evaluator">The evaluator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="evaluator"/> is <see langword="null"/>.</exception>
    internal void SetArcCostEvaluator(IArcCostEvaluator evaluator)
    {
        ArgumentNullException.ThrowIfNull(evaluator, nameof(evaluator));
        
        for (var i = 0; i < SolverModel.VehicleCount; i++)
        {
            var vehicle = SolverModel.Vehicles[i];
            
            var callback = new ArcCostEvaluatorCallback(evaluator, vehicle);
            var solverCallback = RegisterCallback(callback);
            
            SetArcCostEvaluatorOfVehicle(solverCallback, i);
        }
    }
    
    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The internal solver dimension.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension"/> is <see langword="null"/>.</exception>
    internal SolverDimension RegisterDimension(TKey key, ISingleCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        
        return DimensionRegistry.RegisterDimension(key, dimension);
    }
    
    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The internal solver dimension.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension"/> is <see langword="null"/>.</exception>
    internal SolverDimension RegisterDimension(TKey key, IMultiCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        
        return DimensionRegistry.RegisterDimension(key, dimension);
    }
    
    /// <summary>
    /// Gets the dimension from the given key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The dimension.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the key is not found.</exception>
    internal SolverDimension GetDimension(TKey key)
    {
        return DimensionRegistry.GetDimension(key);
    }
    
    /// <summary>
    /// Solves the model with the specified time limit.
    /// </summary>
    /// <param name="timeLimit">The time limit.</param>
    /// <returns>The internal assignment.</returns>
    internal Assignment? Solve(TimeSpan timeLimit)
    {
        RoutingSearchParameters searchParameters = GetSearchParameters(timeLimit);
        return RoutingModel.SolveWithParameters(searchParameters);
    }
    
    /// <summary>
    /// Gets the search parameters.
    /// </summary>
    /// <param name="timeLimit">The time limit.</param>
    /// <returns>The search parameters.</returns>
    private RoutingSearchParameters GetSearchParameters(TimeSpan timeLimit)
    {
        var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
        searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
        searchParameters.LocalSearchMetaheuristic = LocalSearchMetaheuristic.Types.Value.GuidedLocalSearch;
        searchParameters.TimeLimit = new Duration { Seconds = timeLimit.Seconds };
        return searchParameters;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        IndexManager.Dispose();
        RoutingModel.Dispose();
        DimensionRegistry.Dispose();
        CallbackRegistrant.Dispose();
    }
}