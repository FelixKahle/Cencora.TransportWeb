// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Collections.Immutable;
using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;
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
    /// The solver of the model.
    /// </summary>
    internal Google.OrTools.ConstraintSolver.Solver Solver => RoutingModel.solver() ?? throw new VehicleRoutingSolverException("Failed to get solver.");

    /// <summary>
    /// The callbacks of the solver.
    /// </summary>
    internal SolverCallbackManager CallbackManager { get; }
    
    /// <summary>
    /// The dimension manager of the solver.
    /// </summary>
    internal SolverDimensionManager DimensionManager { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverInterface"/> class.
    /// </summary>
    /// <param name="model">The model of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <see langword="null"/>.</exception>
    internal SolverInterface(SolverModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        
        var (startNodeIndices, endNodeIndices) = GetVehicleNodeIndices(model.Vehicles, model.VehicleNodeStores);
        SolverModel = model;
        IndexManager = new RoutingIndexManager(model.NodeCount, model.VehicleCount, startNodeIndices, endNodeIndices);
        RoutingModel = new RoutingModel(IndexManager);
        CallbackManager = new SolverCallbackManager(model, IndexManager, RoutingModel);
        DimensionManager = new SolverDimensionManager(model, RoutingModel, CallbackManager);
    }

    /// <summary>
    /// Gets the start and end node indices of the vehicles.
    /// </summary>
    /// <returns>The start and end node indices of the vehicles.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicles"/> or <paramref name="vehicleNodeStores"/> is <see langword="null"/>.</exception>
    private static (int[] StartNodeIndices, int[] EndNodeIndices) GetVehicleNodeIndices(ImmutableList<DummyVehicle> vehicles, ImmutableDictionary<DummyVehicle, VehicleNodeStore> vehicleNodeStores)
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

    /// <inheritdoc/>
    public void Dispose()
    {
        IndexManager.Dispose();
        RoutingModel.Dispose();
    }
}