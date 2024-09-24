// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicle;

/// <summary>
/// Enumerates the options for a break.
/// </summary>
public enum BreakOption
{
    /// <summary>
    /// The break is optional.
    /// </summary>
    Optional,

    /// <summary>
    /// The break is mandatory.
    /// </summary>
    Mandatory,

    /// <summary>
    /// The break is forbidden.
    /// </summary>
    Forbidden
}
