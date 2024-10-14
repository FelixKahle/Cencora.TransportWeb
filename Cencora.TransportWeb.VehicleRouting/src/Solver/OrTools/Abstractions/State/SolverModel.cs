// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Collections.Immutable;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// Represents the internal model of the solver.
/// </summary>
internal sealed class SolverModel
{
    /// <summary>
    /// Gets the nodes of the model.
    /// </summary>
    internal ImmutableList<Node> Nodes { get; }
    
    /// <summary>
    /// Gets the vehicles of the model.
    /// </summary>
    internal ImmutableList<DummyVehicle> Vehicles { get; }
    
    /// <summary>
    /// Gets the shipment node stores of the model.
    /// </summary>
    internal ImmutableDictionary<Shipment, ShipmentNodeStore> ShipmentNodeStores { get; }
    
    /// <summary>
    /// Gets the vehicle node stores of the model.
    /// </summary>
    internal ImmutableDictionary<DummyVehicle, VehicleNodeStore> VehicleNodeStores { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverModel"/> class.
    /// </summary>
    /// <param name="nodes">The nodes of the model.</param>
    /// <param name="vehicles">The vehicles of the model.</param>
    /// <param name="shipmentNodeStores">The shipment node stores of the model.</param>
    /// <param name="vehicleNodeStores">The vehicle node stores of the model.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="nodes"/>, <paramref name="vehicles"/>, <paramref name="shipmentNodeStores"/> or <paramref name="vehicleNodeStores"/> is <see langword="null"/>.</exception>
    internal SolverModel(ImmutableList<Node> nodes, ImmutableList<DummyVehicle> vehicles, ImmutableDictionary<Shipment, ShipmentNodeStore> shipmentNodeStores, ImmutableDictionary<DummyVehicle, VehicleNodeStore> vehicleNodeStores)
    {
        ArgumentNullException.ThrowIfNull(nodes, nameof(nodes));
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));
        ArgumentNullException.ThrowIfNull(shipmentNodeStores, nameof(shipmentNodeStores));
        ArgumentNullException.ThrowIfNull(vehicleNodeStores, nameof(vehicleNodeStores));
        
        Nodes = nodes;
        Vehicles = vehicles;
        ShipmentNodeStores = shipmentNodeStores;
        VehicleNodeStores = vehicleNodeStores;
    }
    
    /// <summary>
    /// The number of nodes of the solver.
    /// </summary>
    internal int NodeCount => Nodes.Count;
    
    /// <summary>
    /// The number of vehicles of the solver.
    /// </summary>
    internal int VehicleCount => Vehicles.Count;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"SolverModel: NodeCount={NodeCount}, VehicleCount={VehicleCount}";
    }
}