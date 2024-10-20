// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Collections.Immutable;
using Cencora.TransportWeb.Common.MathUtils;
using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Implementation;

/// <summary>
/// Represents the default implementation of the solver model factory.
/// </summary>
internal class DefaultSolverModelFactory : ISolverModelFactory
{
    /// <inheritdoc/>
    public SolverModel Create(Problem problem)
    {
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));
        
        // Calculate the number of nodes and vehicles.
        // We can use this to pre-allocate the lists, which is more efficient.
        var dummyVehicleCount = problem.Vehicles.Sum(vehicle => vehicle.ShiftCount);
        var nodeCount = problem.ShipmentCount * 2 + dummyVehicleCount * 2;
        
        var nodes = new List<Node>(nodeCount);
        var vehicles = new List<DummyVehicle>(dummyVehicleCount);
        var shipmentNodeStores = new Dictionary<Shipment, ShipmentNodeStore>(problem.ShipmentCount);
        var vehicleNodeStores = new Dictionary<DummyVehicle, VehicleNodeStore>(dummyVehicleCount);
        
        // Create shipment nodes.
        foreach (var shipment in problem.Shipments)
        {
            // Create the pickup node and add it to the list.
            var pickupNode = CreateShipmentNode(nodes.Count, shipment, shipment.PickupLocation, ShipmentNodeType.Pickup);
            nodes.Add(pickupNode);
            
            // Create the delivery node and add it to the list.
            var deliveryNode = CreateShipmentNode(nodes.Count, shipment, shipment.DeliveryLocation, ShipmentNodeType.Delivery);
            nodes.Add(deliveryNode);
            
            // Add the shipment node stores.
            shipmentNodeStores.Add(shipment, new ShipmentNodeStore(pickupNode, deliveryNode));
        }
        
        // Create the dummy vehicles and vehicle nodes.
        foreach (var vehicle in problem.Vehicles)
        {
            foreach (var shift in vehicle.Shifts)
            {
                // Create the dummy vehicle and add it to the list.
                var dummyVehicle = CreateDummyVehicle(vehicles.Count, vehicle, shift);
                vehicles.Add(dummyVehicle);
                
                // Create the start node and add it to the list.
                var vehicleNode = CreateVehicleNode(nodes.Count, CreateDummyVehicle(vehicles.Count, vehicle, shift), shift.StartLocation, VehicleNodeType.Start);
                nodes.Add(vehicleNode);
                
                // Create the end node and add it to the list.
                var endNode = CreateVehicleNode(nodes.Count, CreateDummyVehicle(vehicles.Count, vehicle, shift), shift.EndLocation, VehicleNodeType.End);
                nodes.Add(endNode);
                
                // Add the vehicle node stores.
                vehicleNodeStores.Add(dummyVehicle, new VehicleNodeStore(vehicleNode, endNode));
            }
        }
        
        return new SolverModel(nodes.ToImmutableList(), vehicles.ToImmutableList(), shipmentNodeStores.ToImmutableDictionary(), vehicleNodeStores.ToImmutableDictionary());
    }
    
    /// <summary>
    /// Creates a shipment node.
    /// </summary>
    /// <param name="index">The index of the node.</param>
    /// <param name="shipment">The shipment.</param>
    /// <param name="location">The location of the node.</param>
    /// <param name="nodeType">The type of the node.</param>
    /// <returns>The shipment node.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shipment"/> is <see langword="null"/>.</exception>
    private static ShipmentNode CreateShipmentNode(int index, Shipment shipment, Location? location, ShipmentNodeType nodeType)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(shipment, nameof(shipment));
        
        return new ShipmentNode(index, shipment, location, nodeType);
    }
    
    /// <summary>
    /// Creates a vehicle node.
    /// </summary>
    /// <param name="index">The index of the node.</param>
    /// <param name="dummyVehicle">The dummy vehicle.</param>
    /// <param name="location">The location of the node.</param>
    /// <param name="nodeType">The type of the node.</param>
    /// <returns>The vehicle node.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dummyVehicle"/> is <see langword="null"/>.</exception>
    private static VehicleNode CreateVehicleNode(int index, DummyVehicle dummyVehicle, Location? location, VehicleNodeType nodeType)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(dummyVehicle, nameof(dummyVehicle));
        
        return new VehicleNode(index, dummyVehicle, location, nodeType);
    }
    
    /// <summary>
    /// Creates a dummy vehicle.
    /// </summary>
    /// <param name="index">The index of the vehicle.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="shift">The shift.</param>
    /// <returns>The dummy vehicle.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> or <paramref name="shift"/> is <see langword="null"/>.</exception>
    private static DummyVehicle CreateDummyVehicle(int index, Vehicle vehicle, Shift shift)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(shift, nameof(shift));

        // We need the number of shifts to adjust the fixed and base costs.
        var shiftCount = vehicle.Shifts.Count;

        var adjustedFixedCost = MathUtils.DivideOrDefault(vehicle.FixedCost ?? 0, shiftCount, 0, 0);
        var adjustedBaseCost = MathUtils.DivideOrDefault(vehicle.BaseCost ?? 0, shiftCount, 0, 0);
        var fixedCost = MathUtils.AddOrDefault(adjustedBaseCost, shift.FixedCost ?? 0, long.MaxValue);
        var baseCost = MathUtils.AddOrDefault(adjustedFixedCost, shift.BaseCost ?? 0, long.MaxValue);
        var distanceCost = MathUtils.AddOrDefault(vehicle.DistanceCost ?? 0, shift.DistanceCost ?? 0, long.MaxValue);
        var timeCost = MathUtils.AddOrDefault(vehicle.TimeCost ?? 0, shift.TimeCost ?? 0, long.MaxValue);
        var maxDuration = Math.Min(shift.MaxDuration ?? long.MaxValue, shift.ShiftTimeWindow.Difference);
        
        return new DummyVehicleBuilder(index, vehicle, shift)
            .WithFixedCost(fixedCost)
            .WithBaseCost(baseCost)
            .WithDistanceCost(distanceCost)
            .WithTimeCost(timeCost)
            .WithWeightCost(vehicle.WeightCost ?? 0)
            .WithCostPerWeightDistance(vehicle.CostPerWeightDistance ?? 0)
            .WithMaxWeight(vehicle.MaxWeight ?? long.MaxValue)
            .WithMaxTotalWeight(long.MaxValue)
            .WithMaxDistance(shift.MaxDistance ?? long.MaxValue)
            .WithMaxDuration(maxDuration)
            .Build();
    }
}