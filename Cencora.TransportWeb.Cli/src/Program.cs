// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Cli;

public static class Program
{
    public static void Main(string[] args)
    {
        VehicleRoutingTest test = new VehicleRoutingTest();
        test.Run();
    }
}