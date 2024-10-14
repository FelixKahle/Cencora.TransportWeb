// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Collections.Immutable;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// The node indices of the vehicles.
/// </summary>
internal readonly struct VehicleNodeIndices
{
    /// <summary>
    /// Gets the start node indices.
    /// </summary>
    internal int[] StartNodeIndices { get; }
    
    /// <summary>
    /// Gets the end node indices.
    /// </summary>
    internal int[] EndNodeIndices { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleNodeIndices"/> struct.
    /// </summary>
    /// <param name="startNodeIndices">The start node indices.</param>
    /// <param name="endNodeIndices">The end node indices.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="startNodeIndices"/> or <paramref name="endNodeIndices"/> is <see langword="null"/>.</exception>
    internal VehicleNodeIndices(int[] startNodeIndices, int[] endNodeIndices)
    {
        ArgumentNullException.ThrowIfNull(startNodeIndices, nameof(startNodeIndices));
        ArgumentNullException.ThrowIfNull(endNodeIndices, nameof(endNodeIndices));
        
        StartNodeIndices = startNodeIndices;
        EndNodeIndices = endNodeIndices;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleNodeIndices"/> struct.
    /// </summary>
    /// <param name="vehicles">The vehicles.</param>
    /// <param name="vehicleNodeStores">The vehicle node stores.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicles"/> or <paramref name="vehicleNodeStores"/> is <see langword="null"/>.</exception>
    internal VehicleNodeIndices(ImmutableList<DummyVehicle> vehicles, ImmutableDictionary<DummyVehicle, VehicleNodeStore> vehicleNodeStores)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));
        ArgumentNullException.ThrowIfNull(vehicleNodeStores, nameof(vehicleNodeStores));
        
        StartNodeIndices = vehicles.Select(v => vehicleNodeStores[v].StartNode.Index).ToArray();
        EndNodeIndices = vehicles.Select(v => vehicleNodeStores[v].EndNode.Index).ToArray();
    }
}