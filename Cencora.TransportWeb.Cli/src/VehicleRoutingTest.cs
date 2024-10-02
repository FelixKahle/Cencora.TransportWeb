// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
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
    private const int ShipmentCount = 1;
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
        var routeMatrix = BuildRouteMatrix(locations);
        
        var shipment = new ShipmentBuilder()
            .WithAutomaticId()
            .WithPickupLocation(locations.First())
            .WithDeliveryLocation(locations.Last())
            .WithPickupTimeWindow(new ValueRange(0, 100))
            .WithDeliveryTimeWindow(new ValueRange(0, 100))
            .Build();
        
        var shift = new ShiftBuilder()
            .WithShiftTimeWindow(new ValueRange(0, 100))
            .Build();
        var shift2 = new ShiftBuilder()
            .WithShiftTimeWindow(new ValueRange(100, 200))
            .Build();
        
        var vehicle = new VehicleBuilder()
            .WithAutomaticId()
            .WithShift(shift)
            .WithShift(shift2)
            .WithFixedCost(0)
            .WithBaseCost(0)
            .Build();

        var problem = new ProblemBuilder()
            .WithRouteMatrix(routeMatrix)
            .WithLocations(locations)
            .WithVehicle(vehicle)
            .WithMaxVehicleWaitingTime(long.MaxValue)
            .WithShipment(shipment)
            .Build();
        
        Console.WriteLine($"{problem}");
        Console.WriteLine($"{string.Join("\n", problem.Shipments)}");
        
        return problem;
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
                if (firstLocation == secondLocation)
                {
                    var sameEdge = new DefinedRouteEdge(0, 0);
                    routeMatrix.AddEdge(firstLocation, secondLocation, sameEdge);
                    continue;
                }
                
                var distance = _random.Next(1, 50);
                var duration = _random.Next(1, 10);
                
                // Add the distance and duration between the two locations
                // to the route matrix
                var edge = new DefinedRouteEdge(distance, duration);
                routeMatrix.AddEdge(firstLocation, secondLocation, edge);
            }
        }

        return routeMatrix;
    }
}