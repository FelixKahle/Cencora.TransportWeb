// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;
using Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;

/// <summary>
/// Represents a node in the vehicle routing problem associated with a break.
/// </summary>
internal sealed class BreakNode : Node
{
    /// <summary>
    /// The vehicle associated with the node.
    /// </summary>
    internal DummyVehicle Vehicle { get; }

    /// <summary>
    /// The break.
    /// </summary>
    internal Break Break;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BreakNode"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <param name="vehicle">The vehicle associated with the node.</param>
    /// <param name="breakToPerform">The break to perform.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    public BreakNode(int index, DummyVehicle vehicle, Break breakToPerform)
        : base(index)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

        Vehicle = vehicle;
        Break = breakToPerform;
    }

    /// <inheritdoc/>
    internal override Location? GetLocation()
    {
        return Break.Location;
    }

    /// <inheritdoc/>
    internal override Shipment? GetShipment()
    {
        return null;
    }

    /// <inheritdoc/>
    internal override Shipment? GetPickup()
    {
        return null;
    }

    /// <inheritdoc/>
    internal override Shipment? GetDelivery()
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
        return Break.Duration;
    }

    /// <inheritdoc/>
    internal override ValueRange? GetTimeWindow()
    {
        return Break.AllowedTimeWindow;
    }

    /// <inheritdoc/>
    internal override long GetBreakTime()
    {
        return Break.Duration;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"BreakNode: {Index}";
    }
}