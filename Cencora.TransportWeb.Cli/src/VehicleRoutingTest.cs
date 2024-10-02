// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

namespace Cencora.TransportWeb.Cli;

public class VehicleRoutingTest
{
    private const int LocationCount = 20;
    private const int ShipmentCount = 2;
    private const int VehicleCount = 5;
    
    private readonly ISolver _solver = new GoogleOrToolsSolver(new GoogleOrToolsSolverOptions(TimeSpan.FromSeconds(10)));
    private readonly Random _random = new Random();

    public void Run()
    {
        var problem = BuildProblem();
        var solution = _solver.Solve(problem);
    }

    private Problem BuildProblem()
    {
        var locations = BuildLocations();
        var vehicles = BuildVehicles(locations);
        var shipments = BuildShipments(locations);
        var routeMatrix = BuildRouteMatrix(locations);

        var problem = new ProblemBuilder()
            .WithRouteMatrix(routeMatrix)
            .WithLocations(locations)
            .WithVehicles(vehicles)
            .WithMaxVehicleWaitingTime(long.MaxValue)
            .WithShipments(shipments)
            .Build();
        
        return problem;
    }

    private IReadOnlySet<Shipment> BuildShipments(IReadOnlySet<Location> locations)
    {
        var shipments = new HashSet<Shipment>(ShipmentCount);
        var locationList = locations.ToList(); // Convert set to list for easy indexing

        for (var i = 0; i < ShipmentCount; i++)
        {
            // Randomly select a pickup location
            var pickupLocation = locationList[_random.Next(locationList.Count)];
            
            var deliveryLocation = locationList
                .Where(loc => loc != pickupLocation)
                .OrderBy(_ => _random.Next()) // Shuffle the remaining locations
                .First(); // Pick the first one

            // Build the shipment
            var shipment = new ShipmentBuilder()
                .WithAutomaticId()
                .WithWeight(0)
                .WithPickupHandlingTime(0)
                .WithDeliveryHandlingTime(0)
                .WithoutPickupTimeWindow()
                .WithoutDeliveryTimeWindow()
                .WithPickupLocation(pickupLocation)
                .WithDeliveryLocation(deliveryLocation)
                .Build();

            shipments.Add(shipment);
        }

        return shipments;
    }
    
    private IReadOnlySet<Vehicle> BuildVehicles(IReadOnlySet<Location> locations)
    {
        var vehicles = new HashSet<Vehicle>(VehicleCount);
        
        for (var i = 0; i < VehicleCount; i++)
        {
            var shifts = BuildShifts(2, locations);
            
            var vehicle = new VehicleBuilder()
                .WithAutomaticId()
                .WithFixedCost(_random.Next(1, 100))
                .WithBaseCost(_random.Next(1, 100))
                .WithDistanceCost(_random.Next(1, 100))
                .WithTimeCost(_random.Next(1, 100))
                .WithWaitingTimeCost(_random.Next(1, 100))
                .WithWeightCost(_random.Next(1, 100))
                .WithCostPerWeightDistance(_random.Next(1, 100))
                .WithMaxWeight(10000000)
                .WithShifts(shifts)
                .Build();
            
            vehicles.Add(vehicle);
        }
        
        return vehicles;
    }

    private IReadOnlySet<Shift> BuildShifts(int count, IReadOnlySet<Location> locations)
    {
        var shifts = new HashSet<Shift>(count);
        var locationList = locations.ToList(); // Convert set to list for easy indexing
        
        for (var i = 0; i < count; i++)
        {
            var shift = new ShiftBuilder()
                .WithFixedCost(0)
                .WithBaseCost(0)
                .WithStartLocation(locationList[_random.Next(locationList.Count)])
                .WithEndLocation(locationList[_random.Next(locationList.Count)])
                .Build();
            
            shifts.Add(shift);
        }
        
        return shifts;
    }

    private IReadOnlySet<Location> BuildLocations()
    {
        var locations = new HashSet<Location>(LocationCount);
        
        for (var i = 0; i < LocationCount; i++)
        {
            var location = new LocationBuilder()
                .WithAutomaticId()
                .Build();
            
            locations.Add(location);
        }
        
        return locations;
    }
    
    private DirectedRouteMatrix BuildRouteMatrix(IReadOnlySet<Location> locations)
    {
        var routeMatrix = new DirectedRouteMatrix();
        foreach (var firstLocation in locations)
        {
            foreach (var secondLocation in locations)
            {
                var distance = _random.Next(1, 3);
                var duration = _random.Next(1, 2);
                
                // Add the distance and duration between the two locations
                // to the route matrix
                var edge = new DefinedRouteEdge(distance, duration);
                routeMatrix.AddEdge(firstLocation, secondLocation, edge);
            }
        }

        return routeMatrix;
    }
}