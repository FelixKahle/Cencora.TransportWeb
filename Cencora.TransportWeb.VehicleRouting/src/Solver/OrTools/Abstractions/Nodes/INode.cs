// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

/// <summary>
/// Interface for a node.
/// </summary>
internal interface INode
{
    /// <summary>
    /// Gets the index of the node.
    /// </summary>
    int GetIndex();
    
    /// <summary>
    /// Gets the location of the node.
    /// </summary>
    Location? GetLocation();
    
    /// <summary>
    /// Gets the shipment of the node.
    /// </summary>
    Shipment? GetShipment();
    
    /// <summary>
    /// Gets the pickup of the node.
    /// </summary>
    Shipment? GetPickup();
    
    /// <summary>
    /// Gets the delivery of the node.
    /// </summary>
    Shipment? GetDelivery();
    
    /// <summary>
    /// Gets the dummy vehicle of the node.
    /// </summary>
    DummyVehicle? GetDummyVehicle();
    
    /// <summary>
    /// Gets the weight demand of the node.
    /// </summary>
    long GetWeightDemand();
    
    /// <summary>
    /// Gets the time demand of the node.
    /// </summary>
    long GetTimeDemand();
    
    /// <summary>
    /// Gets the time window of the node.
    /// </summary>
    ValueRange? GetTimeWindow();
}