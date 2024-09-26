// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;

/// <summary>
/// Enumerates the types of vehicle nodes.
/// </summary>
internal enum VehicleNodeType
{
    /// <summary>
    /// The node is a start node.
    /// </summary>
    Start,

    /// <summary>
    /// The node is an end node.
    /// </summary>
    End,
}

/// <summary>
/// Represents a node in the vehicle routing problem associated with a vehicle.
/// </summary>
internal sealed class VehicleNode : Node
{
    /// <summary>
    /// The vehicle associated with the node.
    /// </summary>
    internal DummyVehicle Vehicle { get; }

    /// <summary>
    /// The location of the node, <see langword="null"/> if the node is not associated with a location.
    /// </summary>
    internal Location? Location { get; }

    /// <summary>
    /// The type of the node.
    /// </summary>
    internal VehicleNodeType Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleNode"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <param name="vehicle">The vehicle associated with the node.</param>
    /// <param name="location">The location of the node.</param>
    /// <param name="type">The type of the node.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    internal VehicleNode(int index, DummyVehicle vehicle, Location? location, VehicleNodeType type)
        : base(index)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

        Vehicle = vehicle;
        Location = location;
        Type = type;
    }

    /// <inheritdoc/>
    internal override Location? GetLocation()
    {
        return Location;
    }

    /// <inheritdoc/>
    internal override Shipment? GetShipment()
    {
        return null;
    }

    /// <inheritdoc/>
    internal override DummyVehicle GetDummyVehicle()
    {
        return Vehicle;
    }

    /// <inheritdoc/>
    internal override long GetWeightDemand()
    {
        return 0;
    }

    /// <inheritdoc/>
    internal override long GetTimeDemand()
    {
        return 0;
    }

    /// <inheritdoc/>
    internal override ValueRange? GetTimeWindow()
    {
        return Vehicle.AvailableTimeWindow;
    }

    /// <summary>
    /// Returns a string representation of the node type.
    /// </summary>
    /// <returns>A string representation of the node type.</returns>
    private string GetNodeTypeString()
    {
        return Type switch
        {
            VehicleNodeType.Start => "Start",
            VehicleNodeType.End => "End",
            _ => "Unknown"
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var nodeType = GetNodeTypeString();

        return Location is null
            ? $"{nodeType} Node {Index} of Vehicle {Vehicle.Id}"
            : $"{nodeType} Node {Index} of Vehicle {Vehicle.Id} at {Location}";
    }
}
