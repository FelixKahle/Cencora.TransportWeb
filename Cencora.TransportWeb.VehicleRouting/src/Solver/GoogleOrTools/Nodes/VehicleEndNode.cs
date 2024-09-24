// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;
using Cencora.TransportWeb.VehicleRouting.Model.Shipments;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;

/// <summary>
/// Represents an end node for a vehicle in the vehicle routing problem.
/// </summary>
internal sealed class VehicleEndNode : Node
{
    /// <summary>
    /// The vehicle that ends at this node.
    /// </summary>
    internal DummyVehicle Vehicle { get; }

    /// <summary>
    /// The location where the vehicle ends.
    /// </summary>
    internal Location? EndLocation { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleEndNode"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <param name="vehicle">The vehicle that ends at this node.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    internal VehicleEndNode(int index, DummyVehicle vehicle)
        : base(index)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

        Vehicle = vehicle;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleEndNode"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <param name="vehicle">The vehicle that ends at this node.</param>
    /// <param name="endLocation">The location where the vehicle ends.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is <see langword="null"/>.</exception>
    internal VehicleEndNode(int index, DummyVehicle vehicle, Location endLocation)
        : base(index)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

        Vehicle = vehicle;
        EndLocation = endLocation;
    }

    /// <inheritdoc/>
    internal override Location? GetLocation()
    {
        return EndLocation;
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return EndLocation is null
            ? $"Arbitrary end node for vehicle {Vehicle}"
            : $"End node for vehicle {Vehicle} at {EndLocation}";
    }
}
