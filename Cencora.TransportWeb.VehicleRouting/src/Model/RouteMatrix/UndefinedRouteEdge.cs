// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;

/// <summary>
/// Represents an undefined edge in a route matrix.
/// </summary>
public readonly struct UndefinedRouteEdge : IRouteEdge, IEquatable<UndefinedRouteEdge>
{
    /// <inheritdoc/>
    public bool Equals(UndefinedRouteEdge other)
    {
        return true;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is UndefinedRouteEdge;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return 0;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return "(Undefined)";
    }
}
