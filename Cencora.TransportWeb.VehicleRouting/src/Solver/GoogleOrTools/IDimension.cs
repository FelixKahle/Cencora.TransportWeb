// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Represents a dimension of a vehicle routing problem.
/// </summary>
internal interface IDimension
{
    /// <summary>
    /// The internal Id of the dimension.
    /// </summary>
    public Id Id { get; }
}