// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;

/// <summary>
/// Vehicle configurator.
/// </summary>
internal sealed class VehicleConfigurator : IConfigurator<Dimension>
{
    /// <inheritdoc/>
    public void Configure(SolverState<Dimension> state)
    {
        for (var i = 0; i < state.SolverModel.VehicleCount; i++)
        {
            var vehicle = state.SolverModel.Vehicles[i];
            
            var fixedCost = vehicle.FixedCost;
            var baseCost = vehicle.BaseCost;
            var totalCost = fixedCost + baseCost;
            
            state.SolverInterface.RoutingModel.SetFixedCostOfVehicle(totalCost, i);
            
            // If the vehicle has a fixed cost, we need to consider it in the objective
            // function. To do this we set the vehicle as used when empty, as soon
            // as the fixed cost is greater than zero.
            state.SolverInterface.RoutingModel.SetVehicleUsedWhenEmpty(fixedCost > 0, i);
        }
    }
}