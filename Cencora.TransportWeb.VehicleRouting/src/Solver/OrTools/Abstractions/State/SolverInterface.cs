// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Vehicles;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// Represents the interface of the solver to the Google OR-Tools routing library.
/// </summary>
internal sealed class SolverInterface : IDisposable
{
    /// <summary>
    /// The routing model of the solver.
    /// </summary>
    internal RoutingModel Model { get; }
    
    /// <summary>
    /// The index manager of the solver.
    /// </summary>
    internal RoutingIndexManager IndexManager { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverInterface"/> class.
    /// </summary>
    /// <param name="model">The routing model of the solver.</param>
    /// <param name="indexManager">The index manager of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="indexManager"/> is <see langword="null"/>.</exception>
    public SolverInterface(RoutingModel model, RoutingIndexManager indexManager)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        ArgumentNullException.ThrowIfNull(indexManager, nameof(indexManager));
        
        Model = model;
        IndexManager = indexManager;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverInterface"/> class.
    /// </summary>
    /// <param name="model">The model of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <see langword="null"/>.</exception>
    public SolverInterface(SolverModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        
        var (startNodeIndices, endNodeIndices) = GetVehicleNodeIndices(model.Vehicles, model.VehicleNodeStores);
        IndexManager = new RoutingIndexManager(model.NodeCount, model.VehicleCount, startNodeIndices, endNodeIndices);
        Model = new RoutingModel(IndexManager);
    }
    
    /// <summary>
    /// Registers a transit callback.
    /// </summary>
    /// <param name="callback">The callback to register.</param>
    /// <returns>The index of the callback.</returns>
    internal int RegisterTransitCallback(Func<long, long, long> callback) => Model.RegisterTransitCallback((fromIndex, toIndex) => callback(fromIndex, toIndex));
    
    /// <summary>
    /// Registers a unary transit callback.
    /// </summary>
    /// <param name="callback">The callback to register.</param>
    /// <returns>The index of the callback.</returns>
    internal int RegisterTransitCallback(Func<long, long> callback) => Model.RegisterUnaryTransitCallback((index) => callback(index));
    
    /// <summary>
    /// Converts a solver specific index to a node index.
    /// </summary>
    /// <param name="index">The solver specific index.</param>
    /// <returns>The node index.</returns>
    internal int IndexToNode(long index) => IndexManager.IndexToNode(index);
    
    /// <summary>
    /// Converts a node index to a solver specific index.
    /// </summary>
    /// <param name="node">The node index.</param>
    /// <returns>The solver specific index.</returns>
    internal long NodeToIndex(Node node) => IndexManager.NodeToIndex(node);
    
    /// <summary>
    /// Gets the start and end node indices of the vehicles.
    /// </summary>
    /// <returns>The start and end node indices of the vehicles.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicles"/> or <paramref name="vehicleNodeStores"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when the number of start and end node indices does not match.</exception>
    private static (int[] StartNodeIndices, int[] EndNodeIndices) GetVehicleNodeIndices(IReadOnlyList<DummyVehicle> vehicles, IReadOnlyDictionary<DummyVehicle, VehicleNodeStore> vehicleNodeStores)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));
        ArgumentNullException.ThrowIfNull(vehicleNodeStores, nameof(vehicleNodeStores));
        
        var vehicleNodeIndices = vehicles
            .Select(v => (StartNode: vehicleNodeStores[v].StartNode.Index, EndNode: vehicleNodeStores[v].EndNode.Index))
            .ToArray();

        var startNodeIndices = vehicleNodeIndices.Select(x => x.StartNode).ToArray();
        var endNodeIndices = vehicleNodeIndices.Select(x => x.EndNode).ToArray();

        return (startNodeIndices, endNodeIndices);
    }

    /// <summary>
    /// Disposes the solver interface.
    /// </summary>
    public void Dispose()
    {
        Model.Dispose();
        IndexManager.Dispose();
    }
}