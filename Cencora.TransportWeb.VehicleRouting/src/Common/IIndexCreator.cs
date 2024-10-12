// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Common;

/// <summary>
/// Represents an index creator.
/// </summary>
public interface IIndexCreator
{
    /// <summary>
    /// Gets the next index.
    /// </summary>
    public int GetNext();
}