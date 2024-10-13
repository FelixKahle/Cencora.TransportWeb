// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

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
    /// <param name="state">The state of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/> is <see langword="null"/>.</exception>
    public SolverInterface(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        var (startNodeIndices, endNodeIndices) = GetVehicleNodeIndices(state.Vehicles, state.VehicleNodeStores);
        IndexManager = new RoutingIndexManager(state.NodeCount, state.VehicleCount, startNodeIndices, endNodeIndices);
        Model = new RoutingModel(IndexManager);
    }
    
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