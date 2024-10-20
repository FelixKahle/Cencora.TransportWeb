// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;

/// <summary>
/// Node configurator.
/// </summary>
internal sealed class NodeConfigurator : IConfigurator<Dimension>
{
    /// <inheritdoc/>
    public void Configure(SolverState<Dimension> state)
    {
        var nodeCallback = state.SolverInterface.RegisterCallback(new NodeCallback());
        var nodeDimension = state.SolverInterface.RegisterDimension(Dimension.NodeDimension, new NodeDimension(nodeCallback));
        
        foreach (var store in state.SolverModel.ShipmentNodeStores.Values)
        {
            var pickupNode = store.Pickup;
            var deliveryNode = store.Delivery;

            var pickupIndex = state.SolverInterface.NodeToIndex(pickupNode);
            var deliveryIndex = state.SolverInterface.NodeToIndex(deliveryNode);
            
            state.SolverInterface.RoutingModel.AddPickupAndDelivery(pickupIndex, deliveryIndex);
            // The following line adds the requirement that each item must be picked up and delivered by the same vehicle.
            state.SolverInterface.Solver.Add(state.SolverInterface.Solver.MakeEquality(state.SolverInterface.RoutingModel.VehicleVar(pickupIndex), state.SolverInterface.RoutingModel.VehicleVar(deliveryIndex)));
            // Finally, we add the obvious requirement that each item must be picked up before it is delivered. 
            state.SolverInterface.Solver.Add(state.SolverInterface.Solver.MakeLessOrEqual(nodeDimension.RoutingDimension.CumulVar(pickupIndex), nodeDimension.RoutingDimension.CumulVar(deliveryIndex)));
        }
    }
}