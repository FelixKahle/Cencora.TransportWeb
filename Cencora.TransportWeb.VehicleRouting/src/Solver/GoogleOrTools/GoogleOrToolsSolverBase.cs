// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;
using Google.OrTools.ConstraintSolver;

// ReSharper disable MemberCanBePrivate.Global

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
    private protected const string TimeDimensionName = "TimeDimension";
    private protected const string WeightDimensionName = "WeightDimension";
    private protected const string DistanceDimensionName = "DistanceDimension";
    
    // OR-Tools interfaces
    private RoutingIndexManager? _indexManager;
    private RoutingModel? _routingModel;
    
    // Internal state
    private List<Node>? _nodes;
    private List<DummyVehicle>? _vehicles;
    private Dictionary<DummyVehicle, VehicleNodeStore>? _vehiclesToNodeStore;
    private Dictionary<Shipment, ShipmentNodeStore>? _shipmentsToNodeStore;
    private IReadOnlyDirectedRouteMatrix? _routeMatrix;
    
    // Time
    private int _timeCallback = -1;
    private RoutingDimension? _timeDimension;
    
    // Weight
    private int _weightCallback = -1;
    private RoutingDimension? _weightDimension;
    
    // Distance
    private int _distanceCallback = -1;
    private RoutingDimension? _distanceDimension;

    /// <summary>
    /// Initializes the internal model of the solver.
    /// </summary>
    /// <param name="nodeCount">The number of nodes.</param>
    /// <param name="vehicleCount">The number of vehicles.</param>
    /// <param name="shipmentCount">The number of shipments.</param>
    /// <remarks>
    /// Internally calls the initialization methods for the nodes, vehicles, vehicles to transit callback index,
    /// vehicles to node store, and shipments to node store.
    /// </remarks>
    private protected void InitializeInternalModel(int nodeCount = 0, int vehicleCount = 0, int shipmentCount = 0)
    {
        InitializeNodes(nodeCount);
        InitializeVehicles(vehicleCount);
        InitializeVehiclesToNodeStore(vehicleCount);
        InitializeShipmentsToNodeStore(shipmentCount);
    }

    /// <summary>
    /// Gets or sets the <see cref="RoutingIndexManager"/> of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the routing index manager is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected RoutingIndexManager IndexManager
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
    private protected void InitializeRoutingIndexManager(int nodeCount, int vehicleCount, int[] startNodes, int[] endNodes)
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
    private protected RoutingModel RoutingModel
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
    private protected void InitializeRoutingModel()
    {
        RoutingModel = new RoutingModel(IndexManager);
    }

    /// <summary>
    /// Initializes the routing model of the solver.
    /// </summary>
    /// <param name="indexManager">The routing index manager.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexManager"/> is <see langword="null"/>.</exception>
    private protected void InitializeRoutingModel(RoutingIndexManager indexManager)
    {
        ArgumentNullException.ThrowIfNull(indexManager, nameof(indexManager));

        RoutingModel = new RoutingModel(indexManager);
    }

    /// <summary>
    /// Gets or sets the <see cref="IReadOnlyDirectedRouteMatrix"/> of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the route matrix is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected IReadOnlyDirectedRouteMatrix RouteMatrix
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
    private protected List<Node> Nodes
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
    private void InitializeNodes(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        Nodes = new List<Node>(adjustedCapacity);
    }

    /// <summary>
    /// Gets or sets the vehicles of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles are not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected List<DummyVehicle> Vehicles
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
    private void InitializeVehicles(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        Vehicles = new List<DummyVehicle>(adjustedCapacity);
    }

    /// <summary>
    /// Gets or sets the vehicles to node store of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles to node store is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected Dictionary<DummyVehicle, VehicleNodeStore> VehiclesToNodeStore
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
    private void InitializeVehiclesToNodeStore(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        VehiclesToNodeStore = new Dictionary<DummyVehicle, VehicleNodeStore>(adjustedCapacity);
    }

    /// <summary>
    /// Gets or sets the shipments to node store of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the shipments to node store is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected Dictionary<Shipment, ShipmentNodeStore> ShipmentsToNodeStore
    {
        get => _shipmentsToNodeStore ?? throw new InvalidOperationException("The shipments to node store is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _shipmentsToNodeStore = value;
        }
    }
    
    /// <summary>
    /// Gets or sets the time callback of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the time callback is not initialized.</exception>
    private protected int TimeCallback
    {
        get => _timeCallback >= 0 ? _timeCallback : throw new InvalidOperationException("The time callback is not initialized.");
        set => _timeCallback = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), value, "The time callback must be greater or equal to 0.");
    }
    
    /// <summary>
    /// Gets or sets the time dimension of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the time dimension is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected RoutingDimension TimeDimension
    {
        get => _timeDimension ?? throw new InvalidOperationException("The time dimension is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _timeDimension = value;
        }
    }
    
    /// <summary>
    /// Gets or sets the weight dimension of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the weight dimension is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected int WeightCallback
    {
        get => _weightCallback >= 0 ? _weightCallback : throw new InvalidOperationException("The weight callback is not initialized.");
        set => _weightCallback = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), value, "The weight callback must be greater or equal to 0.");
    }
    
    /// <summary>
    /// Gets or sets the weight dimension of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the weight dimension is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected RoutingDimension WeightDimension
    {
        get => _weightDimension ?? throw new InvalidOperationException("The weight dimension is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _weightDimension = value;
        }
    }

    /// <summary>
    /// Gets or sets the distance dimension of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the distance dimension is not initialized.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is less than 0.</exception>
    private protected int DistanceCallback
    {
        get => _distanceCallback >= 0 ? _distanceCallback : throw new InvalidOperationException("The distance callback is not initialized.");
        set => _distanceCallback = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), value, "The distance callback must be greater or equal to 0.");
    }

    /// <summary>
    /// Gets or sets the distance dimension of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the distance dimension is not initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the value is <see langword="null"/>.</exception>
    private protected RoutingDimension DistanceDimension
    {
        get => _distanceDimension ?? throw new InvalidOperationException("The distance dimension is not initialized.");
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _distanceDimension = value;
        }
    }

    /// <summary>
    /// Initializes the shipments to node store of the solver.
    /// </summary>
    /// <param name="capacity">The capacity of the shipments to node store.</param>
    private void InitializeShipmentsToNodeStore(int capacity = 0)
    {
        var adjustedCapacity = Math.Max(0, capacity);
        ShipmentsToNodeStore = new Dictionary<Shipment, ShipmentNodeStore>(adjustedCapacity);
    }

    /// <summary>
    /// Resets the solver.
    /// </summary>
    protected void Reset()
    {
        _routingModel?.Dispose();
        _routingModel = null;
        
        _indexManager?.Dispose();
        _indexManager = null;
        
        _timeDimension?.Dispose();
        _timeDimension = null;
        _timeCallback = -1;
        
        _weightDimension?.Dispose();
        _weightDimension = null;
        _weightCallback = -1;
        
        _nodes?.Clear();
        _nodes = null;
        
        _vehicles?.Clear();
        _vehicles = null;
        
        _vehiclesToNodeStore?.Clear();
        _vehiclesToNodeStore = null;
        
        _shipmentsToNodeStore?.Clear();
        _shipmentsToNodeStore = null;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Reset();
    }

    /// <summary>
    /// Returns the distance between two nodes.
    /// </summary>
    /// <param name="fromNode">The node to start from.</param>
    /// <param name="toNode">The node to end at.</param>
    /// <returns>The distance between the two nodes.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromNode"/> or <paramref name="toNode"/> is <see langword="null"/>.</exception>
    private protected long GetDistance(Node fromNode, Node toNode)
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

    /// <summary>
    /// Returns the time between two nodes.
    /// </summary>
    /// <param name="fromNode">The node to start from.</param>
    /// <param name="toNode">The node to end at.</param>
    /// <returns>The time between the two nodes.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromNode"/> or <paramref name="toNode"/> is <see langword="null"/>.</exception>
    private protected long GetDuration(Node fromNode, Node toNode)
    {
        ArgumentNullException.ThrowIfNull(fromNode, nameof(fromNode));
        ArgumentNullException.ThrowIfNull(toNode, nameof(toNode));
        
        var fromLocation = fromNode.GetLocation();
        var toLocation = toNode.GetLocation();
        
        if (fromLocation is null || toLocation is null)
        {
            return 0;
        }
        
        return RouteMatrix.GetEdge(fromLocation, toLocation) switch
        {
            DefinedRouteEdge definedRouteEdge => definedRouteEdge.Duration,
            _ => long.MaxValue
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(GoogleOrToolsSolverBase)}";
    }
}
