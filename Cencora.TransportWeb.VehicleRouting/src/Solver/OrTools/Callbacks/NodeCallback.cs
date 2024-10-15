// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;

/// <summary>
/// Node callback.
/// </summary>
internal sealed class NodeCallback : IUnaryTransitCallback
{
    /// <inheritdoc/>
    public long GetTransit(Node node)
    {
        return 1;
    }
}