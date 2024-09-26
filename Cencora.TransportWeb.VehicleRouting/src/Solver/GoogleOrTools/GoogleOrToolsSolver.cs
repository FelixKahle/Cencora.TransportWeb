// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.MathUtils;
using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Solver implementation using Google OR-Tools.
/// </summary>
public sealed class GoogleOrToolsSolver : GoogleOrToolsSolverBase, ISolver
{
    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
        // Make sure the solver is reset before solving a new problem.
        Reset();

        // Initialize the solver with the given problem.
        InitializeSolver(problem);

        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes the solver.
    /// </summary>
    /// <param name="problem">The problem to solve.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="problem"/> is <see langword="null"/>.</exception>
    private void InitializeSolver(Problem problem)
    {
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));

        // Precalculate the number of nodes, vehicles, and shipments.
        // This way we avoid resizing the lists later on.
        // Each shipment has two nodes (pickup and delivery) and each vehicle has two nodes (start and end).
        var shipmentCount = problem.Shipments.Count;
        var vehicleCount = problem.Vehicles.Sum(v => v.Shifts.Count);
        var nodeCount = shipmentCount * 2 + vehicleCount * 2;

        // Initialize various components
        InitializeNodes(nodeCount);
        InitializeVehicles(vehicleCount);
        InitializeVehiclesToNodeStore(vehicleCount);
        InitializeVehiclesToTransitCallbackIndex(vehicleCount);
        SetupShipments(problem.Shipments);
        SetupDummyVehicles(problem.Vehicles);

        // Get vehicle start and end node indices
        (var vehicleStartNodeIndices, var vehicleEndNodeIndices) = GetVehicleNodeIndices();

        // Initialize routing components
        InitializeRoutingIndexManager(nodeCount, vehicleCount, vehicleStartNodeIndices, vehicleEndNodeIndices);
        InitializeRoutingModel();
    }

    /// <summary>
    /// Gets the start and end node indices of the vehicles.
    /// </summary>
    /// <returns>The start and end node indices of the vehicles.</returns>
    private (int[] StartNodeIndices, int[] EndNodeIndices) GetVehicleNodeIndices()
    {
        var vehicleNodeIndices = Vehicles
            .Select(v => (StartNode: VehiclesToNodeStore[v].StartNode.Index, EndNode: VehiclesToNodeStore[v].EndNode.Index))
            .ToArray();

        var startNodeIndices = vehicleNodeIndices.Select(x => x.StartNode).ToArray();
        var endNodeIndices = vehicleNodeIndices.Select(x => x.EndNode).ToArray();

        return (startNodeIndices, endNodeIndices);
    }

    /// <summary>
    /// Sets up the shipments of the solver.
    /// </summary>
    /// <param name="shipments">The shipments to set up.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shipments"/> is <see langword="null"/>.</exception>
    private void SetupShipments(IReadOnlySet<Shipment> shipments)
    {
        ArgumentNullException.ThrowIfNull(shipments, nameof(shipments));

        foreach (var shipment in shipments)
        {
            var pickupNode = CreateShipmentNode(shipment, shipment.PickupLocation, ShipmentNodeType.Pickup);
            var deliveryNode = CreateShipmentNode(shipment, shipment.DeliveryLocation, ShipmentNodeType.Delivery);

            Nodes.Add(pickupNode);
            Nodes.Add(deliveryNode);
        }
    }

    /// <summary>
    /// Sets up the dummy vehicles of the solver.
    /// </summary>
    /// <param name="vehicles">The vehicles to set up.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is <see langword="null"/>.</exception>
    private void SetupDummyVehicles(IReadOnlySet<Vehicle> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));

        foreach (var vehicle in vehicles)
        {
            foreach (var shift in vehicle.Shifts)
            {
                var dummyVehicle = CreateDummyVehicle(vehicle, shift);
                var startNode = CreateVehicleNode(dummyVehicle, shift.StartLocation, VehicleNodeType.Start);
                var endNode = CreateVehicleNode(dummyVehicle, shift.EndLocation, VehicleNodeType.End);

                Vehicles.Add(dummyVehicle);
                Nodes.Add(startNode);
                Nodes.Add(endNode);
            }
        }
    }

    /// <summary>
    /// Creates a dummy vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="shift">The shift.</param>
    /// <returns>The dummy vehicle.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> or <paramref name="shift"/> is <see langword="null"/>.</exception>
    private DummyVehicle CreateDummyVehicle(Vehicle vehicle, Shift shift)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(shift, nameof(shift));

        var shiftCount = vehicle.Shifts.Count;

        var adjustedFixedCost = MathUtils.DivideOrDefault(vehicle.FixedCost ?? 0, shiftCount, 0, 0);
        var adjustedBaseCost = MathUtils.DivideOrDefault(vehicle.BaseCost ?? 0, shiftCount, 0, 0);
        var fixedCost = MathUtils.AddOrDefault(adjustedBaseCost, shift.FixedCost ?? 0, long.MaxValue);
        var baseCost = MathUtils.AddOrDefault(adjustedFixedCost, shift.BaseCost ?? 0, long.MaxValue);
        var distanceCost = MathUtils.AddOrDefault(vehicle.DistanceCost ?? 0, shift.DistanceCost ?? 0, long.MaxValue);
        var timeCost = MathUtils.AddOrDefault(vehicle.TimeCost ?? 0, shift.TimeCost ?? 0, long.MaxValue);

        var dummyVehicleIndex = VehicleCount;
        return new DummyVehicleBuilder(dummyVehicleIndex, vehicle, shift)
            .WithFixedCost(fixedCost)
            .WithBaseCost(baseCost)
            .WithDistanceCost(distanceCost)
            .WithTimeCost(timeCost)
            .WithWeightCost(vehicle.WeightCost ?? 0)
            .WithCostPerWeightDistance(vehicle.CostPerWeightDistance ?? 0)
            .WithMaxWeight(vehicle.MaxWeight ?? long.MaxValue)
            .WithMaxDistance(shift.MaxDistance ?? long.MaxValue)
            .WithMaxDuration(shift.MaxDuration ?? long.MaxValue)
            .Build();
    }

    /// <summary>
    /// Creates a shipment node.
    /// </summary>
    /// <param name="shipment">The shipment.</param>
    /// <param name="location">The location of the node.</param>
    /// <param name="nodeType">The type of the node.</param>
    /// <returns>The shipment node.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shipment"/> is <see langword="null"/>.</exception>
    private ShipmentNode CreateShipmentNode(Shipment shipment, Location? location, ShipmentNodeType nodeType)
    {
        ArgumentNullException.ThrowIfNull(shipment, nameof(shipment));

        var nodeIndex = NodeCount;
        return new ShipmentNode(nodeIndex, shipment, location, nodeType);
    }

    /// <summary>
    /// Creates a vehicle node.
    /// </summary>
    /// <param name="dummyVehicle">The dummy vehicle.</param>
    /// <param name="location">The location of the node.</param>
    /// <param name="nodeType">The type of the node.</param>
    /// <returns>The vehicle node.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dummyVehicle"/> is <see langword="null"/>.</exception>
    private VehicleNode CreateVehicleNode(DummyVehicle dummyVehicle, Location? location, VehicleNodeType nodeType)
    {
        ArgumentNullException.ThrowIfNull(dummyVehicle, nameof(dummyVehicle));

        var nodeIndex = NodeCount;
        return new VehicleNode(nodeIndex, dummyVehicle, location, nodeType);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(GoogleOrToolsSolver)}";
    }
}
