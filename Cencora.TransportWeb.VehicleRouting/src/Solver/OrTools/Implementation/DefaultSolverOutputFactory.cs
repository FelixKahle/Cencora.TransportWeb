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
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Output;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Vehicles;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Implementation;

/// <summary>
/// Represents the default implementation of the output factory.
/// </summary>
internal class DefaultSolverOutputFactory : IOutputFactory
{
    /// <summary>
    /// The time dimension.
    /// </summary>
    private SolverDimension TimeDimension { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultSolverOutputFactory"/> class.
    /// </summary>
    /// <param name="timeDimension">The time dimension.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="timeDimension"/> is <see langword="null"/>.</exception>
    internal DefaultSolverOutputFactory(SolverDimension timeDimension)
    {
        ArgumentNullException.ThrowIfNull(timeDimension, nameof(timeDimension));
        
        TimeDimension = timeDimension;
    }
    
    /// <inheritdoc/>
    public SolverOutput CreateOutput(Problem problem, SolverState state, Assignment? assignment)
    {
        // If the assignment is null, we did not find a solution.
        if (assignment is null)
        {
            return new SolverOutput();
        }
        
        // Store the vehicle shifts in a dictionary,
        // so we can later convert them into vehicle plans.
        var vehicleShifts = new Dictionary<Vehicle, List<VehicleShift>>(problem.VehicleCount);
        
        // Iterate over all vehicles we need to create a plan for.
        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            var currentDummyVehicle = state.SolverModel.Vehicles[i];
            var currentShift = currentDummyVehicle.Shift;
            var currentVehicle = currentDummyVehicle.Vehicle;

            var stops = CollectStopsForVehicle(state, assignment, i, currentVehicle);
            var trips = CollectTripsForVehicle(problem.DirectedRouteMatrix, stops, currentVehicle);
            
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
    /// <param name="state">The state.</param>
    /// <param name="assignment">The assignment.</param>
    /// <param name="vehicleIndex">The index of the vehicle.</param>
    /// <param name="currentVehicle">The current vehicle.</param>
    /// <returns>The stops for the vehicle.</returns>
    private List<VehicleStop> CollectStopsForVehicle(SolverState state, Assignment assignment, int vehicleIndex, Vehicle currentVehicle)
    {
        ArgumentNullException.ThrowIfNull(assignment, nameof(assignment));
        ArgumentOutOfRangeException.ThrowIfNegative(vehicleIndex, nameof(vehicleIndex));
        ArgumentNullException.ThrowIfNull(currentVehicle, nameof(currentVehicle));
        
        var stops = new List<MutableVehicleStop>();
        var currentIndex = state.SolverInterface.RoutingModel.Start(vehicleIndex);
        
        // Keep track of the stop index.
        // For cleaner code, we use a function to get the next stop index.
        var stopIndex = 1;
        var getNextStopIndex = new Func<int>(() => stopIndex++);
        
        while (!state.SolverInterface.RoutingModel.IsEnd(currentIndex))
        {
            var currentNode = state.SolverInterface.IndexToNode(currentIndex);
            var currentLocation = currentNode.GetLocation();
            
            // Move to the next index.
            if (currentLocation is null)
            {
                currentIndex = GetNextRoutingIndex(state, assignment, currentIndex);
                continue;
            }

            var (arrivalWindow, waitingWindow, departureWindow) = GetTimeWindows(state, assignment, currentIndex);
        
            if (IsSameLocationAsLastStop(stops, currentLocation))
            {
                MergeStopWithLast(stops.Last(), currentNode, arrivalWindow, waitingWindow, departureWindow);
            }
            else
            {
                stops.Add(CreateNewStop(getNextStopIndex(), currentNode, currentVehicle, currentLocation, arrivalWindow, waitingWindow, departureWindow));
            }
            
            // Move to the next index.
            currentIndex = GetNextRoutingIndex(state, assignment, currentIndex);
        }

        return stops.ConvertAll(s => s.ToVehicleStop());
    }
    
    /// <summary>
    /// Creates the trips for a vehicle from a list of stops.
    /// </summary>
    /// <param name="routeMatrix">The route matrix.</param>
    /// <param name="stops">The list of stops.</param>
    /// <param name="currentVehicle">The current vehicle.</param>
    /// <returns>The trips for the vehicle.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stops"/> or <paramref name="currentVehicle"/> is <see langword="null"/>.</exception>
    private List<VehicleTrip> CollectTripsForVehicle(IReadOnlyDirectedRouteMatrix routeMatrix, List<VehicleStop> stops, in Vehicle currentVehicle)
    {
        ArgumentNullException.ThrowIfNull(routeMatrix, nameof(routeMatrix));
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

            var (distance, duration) = routeMatrix.GetEdge(fromLocation, toLocation) switch
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
    /// Gets the next routing index.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="assignment">The assignment.</param>
    /// <param name="currentIndex">The current index.</param>
    /// <returns>The next routing index.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> or <paramref name="assignment"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="currentIndex"/> is negative.</exception>
    private long GetNextRoutingIndex(SolverState state, Assignment assignment, long currentIndex)
    {
        ArgumentNullException.ThrowIfNull(assignment, nameof(assignment));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentOutOfRangeException.ThrowIfNegative(currentIndex, nameof(currentIndex));
        
        return assignment.Value(state.SolverInterface.RoutingModel.NextVar(currentIndex));
    }
    
    /// <summary>
    /// Gets the time windows for a given index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="assignment">The assignment.</param>
    /// <returns>The time windows.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="assignment"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is negative.</exception>
    private (ValueRange arrival, ValueRange waiting, ValueRange departure) GetTimeWindows(SolverState state, Assignment assignment, long index)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(assignment, nameof(assignment));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        
        var arrivalTimeVar = TimeDimension.Dimension.CumulVar(index);
        var earliestArrival = assignment.Min(arrivalTimeVar);
        var latestArrival = assignment.Max(arrivalTimeVar);

        var waitingTimeVar = TimeDimension.Dimension.SlackVar(index);
        var minimumWaitingTime = assignment.Min(waitingTimeVar);
        var maximumWaitingTime = assignment.Max(waitingTimeVar);
        
        var nodeIndex = state.SolverInterface.IndexToNode(index);
        var handlingTime = state.SolverModel.Nodes[nodeIndex].GetTimeDemand();
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
}