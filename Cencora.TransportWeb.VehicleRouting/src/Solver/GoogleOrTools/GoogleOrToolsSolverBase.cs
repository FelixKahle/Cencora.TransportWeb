// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Base class for Google OR-Tools solver implementations.
/// </summary>
/// <remarks>
/// The base class is responsible for the internal state keeping of the solver.
/// This seperates the solver logic from the internal state management,
/// and disallows the direct access to the internal collections and interfaces.
/// </remarks>
public abstract class GoogleOrToolsSolverBase : IDisposable
{
    private RoutingIndexManager? _indexManager;
    private RoutingModel? _routingModel;
    private List<Node>? _nodes;
    private List<DummyVehicle>? _vehicles;
    private Dictionary<DummyVehicle, int>? _vehiclesToTransitCallbackIndex;
    private Dictionary<DummyVehicle, VehicleNodeStore>? _vehiclesToNodeStore;
    private Dictionary<Shipment, ShipmentNodeStore>? _shipmentsToNodeStore;
    private IReadOnlyDirectedRouteMatrix? _routeMatrix;

    /// <summary>
    /// Gets or sets the <see cref="RoutingIndexManager"/> of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the routing index manager is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    protected internal RoutingIndexManager IndexManager
    {
        get => _indexManager ?? throw new InvalidOperationException("The routing index manager is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _indexManager = value;
        }
    }

    /// <summary>
    /// Initializes the routing index manager of the solver.
    /// </summary>
    /// <param name="nodeCount">The number of nodes.</param>
    /// <param name="vehicleCount">The number of vehicles.</param>
    /// <param name="startNodes">The start nodes of the vehicles.</param>
    /// <param name="endNodes">The end nodes of the vehicles.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="startNodes"/> or <paramref name="endNodes"/> is <see langword="null"/>.</exception>
    protected internal void InitializeRoutingIndexManager(int nodeCount, int vehicleCount, int[] startNodes, int[] endNodes)
    {
        ArgumentNullException.ThrowIfNull(startNodes, nameof(startNodes));
        ArgumentNullException.ThrowIfNull(endNodes, nameof(endNodes));

        var adjustedNodeCount = Math.Max(0, nodeCount);
        var adjustedVehicleCount = Math.Max(0, vehicleCount);

        IndexManager = new RoutingIndexManager(adjustedNodeCount, adjustedVehicleCount, startNodes.ToArray(), endNodes.ToArray());
    }

    /// <summary>
    /// Gets or sets the <see cref="RoutingModel"/> of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the routing model is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    protected internal RoutingModel RoutingModel
    {
        get => _routingModel ?? throw new InvalidOperationException("The routing model is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _routingModel = value;
        }
    }

    /// <summary>
    /// Initializes the routing model of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the routing index manager is not initialized.</exception>
    protected internal void InitializeRoutingModel()
    {
        RoutingModel = new RoutingModel(IndexManager);
    }

    /// <summary>
    /// Initializes the routing model of the solver.
    /// </summary>
    /// <param name="indexManager">The routing index manager.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexManager"/> is <see langword="null"/>.</exception>
    protected internal void InitializeRoutingModel(RoutingIndexManager indexManager)
    {
        ArgumentNullException.ThrowIfNull(indexManager, nameof(indexManager));

        RoutingModel = new RoutingModel(indexManager);
    }

    /// <summary>
    /// Gets or sets the <see cref="IReadOnlyDirectedRouteMatrix"/> of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the route matrix is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    protected internal IReadOnlyDirectedRouteMatrix RouteMatrix
    {
        get => _routeMatrix ?? throw new InvalidOperationException("The route matrix is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _routeMatrix = value;
        }
    }

    /// <summary>
    /// Gets or sets the nodes of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the nodes are not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    internal List<Node> Nodes
    {
        get => _nodes ?? throw new InvalidOperationException("The nodes are not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _nodes = value;
        }
    }

    /// <summary>
    /// Gets the number of nodes.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the nodes are not initialized.</exception>
    internal int NodeCount => Nodes.Count;

    /// <summary>
    /// Initializes the nodes of the solver.
    /// </summary>
    /// <param name="capacity">The capacity of the nodes.</param>
    internal void InitializeNodes(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        Nodes = new List<Node>(adjustedCapacity);
    }

    /// <summary>
    /// Gets or sets the vehicles of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles are not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    internal List<DummyVehicle> Vehicles
    {
        get => _vehicles ?? throw new InvalidOperationException("The vehicles are not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _vehicles = value;
        }
    }

    /// <summary>
    /// Gets the number of vehicles.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles are not initialized.</exception>
    internal int VehicleCount => Vehicles.Count;

    /// <summary>
    /// Initializes the vehicles of the solver.
    /// </summary>
    /// <param name="capacity">The capacity of the vehicles.</param>
    protected void InitializeVehicles(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        Vehicles = new List<DummyVehicle>(adjustedCapacity);
    }

    /// <summary>
    /// Gets or sets the vehicles to node store of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles to node store is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    internal Dictionary<DummyVehicle, int> VehiclesToTransitCallbackIndex
    {
        get => _vehiclesToTransitCallbackIndex ?? throw new InvalidOperationException("The vehicles to transit callback index is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _vehiclesToTransitCallbackIndex = value;
        }
    }

    /// <summary>
    /// Initializes the vehicles to transit callback index of the solver.
    /// </summary>
    /// <param name="capacity">The capacity of the vehicles to transit callback index.</param>
    protected void InitializeVehiclesToTransitCallbackIndex(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        VehiclesToTransitCallbackIndex = new Dictionary<DummyVehicle, int>(adjustedCapacity);
    }

    /// <summary>
    /// Gets or sets the vehicles to node store of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles to node store is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    internal Dictionary<DummyVehicle, VehicleNodeStore> VehiclesToNodeStore
    {
        get => _vehiclesToNodeStore ?? throw new InvalidOperationException("The vehicles to node store is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _vehiclesToNodeStore = value;
        }
    }

    /// <summary>
    /// Initializes the vehicles to node store of the solver.
    /// </summary>
    /// <param name="capacity">The capacity of the vehicles to node store.</param>
    protected void InitializeVehiclesToNodeStore(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        VehiclesToNodeStore = new Dictionary<DummyVehicle, VehicleNodeStore>(adjustedCapacity);
    }

    /// <summary>
    /// Gets or sets the shipments to node store of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the shipments to node store is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    internal Dictionary<Shipment, ShipmentNodeStore> ShipmentsToNodeStore
    {
        get => _shipmentsToNodeStore ?? throw new InvalidOperationException("The shipments to node store is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _shipmentsToNodeStore = value;
        }
    }

    /// <summary>
    /// Initializes the shipments to node store of the solver.
    /// </summary>
    /// <param name="capacity">The capacity of the shipments to node store.</param>
    protected void InitializeShipmentsToNodeStore(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        ShipmentsToNodeStore = new Dictionary<Shipment, ShipmentNodeStore>(adjustedCapacity);
    }

    /// <summary>
    /// Resets the solver.
    /// </summary>
    public void Reset()
    {
        // Dispose the GoogleOr-Tools interfaces
        _routingModel?.Dispose();
        _indexManager?.Dispose();

        // Clear the collections
        _nodes?.Clear();
        _vehicles?.Clear();
        _vehiclesToTransitCallbackIndex?.Clear();
        _vehiclesToNodeStore?.Clear();

        // Set everything to null
        _indexManager = null;
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
    /// Checks if the solver is initialized.
    /// </summary>
    /// <returns><see langword="true"/> if the solver is initialized; otherwise, <see langword="false"/>.</returns>
    internal bool IsInitialized()
    {
        return _routingModel is not null
            && _indexManager is not null
            && _routeMatrix is not null
            && _nodes is not null
            && _vehicles is not null
            && _vehiclesToTransitCallbackIndex is not null
            && _vehiclesToNodeStore is not null;
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

        var fromLocation = fromNode.GetLocation();
        var toLocation = toNode.GetLocation();

        // Arbitrary nodes have a distance of 0.
        if (fromLocation is null || toLocation is null)
        {
            return 0;
        }

        return RouteMatrix.GetEdge(fromLocation, toLocation) switch
        {
            DefinedRouteEdge definedRouteEdge => definedRouteEdge.Distance,
            _ => long.MaxValue
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(GoogleOrToolsSolverBase)}";
    }
}
