// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Represents a node in the vehicle routing problem that has a location.
/// </summary>
internal abstract class LocatedNode : Node
{
    /// <summary>
    /// The location of the node.
    /// </summary>
    internal Location Location { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocatedNode"/> class.
    /// </summary>
    /// <param name="index">The index of the node in the list of nodes of the solver.</param>
    /// <param name="location">The location of the node.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="location"/> is <see langword="null"/>.</exception>
    internal LocatedNode(int index, Location location)
        : base(index)
    {
        ArgumentNullException.ThrowIfNull(location, nameof(location));

        Location = location;
    }

    /// <inheritdoc/>
    internal override Location? GetLocation()
    {
        return Location;
    }
}
