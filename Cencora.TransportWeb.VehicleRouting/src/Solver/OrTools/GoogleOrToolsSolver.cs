// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Extensions;
using Cencora.TransportWeb.Common.MathUtils;
using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Vehicles;
using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
// ReSharper disable UnusedMember.Local

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// GoogleOrToolsSolver implementation using Google OR-Tools.
/// </summary>
public sealed class GoogleOrToolsSolver : SolverBase, ISolver
{
    private readonly ILogger<GoogleOrToolsSolver> _logger;
    private readonly SolverOptions _options;
    
    /// <summary>
    /// Creates a new instance of the <see cref="GoogleOrToolsSolver"/> class.
    /// </summary>
    /// <param name="options">The options for the solver.</param>
    public GoogleOrToolsSolver(SolverOptions options)
    {
        _logger = NullLogger<GoogleOrToolsSolver>.Instance;
        _options = options;
    }
    
    /// <summary>
    /// Creates a new instance of the <see cref="GoogleOrToolsSolver"/> class.
    /// </summary>
    /// <param name="options">The options for the solver.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
    public GoogleOrToolsSolver(SolverOptions options, ILogger<GoogleOrToolsSolver> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        
        _logger = logger;
        _options = options;
    }
    
    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
        // NOTE: The order of the following method calls is important.
        // Do not modify the order unless you know what you are doing
        // as some methods depend on the successful execution of others.
        
        // Make sure the solver is reset before solving a new problem.
        Reset();
        
        try
        {
            // Initialize the solver with the given problem.
            InitializeSolver(problem);
        
            // Prepare the solver for solving the problem.
            PrepareSolver();
            
            //_logger.LogInformation(GetSolverStateString());
            
            // Solve the problem.
            var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
            searchParameters.LocalSearchMetaheuristic = LocalSearchMetaheuristic.Types.Value.GuidedLocalSearch;
            searchParameters.TimeLimit = new Duration { Seconds = _options.MaximumComputeTime.Seconds };
            using var solution = RoutingModel.SolveWithParameters(searchParameters);
            return CreateOutput(solution);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while solving the vehicle routing problem");
            throw new VehicleRoutingSolverException("An error occurred while solving the vehicle routing problem", e);
        }
    }

    /// <summary>
    /// Initializes the solver.
    /// </summary>
    /// <param name="problem">The problem to solve.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="problem"/> is <see langword="null"/>.</exception>
    private void InitializeSolver(Problem problem)
    {
        // NOTE: The order of the following method calls is important.
        // Do not modify the order unless you know what you are doing
        // as some methods depend on the successful execution of others.
        
        // Make sure the problem is not null.
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));

        // Precalculate the number of nodes, vehicles, and shipments.
        // This way we avoid resizing the lists later on.
        // Each shipment has two nodes (pickup and delivery) and each vehicle has two nodes (start and end).
        var shipmentCount = problem.Shipments.Count;
        var vehicleCount = problem.Vehicles.Sum(v => v.Shifts.Count);
        var nodeCount = shipmentCount * 2 + vehicleCount * 2;

        // Initialize the internal model,
        // which holds the nodes, vehicles and mappings.
        InitializeInternalModel(problem, nodeCount, vehicleCount, shipmentCount);
        
        // Set the route matrix of the solver.
        RouteMatrix = problem.DirectedRouteMatrix;
        
        // Populate the internal model with the given problem.
        SetupShipments(problem.Shipments);
        SetupDummyVehicles(problem.Vehicles);

        // Get vehicle start and end node indices out of the internal model.
        // We need them to tell Google OR-Tools where the vehicles start and end.
        var (vehicleStartNodeIndices, vehicleEndNodeIndices) = GetVehicleNodeIndices();

        // Initialize the Google OR-Tools interfaces.
        InitializeRoutingIndexManager(NodeCount, VehicleCount, vehicleStartNodeIndices, vehicleEndNodeIndices);
        InitializeRoutingModel();
    }

    /// <summary>
    /// Prepares the solver.
    /// </summary>
    /// <remarks>
    /// This method mainly populates the Google OR-Tools routing model with the necessary data
    /// for solving the vehicle routing problem.
    /// </remarks>
    private void PrepareSolver()
    {
        // NOTE: The order of the following method calls is important.
        // Do not modify the order unless you know what you are doing
        // as some methods depend on the successful execution of others.
        
        // Set up the fixed costs of the vehicles.
        SetupVehicleCosts();
        
        // Setup all the solver callbacks.
        SetupArcCostEvaluators();
        SetupTimeCallback();
        //SetupDistanceCallback();
        //SetupWeightCallback();
        //SetupCumulativeWeightCallback();
        SetupIndexCallback();
        
        // Set up the dimensions of the solver.
        SetupTimeDimension();
        //SetupDistanceDimension(); 
        //SetupWeightDimension();
        //SetupCumulativeWeightDimension();
        SetupIndexDimension();
        
        // Add time window constraints to the solver.
        AddTimeWindowConstraints();
        
        // Link pickup and delivery.
        LinkNodes();
        
        // Setup vehicle breaks.
        SetupVehicleBreaks();
        
        // Set the objective functions for the vehicles.
        SetupVehicleObjectiveFunctions();
    }

    /// <summary>
    /// Sets the objective functions for the vehicles.
    /// </summary>
    private void SetupVehicleObjectiveFunctions()
    {
        // We want to maximize the start time to start as late as possible
        // and minimize the end time to finish as early as possible.
        for (var i = 0; i < VehicleCount; i++)
        {
            RoutingModel.AddVariableMaximizedByFinalizer(TimeDimension.CumulVar(RoutingModel.Start(i)));
            RoutingModel.AddVariableMinimizedByFinalizer(TimeDimension.CumulVar(RoutingModel.End(i)));
        }
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
            ShipmentsToNodeStore.Add(shipment, new ShipmentNodeStore(pickupNode, deliveryNode));
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
                VehiclesToNodeStore.Add(dummyVehicle, new VehicleNodeStore(startNode, endNode));
            }
        }
    }

    /// <summary>
    /// Sets up the vehicle costs of the solver.
    /// </summary>
    private void SetupVehicleCosts()
    {
        for (var i = 0; i < VehicleCount; i++)
        {
            var vehicle = Vehicles[i];
            
            var fixedCost = vehicle.FixedCost;
            var baseCost = vehicle.BaseCost;
            var totalCost = fixedCost + baseCost;
            
            RoutingModel.SetFixedCostOfVehicle(totalCost, i);
            
            // If the vehicle has a fixed cost, we need to consider it in the objective
            // function. To do this we set the vehicle as used when empty, as soon
            // as the fixed cost is greater than zero.
            RoutingModel.SetVehicleUsedWhenEmpty(fixedCost > 0, i);
        }
    }

    /// <summary>
    /// Sets up the time callback of the solver.
    /// </summary>
    private void SetupTimeCallback()
    {
        TimeCallback = RoutingModel.RegisterTransitCallback((from, to) =>
        {
            var fromNodeIndex = IndexManager.IndexToNode(from);
            var toNodeIndex = IndexManager.IndexToNode(to);
            var fromNode = Nodes[fromNodeIndex];
            var toNode = Nodes[toNodeIndex];

            var travelTime = GetDuration(fromNode, toNode);
            var handlingTime = fromNode.GetTimeDemand();
            
            return MathUtils.AddOrDefault(travelTime, handlingTime, long.MaxValue);
        });
    }

    /// <summary>
    /// Sets up the time dimension of the solver.
    /// </summary>
    /// <param name="maxSlackTime">The maximum slack time. Defaults to <see cref="long.MaxValue"/>.</param>
    private void SetupTimeDimension(long maxSlackTime = long.MaxValue)
    {
        var adjustedMaxSlackTime = Math.Max(0, maxSlackTime);
        var maxTravelTimes = Vehicles.Select(v => v.MaxDuration).ToArray();
        RoutingModel.AddDimensionWithVehicleCapacity(TimeCallback, adjustedMaxSlackTime, maxTravelTimes, false, TimeDimensionName);
        TimeDimension = RoutingModel.GetMutableDimension(TimeDimensionName);

        for (var i = 0; i < VehicleCount; i++)
        {
            var vehicle = Vehicles[i];
            var timeCost = vehicle.TimeCost;
            TimeDimension.SetSpanCostCoefficientForVehicle(timeCost, i);
        }
    }

    /// <summary>
    /// Sets up the weight callback of the solver.
    /// </summary>
    /// <remarks>
    /// The weight callback is used to calculate the weight demand of a node.
    /// </remarks>
    private void SetupWeightCallback()
    {
        WeightCallback = RoutingModel.RegisterUnaryTransitCallback((node) =>
        {
            var nodeIndex = IndexManager.IndexToNode(node);
            var actualNode = Nodes[nodeIndex];
            
            return actualNode.GetWeightDemand();
        });
    }
    
    /// <summary>
    /// Sets up the cumulative weight callback of the solver.
    /// </summary>
    /// <remarks>
    /// The cumulative weight callback is used to calculate the cumulative weight of a node.
    /// </remarks>
    private void SetupCumulativeWeightCallback()
    {
        CumulativeWeightCallback = RoutingModel.RegisterUnaryTransitCallback((node) =>
        {
            var nodeIndex = IndexManager.IndexToNode(node);
            var actualNode = Nodes[nodeIndex];
            
            // We do not want any negative weights in the cumulative weight dimension,
            // as we want to keep track of the total weight that has been collected.
            return Math.Max(0, actualNode.GetWeightDemand());
        });
    }
    
    /// <summary>
    /// Sets up the weight dimension of the solver.
    /// </summary>
    private void SetupWeightDimension()
    {
        // The weight dimension is only to enforce the maximum weight of the vehicles.
        // We do not set any coefficients for the weight dimension, as this will increase or decrease from node to node,
        // and would not make any sense to set a coefficient for it.
        // Coefficients are set for the cumulative weight dimension.
        var maxWeights = Vehicles.Select(v => v.MaxWeight).ToArray();
        RoutingModel.AddDimensionWithVehicleCapacity(WeightCallback, 0, maxWeights, true, WeightDimensionName);
        WeightDimension = RoutingModel.GetMutableDimension(WeightDimensionName);
    }
    
    /// <summary>
    /// Sets up the cumulative weight dimension of the solver.
    /// </summary>
    /// <remarks>
    /// The cumulative weight dimension is used to keep track of the total weight that has been collected.
    /// </remarks>
    private void SetupCumulativeWeightDimension()
    {
        var maxWeights = Vehicles.Select(v => v.MaxTotalWeight).ToArray();
        RoutingModel.AddDimensionWithVehicleCapacity(CumulativeWeightCallback, 0, maxWeights, true, CumulativeWeightDimensionName);
        CumulativeWeightDimension = RoutingModel.GetMutableDimension(CumulativeWeightDimensionName);

        for (var i = 0; i < VehicleCount; i++)
        {
            var vehicle = Vehicles[i];
            var weightCost = vehicle.WeightCost;
            
            CumulativeWeightDimension.SetSpanCostCoefficientForVehicle(weightCost, i);
        }
    }

    /// <summary>
    /// Sets up the distance callback of the solver.
    /// </summary>
    private void SetupDistanceCallback()
    {
        DistanceCallback = RoutingModel.RegisterTransitCallback((from, to) =>
        {
            var fromNodeIndex = IndexManager.IndexToNode(from);
            var toNodeIndex = IndexManager.IndexToNode(to);
            var fromNode = Nodes[fromNodeIndex];
            var toNode = Nodes[toNodeIndex];
            
            var distance = GetDistance(fromNode, toNode);
            return distance;
        });
    }

    /// <summary>
    /// Sets up the distance dimension of the solver.
    /// </summary>
    private void SetupDistanceDimension()
    {
        var maxDistances = Vehicles.Select(v => v.MaxDistance).ToArray();
        RoutingModel.AddDimensionWithVehicleCapacity(DistanceCallback, 0, maxDistances, true, DistanceDimensionName);
        DistanceDimension = RoutingModel.GetMutableDimension(DistanceDimensionName);
        
        for (var i = 0; i < VehicleCount; i++)
        {
            var vehicle = Vehicles[i];
            var distanceCost = vehicle.DistanceCost;
            
            DistanceDimension.SetSpanCostCoefficientForVehicle(distanceCost, i);
        }
    }
    
    /// <summary>
    /// Sets up the index callback of the solver.
    /// </summary>
    private void SetupIndexCallback()
    {
        IndexCallback = RoutingModel.RegisterUnaryTransitCallback(_ => 1);
    }
    
    /// <summary>
    /// Sets up the index dimension of the solver.
    /// </summary>
    private void SetupIndexDimension()
    {
        RoutingModel.AddDimension(IndexCallback, 0, long.MaxValue, true, IndexDimensionName);
        IndexDimension = RoutingModel.GetMutableDimension(IndexDimensionName);
    }

    /// <summary>
    /// Sets up the arc cost evaluators of the solver.
    /// </summary>
    private void SetupArcCostEvaluators()
    {
        // TODO:
        // The Arc cost evaluator takes into account the cost of traveling from one node to another.
        // We calculate the cost based on the distance and time cost of the vehicle.
        // As we also have dimensions for time and distance, we consider these costs twice,
        // which is not ideal.
        for (var i = 0; i < VehicleCount; i++)
        {
            var vehicle = Vehicles[i];
            var distanceCost = vehicle.DistanceCost;
            var timeCost = vehicle.TimeCost;

            var callback = RoutingModel.RegisterTransitCallback((from, to) =>
            {
                var fromNodeIndex = IndexManager.IndexToNode(from);
                var toNodeIndex = IndexManager.IndexToNode(to);
                var fromNode = Nodes[fromNodeIndex];
                var toNode = Nodes[toNodeIndex];
                
                var distance = GetDistance(fromNode, toNode);
                var duration = GetDuration(fromNode, toNode);
                
                var totalDistanceCost = MathUtils.MultiplyOrDefault(distance, distanceCost, long.MaxValue);
                var totalTimeCost = MathUtils.MultiplyOrDefault(duration, timeCost, long.MaxValue);
                
                return MathUtils.AddOrDefault(totalDistanceCost, totalTimeCost, long.MaxValue);
            });
            
            RoutingModel.SetArcCostEvaluatorOfVehicle(callback, i);
        }
    }
    
    /// <summary>
    /// Adds the time window constraints to the solver.
    /// </summary>
    /// <remarks>
    /// The time window constraints are used to ensure that the vehicles visit the nodes within the given time windows.
    /// </remarks>
    private void AddTimeWindowConstraints()
    {
        // Add time window constraints for the nodes.
        for (var i = 0; i < NodeCount; i++)
        {
            var node = Nodes[i];
            var range = node.GetTimeWindow();
            var index = IndexManager.NodeToIndex(i);
            
            // Copy the slack var to the assignment,
            // so we can later retrieve the slack time of the node.
            RoutingModel.AddToAssignment(TimeDimension.SlackVar(index));
            
            // Skip the start and end nodes.
            // Start nodes get their time windows from the vehicles
            // and end nodes do not have time windows.
            if (RoutingModel.IsStart(index) || RoutingModel.IsEnd(index))
            {
                continue;
            }
            
            // Skip nodes without time windows.
            if (range is null)
            {
                continue;
            }
            
            TimeDimension.CumulVar(index).SetRange(range.Value.Min, range.Value.Max);
        }
        
        // Add time window constraints for the vehicles.
        for (var i = 0; i < VehicleCount; i++)
        {
            var vehicle = Vehicles[i];
            var shiftTimeWindow = vehicle.AvailableTimeWindow;
            
            var index = RoutingModel.Start(i);
            TimeDimension.CumulVar(index).SetRange(shiftTimeWindow.Min, shiftTimeWindow.Max);
            RoutingModel.AddToAssignment(TimeDimension.SlackVar(index));
        }
    }

    /// <summary>
    /// Links the nodes of the solver.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the solver is <see langword="null"/>.</exception>
    private void LinkNodes()
    {
        foreach (var store in ShipmentsToNodeStore.Values)
        {
            var pickupNode = store.Pickup;
            var deliveryNode = store.Delivery;
            
            var pickupIndex = IndexManager.NodeToIndex(pickupNode);
            var deliveryIndex = IndexManager.NodeToIndex(deliveryNode);
            
            RoutingModel.AddPickupAndDelivery(pickupIndex, deliveryIndex);
            // The following line adds the requirement that each item must be picked up and delivered by the same vehicle.
            Solver.Add(Solver.MakeEquality(RoutingModel.VehicleVar(pickupIndex), RoutingModel.VehicleVar(deliveryIndex)));
            // Finally, we add the obvious requirement that each item must be picked up before it is delivered. 
            Solver.Add(Solver.MakeLessOrEqual(IndexDimension.CumulVar(pickupIndex), IndexDimension.CumulVar(deliveryIndex)));
        }
    }

    /// <summary>
    /// Sets up the vehicle breaks of the solver.
    /// </summary>
    private void SetupVehicleBreaks()
    {
        var nodeTimeDemands = GetNodeTimeDemands();

        var breakIndex = 0;
        for (var i = 0; i < VehicleCount; i++)
        {
            var vehicle = Vehicles[i];

            var intervalVars = new IntervalVarVector();
            foreach (var currentBreak in vehicle.Breaks)
            {
                var breakTimeWindow = currentBreak.AllowedTimeWindow;
                var breakDuration = currentBreak.Duration;
                var breakOptional = currentBreak.Option == BreakOption.Optional;
                var breakName = $"Break_{breakIndex++}";
                
                var intervalVar = Solver.MakeFixedDurationIntervalVar(breakTimeWindow.Min, breakTimeWindow.Max, breakDuration, breakOptional, breakName);
                intervalVars.Add(intervalVar);
            }
            
            TimeDimension.SetBreakIntervalsOfVehicle(intervalVars, i, nodeTimeDemands);
        }
    }
    private long[] GetNodeTimeDemands()
    {
        // https://github.com/google/or-tools/issues/2578
        var count = RoutingModel.Size();
        var timeDemands = new long[count];
        for (var i = 0; i < count; i++)
        {
            var nodeIndex = IndexManager.IndexToNode(i);
            var node = Nodes[nodeIndex];
            timeDemands[i] = node.GetTimeDemand();
        }
        return timeDemands;
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

        // We need the number of shifts to adjust the fixed and base costs.
        var shiftCount = vehicle.Shifts.Count;

        var adjustedFixedCost = MathUtils.DivideOrDefault(vehicle.FixedCost ?? 0, shiftCount, 0, 0);
        var adjustedBaseCost = MathUtils.DivideOrDefault(vehicle.BaseCost ?? 0, shiftCount, 0, 0);
        var fixedCost = MathUtils.AddOrDefault(adjustedBaseCost, shift.FixedCost ?? 0, long.MaxValue);
        var baseCost = MathUtils.AddOrDefault(adjustedFixedCost, shift.BaseCost ?? 0, long.MaxValue);
        var distanceCost = MathUtils.AddOrDefault(vehicle.DistanceCost ?? 0, shift.DistanceCost ?? 0, long.MaxValue);
        var timeCost = MathUtils.AddOrDefault(vehicle.TimeCost ?? 0, shift.TimeCost ?? 0, long.MaxValue);
        var maxDuration = Math.Min(shift.MaxDuration ?? long.MaxValue, shift.ShiftTimeWindow.Difference);
        
        return new DummyVehicleBuilder(GetNextVehicleIndex(), vehicle, shift)
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
        
        return new ShipmentNode(GetNextNodeIndex(), shipment, location, nodeType);
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
        
        return new VehicleNode(GetNextNodeIndex(), dummyVehicle, location, nodeType);
    }

    /// <summary>
    /// Creates a <see cref="SolverOutput"/> from an assignment.
    /// </summary>
    /// <param name="assignment">The assignment.</param>
    /// <returns>The output of the solver.</returns>
    private SolverOutput CreateOutput(in Assignment? assignment)
    {
        // If the assignment is null, we did not find a solution.
        if (assignment is null)
        {
            return new SolverOutput();
        }
        
        // Store the vehicle shifts in a dictionary,
        // so we can later convert them into vehicle plans.
        var vehicleShifts = new Dictionary<Vehicle, List<VehicleShift>>(SolverProblem.VehicleCount);
        
        // Iterate over all vehicles we need to create a plan for.
        for (var i = 0; i < VehicleCount; i++)
        {
            var currentDummyVehicle = Vehicles[i];
            var currentShift = currentDummyVehicle.Shift;
            var currentVehicle = currentDummyVehicle.Vehicle;

            var stops = CollectStopsForVehicle(assignment, i, currentVehicle);
            var trips = CollectTripsForVehicle(stops, currentVehicle);
            
            // Make sure the stops and trips are sorted by their index.
            stops.Sort();
            trips.Sort();
        
            // Create a vehicle shift for the current vehicle
            var vehicleShift = new VehicleShift(currentVehicle, currentShift, stops, trips);

            // Check if we already have a list of shifts for the current vehicle.
            // If not, we create a new list and add the current shift to it.
            // If we already have a list of shifts, we add the current shift to it.
            if (!vehicleShifts.TryGetValue(currentVehicle, out var shifts))
            {
                shifts = new List<VehicleShift>();
                vehicleShifts[currentVehicle] = shifts;
            }
            shifts.Add(vehicleShift);
        }
        
        // Create the vehicle plans from the vehicle shifts.
        // Each vehicle plan is created from a vehicle and its shifts.
        var vehiclePlans = vehicleShifts.Select(kvp => new VehiclePlan(kvp.Key, kvp.Value)).ToHashSet();
        var solution = new Solution(vehiclePlans);
        return new SolverOutput(solution);
    }
    
    /// <summary>
    /// Collects the stops for a vehicle from an assignment.
    /// </summary>
    /// <param name="assignment">The assignment.</param>
    /// <param name="vehicleIndex">The index of the vehicle.</param>
    /// <param name="currentVehicle">The current vehicle.</param>
    /// <returns>The stops for the vehicle.</returns>
    private List<VehicleStop> CollectStopsForVehicle(in Assignment assignment, int vehicleIndex, in Vehicle currentVehicle)
    {
        ArgumentNullException.ThrowIfNull(assignment, nameof(assignment));
        ArgumentOutOfRangeException.ThrowIfNegative(vehicleIndex, nameof(vehicleIndex));
        ArgumentNullException.ThrowIfNull(currentVehicle, nameof(currentVehicle));
        
        var stops = new List<MutableVehicleStop>();
        var currentIndex = RoutingModel.Start(vehicleIndex);
        
        // Keep track of the stop index.
        // For cleaner code, we use a function to get the next stop index.
        var stopIndex = 1;
        var getNextStopIndex = new Func<int>(() => stopIndex++);
        
        while (!RoutingModel.IsEnd(currentIndex))
        {
            var currentNodeIndex = IndexManager.IndexToNode(currentIndex);
            var currentNode = Nodes[currentNodeIndex];
            var currentLocation = currentNode.GetLocation();
            
            // Move to the next index.
            if (currentLocation is null)
            {
                currentIndex = GetNextRoutingIndex(assignment, currentIndex);
                continue;
            }

            var (arrivalWindow, waitingWindow, departureWindow) = GetTimeWindows(currentIndex, assignment);
        
            if (IsSameLocationAsLastStop(stops, currentLocation))
            {
                MergeStopWithLast(stops.Last(), currentNode, arrivalWindow, waitingWindow, departureWindow);
            }
            else
            {
                stops.Add(CreateNewStop(getNextStopIndex(), currentNode, currentVehicle, currentLocation, arrivalWindow, waitingWindow, departureWindow));
            }
            
            // Move to the next index.
            currentIndex = GetNextRoutingIndex(assignment, currentIndex);
        }

        return stops.ConvertAll(s => s.ToVehicleStop());
    }

    /// <summary>
    /// Gets the next routing index.
    /// </summary>
    /// <param name="assignment">The assignment.</param>
    /// <param name="currentIndex">The current index.</param>
    /// <returns>The next routing index.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="assignment"/> is <see langword="null"/>.</exception>
    private long GetNextRoutingIndex(Assignment assignment, long currentIndex)
    {
        ArgumentNullException.ThrowIfNull(assignment, nameof(assignment));
        
        return assignment.Value(RoutingModel.NextVar(currentIndex));
    }
    
    /// <summary>
    /// Creates the trips for a vehicle from a list of stops.
    /// </summary>
    /// <param name="stops">The list of stops.</param>
    /// <param name="currentVehicle">The current vehicle.</param>
    /// <returns>The trips for the vehicle.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stops"/> or <paramref name="currentVehicle"/> is <see langword="null"/>.</exception>
    private List<VehicleTrip> CollectTripsForVehicle(in List<VehicleStop> stops, in Vehicle currentVehicle)
    {
        ArgumentNullException.ThrowIfNull(stops, nameof(stops));
        ArgumentNullException.ThrowIfNull(currentVehicle, nameof(currentVehicle));

        // Preallocate the list of trips.
        var trips = new List<VehicleTrip>(Math.Max(0, stops.Count - 1));
        
        // Keep track of the trip index.
        // Fo cleaner code, we use a function to get the next trip index.
        var tripIndex = 1;
        var getNextTripIndex = new Func<int>(() => tripIndex++);
        
        // Iterate over all stops and create trips between them.
        for (var i = 0; i < stops.Count - 1; i++)
        {
            var from = stops[i];
            var to = stops[i + 1];
            var fromLocation = from.Location;
            var toLocation = to.Location;

            var (distance, duration) = RouteMatrix.GetEdge(fromLocation, toLocation) switch
            {
                DefinedRouteEdge edge => (edge.Distance, edge.Duration),
                _ => (long.MaxValue, long.MaxValue)
            };
            
            var tripDistanceCost = MathUtils.MultiplyOrDefault(distance, currentVehicle.DistanceCost ?? 0, long.MaxValue);
            var tripTimeCost = MathUtils.MultiplyOrDefault(duration, currentVehicle.TimeCost ?? 0, long.MaxValue);

            var trip = new VehicleTrip(getNextTripIndex(), currentVehicle, fromLocation, toLocation, distance, duration,
                from.DepartureTimeWindow, to.ArrivalTimeWindow, tripDistanceCost, tripTimeCost);
            
            trips.Add(trip);
        }
        
        return trips;
    }
    
    
    /// <summary>
    /// Retrieves the time windows (arrival, waiting, and departure) for a given index.
    /// </summary>
    /// <param name="index">The index to retrieve the time windows for.</param>
    /// <param name="assignment">The assignment to retrieve the time windows from.</param>
    /// <returns>The time windows (arrival, waiting, and departure) for the given index.</returns>
    private (ValueRange arrival, ValueRange waiting, ValueRange departure) GetTimeWindows(long index, in Assignment assignment)
    {
        ArgumentNullException.ThrowIfNull(assignment, nameof(assignment));
        
        var arrivalTimeVar = TimeDimension.CumulVar(index);
        var earliestArrival = assignment.Min(arrivalTimeVar);
        var latestArrival = assignment.Max(arrivalTimeVar);

        var waitingTimeVar = TimeDimension.SlackVar(index);
        var minimumWaitingTime = assignment.Min(waitingTimeVar);
        var maximumWaitingTime = assignment.Max(waitingTimeVar);
        
        var nodeIndex = IndexManager.IndexToNode(index);
        var handlingTime = Nodes[nodeIndex].GetTimeDemand();
        var totalMinimumWaiting = minimumWaitingTime + handlingTime;
        var totalMaximumWaiting = maximumWaitingTime + handlingTime;
        
        var arrivalWindow = new ValueRange(earliestArrival, latestArrival);
        var waitingWindow = new ValueRange(totalMinimumWaiting, totalMaximumWaiting);
        var departureWindow = new ValueRange(earliestArrival + handlingTime, latestArrival + handlingTime);

        return (arrivalWindow, waitingWindow, departureWindow);
    }
    
    /// <summary>
    /// Checks if the current location matches the last stop's location.
    /// </summary>
    /// <param name="stops">The list of stops.</param>
    /// <param name="currentLocation">The current location.</param>
    /// <returns><see langword="true"/> if the current location matches the last stop's location; otherwise, <see langword="false"/>.</returns>
    private static bool IsSameLocationAsLastStop(in IReadOnlyList<MutableVehicleStop> stops, Location? currentLocation)
    {
        if (stops.Count == 0)
        {
            return false;
        }

        var lastLocation = stops.Last().Location;
        return AreLocationsEqual(lastLocation, currentLocation);
    }
    
    /// <summary>
    /// Creates a new stop for a vehicle.
    /// </summary>
    /// <param name="stopIndex">The index of the stop.</param>
    /// <param name="currentNode">The current node.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="location">The location of the stop.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the stop.</param>
    /// <param name="waitingTimeWindow">The waiting time window of the stop.</param>
    /// <param name="departureTimeWindow">The departure time window of the stop.</param>
    /// <returns>The new stop for the vehicle.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="stopIndex"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="currentNode"/>, <paramref name="vehicle"/>, or <paramref name="location"/> is <see langword="null"/>.</exception>
    private static MutableVehicleStop CreateNewStop(int stopIndex, Node currentNode, Vehicle vehicle, Location location, ValueRange arrivalTimeWindow, ValueRange waitingTimeWindow, ValueRange departureTimeWindow)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(stopIndex, nameof(stopIndex));
        ArgumentNullException.ThrowIfNull(currentNode, nameof(currentNode));
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(location, nameof(location));
        
        var pickups = new HashSet<Shipment>();
        var deliveries = new HashSet<Shipment>();
        pickups.AddIfNotNull(currentNode.GetPickup());
        deliveries.AddIfNotNull(currentNode.GetDelivery());

        return new MutableVehicleStop(stopIndex, location, vehicle, pickups, deliveries, arrivalTimeWindow, departureTimeWindow, waitingTimeWindow);
    }
    
    /// <summary>
    /// Merges the current node information with the last stop.
    /// </summary>
    /// <param name="lastStop">The last stop.</param>
    /// <param name="currentNode">The current node.</param>
    /// <param name="arrivalTimeWindow">The arrival time window of the current node.</param>
    /// <param name="waitingTimeWindow">The waiting time window of the current node.</param>
    /// <param name="departureTimeWindow">The departure time window of the current node.</param>
    private static void MergeStopWithLast(MutableVehicleStop lastStop, Node currentNode, ValueRange arrivalTimeWindow, ValueRange waitingTimeWindow, ValueRange departureTimeWindow)
    {
        lastStop.ArrivalTimeWindow = CombineTimeWindows(lastStop.ArrivalTimeWindow, arrivalTimeWindow);
        lastStop.WaitingTime = CombineTimeWindows(lastStop.WaitingTime, waitingTimeWindow);
        lastStop.DepartureTimeWindow = CombineTimeWindows(lastStop.DepartureTimeWindow, departureTimeWindow);
        lastStop.Pickups.AddIfNotNull(currentNode.GetPickup());
        lastStop.Deliveries.AddIfNotNull(currentNode.GetDelivery());
    }
    
    /// <summary>
    /// Combines two specified time windows.
    /// </summary>
    /// <param name="left">The first time window to combine.</param>
    /// <param name="right">The second time window to combine.</param>
    /// <returns>The combined time window.</returns>
    /// <remarks>
    /// It uses the maximum of the minimum values and the minimum of the maximum values of the two time windows.
    /// </remarks>
    private static ValueRange CombineTimeWindows(ValueRange left, ValueRange right)
    {
        return new ValueRange(Math.Max(left.Min, right.Min), Math.Min(left.Max, right.Max));
    }
    
    /// <summary>
    /// Helper method to compare two locations using LocationNullComparer.
    /// </summary>
    /// <param name="left">The first location to compare.</param>
    /// <param name="right">The second location to compare.</param>
    /// <returns><see langword="true"/> if the two locations are equal; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// If both locations are <see langword="null"/>, this method returns <see langword="false"/>.
    /// </remarks>
    private static bool AreLocationsEqual(Location? left, Location? right)
    {
        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return "Google OR-Tools Vehicle Routing GoogleOrToolsSolver";
    }
}
