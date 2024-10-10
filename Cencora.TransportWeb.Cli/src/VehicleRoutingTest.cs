// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

namespace Cencora.TransportWeb.Cli;

public class VehicleRoutingTest
{
    private readonly long[,] _distanceTimeMatrix = {
        { 0, 6, 9, 8, 7, 3, 6, 2, 3, 2, 6, 6, 4, 4, 5, 9, 7 },
        { 6, 0, 8, 3, 2, 6, 8, 4, 8, 8, 13, 7, 5, 8, 12, 10, 14 },
        { 9, 8, 0, 11, 10, 6, 3, 9, 5, 8, 4, 15, 14, 13, 9, 18, 9 },
        { 8, 3, 11, 0, 1, 7, 10, 6, 10, 10, 14, 6, 7, 9, 14, 6, 16 },
        { 7, 2, 10, 1, 0, 6, 9, 4, 8, 9, 13, 4, 6, 8, 12, 8, 14 },
        { 3, 6, 6, 7, 6, 0, 2, 3, 2, 2, 7, 9, 7, 7, 6, 12, 8 },
        { 6, 8, 3, 10, 9, 2, 0, 6, 2, 5, 4, 12, 10, 10, 6, 15, 5 },
        { 2, 4, 9, 6, 4, 3, 6, 0, 4, 4, 8, 5, 4, 3, 7, 8, 10 },
        { 3, 8, 5, 10, 8, 2, 2, 4, 0, 3, 4, 9, 8, 7, 3, 13, 6 },
        { 2, 8, 8, 10, 9, 2, 5, 4, 3, 0, 4, 6, 5, 4, 3, 9, 5 },
        { 6, 13, 4, 14, 13, 7, 4, 8, 4, 4, 0, 10, 9, 8, 4, 13, 4 },
        { 6, 7, 15, 6, 4, 9, 12, 5, 9, 6, 10, 0, 1, 3, 7, 3, 10 },
        { 4, 5, 14, 7, 6, 7, 10, 4, 8, 5, 9, 1, 0, 2, 6, 4, 8 },
        { 4, 8, 13, 9, 8, 7, 10, 3, 7, 4, 8, 3, 2, 0, 4, 5, 6 },
        { 5, 12, 9, 14, 12, 6, 6, 7, 3, 3, 4, 7, 6, 4, 0, 9, 2 },
        { 9, 10, 18, 6, 8, 12, 15, 8, 13, 9, 13, 3, 4, 5, 9, 0, 9 },
        { 7, 14, 9, 16, 14, 8, 5, 10, 6, 5, 4, 10, 8, 6, 2, 9, 0 },
    };

    public void Run()
    {
        var problem = BuildProblem();
        using var solver = new GoogleOrToolsSolver(new GoogleOrToolsSolverOptions(TimeSpan.FromSeconds(2)), new ConsoleLogger<GoogleOrToolsSolver>());
        var solution = solver.Solve(problem);
        
        Console.WriteLine($"Has solution: {solution.HasSolution}");

        if (solution.HasSolution)
        {
            var vehiclePlans = solution.Solution?.VehiclePlans ?? throw new InvalidOperationException("Solution is null");
            foreach (var plan in vehiclePlans)
            {
                Console.WriteLine(plan.ToDebugString());
            }
        }
    }

    private Problem BuildProblem()
    {
        var locations = BuildLocations();
        var routeMatrix = BuildRouteMatrix(locations);
        var shipments = BuildShipments(locations);
        var vehicles = BuildVehicles(locations);
        
        return new ProblemBuilder()
            .WithLocations(locations)
            .WithRouteMatrix(routeMatrix)
            .WithShipments(shipments)
            .WithVehicles(vehicles)
            .Build();
    }
    
    private List<Vehicle> BuildVehicles(List<Location> locations)
    {
        var shift1 = new ShiftBuilder()
            .WithShiftTimeWindow(new ValueRange(0, 2000))
            .WithBreak(new Break(new ValueRange(0, 2000), 10, BreakOption.Mandatory))
            .WithMaxDuration(400)
            .WithStartLocation(locations.Find(l => l.Id == "0") ?? throw new InvalidOperationException("Location not found"))
            .WithEndLocation(locations.Find(l => l.Id == "0") ?? throw new InvalidOperationException("Location not found"))
            .Build();
        
        var vehicle = new VehicleBuilder()
            .WithId("First Vehicle")
            .WithShift(shift1)
            .WithDistanceCost(10)
            .WithTimeCost(10)
            .WithFixedCost(10)
            .WithBaseCost(10)
            .WithMaxWeight(10000000)
            .Build();
        
        var shift3 = new ShiftBuilder()
            .WithShiftTimeWindow(new ValueRange(200, 2000))
            .WithBreak(new Break(new ValueRange(200, 2000), 10, BreakOption.Mandatory))
            .WithStartLocation(locations.Find(l => l.Id == "0") ?? throw new InvalidOperationException("Location not found"))
            .WithEndLocation(locations.Find(l => l.Id == "0") ?? throw new InvalidOperationException("Location not found"))
            .Build();
        
        var secondVehicle = new VehicleBuilder()
            .WithId("Second Vehicle")
            .WithShift(shift3)
            .WithDistanceCost(10)
            .WithTimeCost(10)
            .WithFixedCost(10)
            .WithBaseCost(10)
            .WithMaxWeight(10000000)
            .Build();
        
        return [vehicle, secondVehicle];
    }
    
    private List<Shipment> BuildShipments(List<Location> locations)
    {
        var shipments = new List<Shipment>();
        
        var firstShipment =
            new ShipmentBuilder(
                    locations.Find(l => l.Id == "3") ?? throw new InvalidOperationException("Location not found"),
                    locations.Find(l => l.Id == "0") ?? throw new InvalidOperationException("Location not found"))
            .WithId("123")
            .WithPickupHandlingTime(3)
            .WithDeliveryHandlingTime(5)
            .WithPickupTimeWindow(new ValueRange(7, 12))
            .WithDeliveryTimeWindow(new ValueRange(9, 20))
            .WithWeight(10)
            .Build();
        
        var secondShipment =
            new ShipmentBuilder(
                    locations.Find(l => l.Id == "3") ?? throw new InvalidOperationException("Location not found"),
                    locations.Find(l => l.Id == "0") ?? throw new InvalidOperationException("Location not found"))
                .WithId("267")
                .WithPickupHandlingTime(3)
                .WithDeliveryHandlingTime(5)
                .WithPickupTimeWindow(new ValueRange(350, 400))
                .WithDeliveryTimeWindow(new ValueRange(420, 490))
                .WithWeight(10)
                .Build();
        
        
        var thirdShipment =
            new ShipmentBuilder(
                    locations.Find(l => l.Id == "0") ?? throw new InvalidOperationException("Location not found"),
                    locations.Find(l => l.Id == "9") ?? throw new InvalidOperationException("Location not found"))
                .WithId("555")
                .WithPickupHandlingTime(3)
                .WithDeliveryHandlingTime(5)
                .WithPickupTimeWindow(new ValueRange(710, 800))
                .WithDeliveryTimeWindow(new ValueRange(801, 860))
                .WithWeight(10)
                .Build();
        
        shipments.Add(firstShipment);
        shipments.Add(secondShipment);
        shipments.Add(thirdShipment);

        return shipments;
    }

    private List<Location> BuildLocations()
    {
        var locations = new List<Location>();
        for (var i = 0; i < _distanceTimeMatrix.GetLength(0); i++)
        {
            var location = new LocationBuilder()
                .WithId(i.ToString())
                .Build();
            locations.Add(location);
        }

        return locations;
    }
    
    private DirectedRouteMatrix BuildRouteMatrix(List<Location> locations)
    {
        var builder = new DirectedRouteMatrixBuilder();
        for (var i = 0; i < _distanceTimeMatrix.GetLength(0); i++)
        {
            var firstLocation = locations[i];
            for (var j = 0; j < _distanceTimeMatrix.GetLength(1); j++)
            {
                var secondLocation = locations[j];
                
                builder.WithEdge(firstLocation, secondLocation, new DefinedRouteEdge(_distanceTimeMatrix[i, j], _distanceTimeMatrix[i, j]));
            }
        }
        
        return builder.Build();
    }
}