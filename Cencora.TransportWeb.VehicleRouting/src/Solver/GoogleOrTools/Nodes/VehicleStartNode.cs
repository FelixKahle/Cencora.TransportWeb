// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;

/// <summary>
/// Represents a start node for a vehicle in the vehicle routing problem.
/// </summary>
internal sealed class VehicleStartNode : Node
{
    /// <summary>
    /// The vehicle that starts at this node.
    /// </summary>
    internal DummyVehicle Vehicle { get; }

    /// <summary>
    /// The location where the vehicle starts.
    /// </summary>
    internal Location? StartLocation { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleStartNode"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <param name="vehicle">The vehicle that starts at this node.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    internal VehicleStartNode(int index, DummyVehicle vehicle)
        : base(index)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

        Vehicle = vehicle;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleStartNode"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <param name="vehicle">The vehicle that starts at this node.</param>
    /// <param name="startLocation">The location where the vehicle starts.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    internal VehicleStartNode(int index, DummyVehicle vehicle, Location startLocation)
        : base(index)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

        Vehicle = vehicle;
        StartLocation = startLocation;
    }

    /// <inheritdoc/>
    internal override Location? GetLocation()
    {
        return StartLocation;
    }

    /// <inheritdoc/>
    internal override Shipment? GetShipment()
    {
        return null;
    }

    /// <inheritdoc/>
    internal override DummyVehicle? GetDummyVehicle()
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return StartLocation is null
            ? $"Arbitrary start node for vehicle {Vehicle}"
            : $"Start node for vehicle {Vehicle} at {StartLocation}";
    }
}
