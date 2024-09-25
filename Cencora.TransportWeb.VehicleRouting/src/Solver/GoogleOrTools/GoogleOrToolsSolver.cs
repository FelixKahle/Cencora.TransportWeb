// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Runtime.CompilerServices;
using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Solver implementation using Google OR-Tools.
/// </summary>
public sealed class GoogleOrToolsSolver : ISolver, IDisposable
{
    private RoutingIndexManager? _indexManager;
    private RoutingModel? _routingModel;
    private List<Node>? _nodes;
    private List<DummyVehicle>? _vehicles;
    private Dictionary<DummyVehicle, int>? _vehiclesToTransitCallbackIndex;
    private Dictionary<DummyVehicle, VehicleNodeStore>? _vehiclesToNodeStore;
    private IReadOnlyDirectedRouteMatrix? _routeMatrix;

    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
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

        // Each shift of a vehicle is represented by a dummy vehicle.
        // Each shipment has a pickup and a delivery node.
        // Each dummy vehicle has a start and an end node.
        var shipmentCount = problem.Shipments.Count;
        var dummyVehicleCount = problem.Vehicles.Sum(v => v.Shifts.Count);
        var nodeCount = shipmentCount * 2 + dummyVehicleCount * 2;

        // Make sure all structures that store the internal model are initialized.
        InitializeNodes(nodeCount);
        InitializeVehicles(dummyVehicleCount);
        InitializeVehicleNodeStore(dummyVehicleCount);

        // Build the internal model.
        InitializeShipments(problem.Shipments);
        InitializeDummyVehicles(problem.Vehicles);
    }

    /// <summary>
    /// Initializes the Shipments of the solver.
    /// </summary>
    /// <param name="shipments">The shipments to initialize.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shipments"/> is <see langword="null"/>.</exception>
    private void InitializeShipments(IReadOnlySet<Shipment> shipments)
    {
        ArgumentNullException.ThrowIfNull(shipments, nameof(shipments));

        var nodes = GetNodes();

        foreach (var shipment in shipments)
        {
            var pickupNode = CreateShipmentNode(shipment, shipment.PickupLocation, ShipmentNodeType.Pickup);
            var deliveryNode = CreateShipmentNode(shipment, shipment.DeliveryLocation, ShipmentNodeType.Delivery);

            nodes.Add(pickupNode);
            nodes.Add(deliveryNode);
        }
    }

    /// <summary>
    /// Initializes the Vehicles of the solver.
    /// </summary>
    /// <param name="vehicles">The vehicles to initialize.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is <see langword="null"/>.</exception>
    private void InitializeDummyVehicles(IReadOnlySet<Vehicle> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));

        var dummyVehicles = GetVehicles();
        var nodes = GetNodes();

        foreach (var vehicle in vehicles)
        {
            foreach (var shift in vehicle.Shifts)
            {
                var dummyVehicle = CreateDummyVehicle(vehicle, shift);
                var startNode = CreateVehicleNode(dummyVehicle, shift.StartLocation, VehicleNodeType.Start);
                var endNode = CreateVehicleNode(dummyVehicle, shift.EndLocation, VehicleNodeType.End);

                dummyVehicles.Add(dummyVehicle);
                nodes.Add(startNode);
                nodes.Add(endNode);
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

        var dummyVehicleIndex = GetVehicles().Count;
        return new DummyVehicleBuilder(dummyVehicleIndex, vehicle, shift)
            .WithFixedCost(vehicle.FixedCost ?? 0)
            .WithBaseCost(vehicle.BaseCost ?? 0)
            .WithDistanceCost(vehicle.DistanceCost ?? 0)
            .WithTimeCost(vehicle.TimeCost ?? 0)
            .WithWeightCost(vehicle.WeightCost ?? 0)
            .WithCostPerWeightDistance(vehicle.CostPerWeightDistance ?? 0)
            .WithMaxDistance(vehicle.MaxDistance ?? 0)
            .WithMaxDuration(vehicle.MaxDuration ?? 0)
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

        var nodeIndex = GetNodes().Count;
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

        var nodeIndex = GetNodes().Count;
        return new VehicleNode(nodeIndex, dummyVehicle, location, nodeType);
    }

    /// <summary>
    /// Returns the distance between two nodes.
    /// </summary>
    /// <param name="fromNode">The node to start from.</param>
    /// <param name="toNode">The node to end at.</param>
    /// <returns>The distance between the two nodes.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromNode"/> or <paramref name="toNode"/> is <see langword="null"/>.</exception>
    internal long GetDistance(Node fromNode, Node toNode)
    {
        ArgumentNullException.ThrowIfNull(fromNode, nameof(fromNode));
        ArgumentNullException.ThrowIfNull(toNode, nameof(toNode));

        var routeMatrix = GetRouteMatrix();
        var fromLocation = fromNode.GetLocation();
        var toLocation = toNode.GetLocation();

        // Arbitrary nodes have a distance of 0.
        if (fromLocation is null || toLocation is null)
        {
            return 0;
        }

        return routeMatrix.GetEdge(fromLocation, toLocation) switch
        {
            DefinedRouteEdge definedRouteEdge => definedRouteEdge.Distance,
            _ => long.MaxValue
        };
    }

    /// <summary>
    /// Resets the solver.
    /// </summary>
    public void Reset()
    {
        _routingModel?.Dispose();
        _indexManager?.Dispose();
        _nodes?.Clear();
        _vehicles?.Clear();
        _vehiclesToTransitCallbackIndex?.Clear();

        _routingModel = null;
        _nodes = null;
        _vehicles = null;
        _vehiclesToTransitCallbackIndex = null;
        _routeMatrix = null;
        _vehiclesToNodeStore = null;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Reset();
    }

    /// <summary>
    /// Gets the <see cref="GetRoutingIndexManager"/>.
    /// </summary>
    /// <returns>The routing index manager.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the routing index manager has not been initialized.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal RoutingIndexManager GetRoutingIndexManager()
    {
        return _indexManager ?? throw new InvalidOperationException("The routing index manager has not been initialized.");
    }

    /// <summary>
    /// Sets the <see cref="RoutingIndexManager"/>.
    /// </summary>
    /// <param name="indexManager">The routing index manager.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexManager"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetRoutingIndexManager(RoutingIndexManager indexManager)
    {
        ArgumentNullException.ThrowIfNull(indexManager, nameof(indexManager));

        _indexManager = indexManager;
    }

    /// <summary>
    /// Gets the <see cref="RoutingModel"/>.
    /// </summary>
    /// <returns>The routing model.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the routing model has not been initialized.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal RoutingModel GetRoutingModel()
    {
        return _routingModel ?? throw new InvalidOperationException("The routing model has not been initialized.");
    }

    /// <summary>
    /// Sets the <see cref="RoutingModel"/>.
    /// </summary>
    /// <param name="routingModel">The routing model.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routingModel"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetRoutingModel(RoutingModel routingModel)
    {
        ArgumentNullException.ThrowIfNull(routingModel, nameof(routingModel));

        _routingModel = routingModel;
    }

    /// <summary>
    /// Gets the nodes of the solver.
    /// </summary>
    /// <returns>The nodes of the solver.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the nodes have not been initialized.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal List<Node> GetNodes()
    {
        return _nodes ?? throw new InvalidOperationException("The nodes have not been initialized.");
    }

    /// <summary>
    /// Sets the nodes of the solver.
    /// </summary>
    /// <param name="nodes">The nodes of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="nodes"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetNodes(List<Node> nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes, nameof(nodes));

        _nodes = nodes;
    }

    /// <summary>
    /// Initializes the nodes of the solver.
    /// </summary>
    /// <param name="capacity">The capacity of the nodes.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void InitializeNodes(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        _nodes = new List<Node>(adjustedCapacity);
    }

    /// <summary>
    /// Gets the vehicles of the solver.
    /// </summary>
    /// <returns>The vehicles of the solver.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles have not been initialized.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal List<DummyVehicle> GetVehicles()
    {
        return _vehicles ?? throw new InvalidOperationException("The vehicles have not been initialized.");
    }

    /// <summary>
    /// Sets the vehicles of the solver.
    /// </summary>
    /// <param name="vehicles">The vehicles of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetVehicles(List<DummyVehicle> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles, nameof(vehicles));

        _vehicles = vehicles;
    }

    /// <summary>
    /// Initializes the vehicles of the solver.
    /// </summary>
    /// <param name="capacity">The capacity of the vehicles.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void InitializeVehicles(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        _vehicles = new List<DummyVehicle>(adjustedCapacity);
    }

    /// <summary>
    /// Gets the vehicles to transit callback index.
    /// </summary>
    /// <returns>The vehicles to transit callback index.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles to transit callback index has not been initialized.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Dictionary<DummyVehicle, int> GetVehiclesToTransitCallbackIndex()
    {
        return _vehiclesToTransitCallbackIndex ?? throw new InvalidOperationException("The vehicles to transit callback index has not been initialized.");
    }

    /// <summary>
    /// Sets the vehicles to transit callback index.
    /// </summary>
    /// <param name="vehiclesToTransitCallbackIndex">The vehicles to transit callback index.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehiclesToTransitCallbackIndex"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetVehiclesToTransitCallbackIndex(Dictionary<DummyVehicle, int> vehiclesToTransitCallbackIndex)
    {
        ArgumentNullException.ThrowIfNull(vehiclesToTransitCallbackIndex, nameof(vehiclesToTransitCallbackIndex));

        _vehiclesToTransitCallbackIndex = vehiclesToTransitCallbackIndex;
    }

    /// <summary>
    /// Initializes the vehicles to transit callback index.
    /// </summary>
    /// <param name="capacity">The capacity of the vehicles to transit callback index.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void InitializeVehiclesToTransitCallbackIndex(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        _vehiclesToTransitCallbackIndex = new Dictionary<DummyVehicle, int>(adjustedCapacity);
    }

    /// <summary>
    /// Gets the route matrix of the solver.
    /// </summary>
    /// <returns>The route matrix of the solver.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the route matrix has not been initialized.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal IReadOnlyDirectedRouteMatrix GetRouteMatrix()
    {
        return _routeMatrix ?? throw new InvalidOperationException("The route matrix has not been initialized.");
    }

    /// <summary>
    /// Sets the route matrix of the solver.
    /// </summary>
    /// <param name="routeMatrix">The route matrix of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routeMatrix"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetRouteMatrix(IReadOnlyDirectedRouteMatrix routeMatrix)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));

        _routeMatrix = routeMatrix;
    }

    /// <summary>
    /// Gets the vehicles to node store.
    /// </summary>
    /// <returns>The vehicles to node store.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles to node store has not been initialized.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Dictionary<DummyVehicle, VehicleNodeStore> GetVehiclesToNodeStore()
    {
        return _vehiclesToNodeStore ?? throw new InvalidOperationException("The vehicles to node store has not been initialized.");
    }

    /// <summary>
    /// Sets the vehicles to node store.
    /// </summary>
    /// <param name="vehiclesToNodeStore">The vehicles to node store.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehiclesToNodeStore"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetVehiclesToNodeStore(Dictionary<DummyVehicle, VehicleNodeStore> vehiclesToNodeStore)
    {
        ArgumentNullException.ThrowIfNull(vehiclesToNodeStore, nameof(vehiclesToNodeStore));

        _vehiclesToNodeStore = vehiclesToNodeStore;
    }

    /// <summary>
    /// Initializes the vehicles to node store.
    /// </summary>
    /// <param name="capacity">The capacity of the vehicles to node store.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void InitializeVehicleNodeStore(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        _vehiclesToNodeStore = new Dictionary<DummyVehicle, VehicleNodeStore>(adjustedCapacity);
    }
}
