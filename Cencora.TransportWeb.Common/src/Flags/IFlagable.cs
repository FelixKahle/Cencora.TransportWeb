// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Flags;

/// <summary>
/// Represents a flaggable object that can be flagged.
/// </summary>
public interface IFlaggable
{
    /// <summary>
    /// Adds a flag to the flaggable object.
    /// </summary>
    /// <param name="flag">The flag to add.</param>
    public void AddFlag(Flag flag);
    
    /// <summary>
    /// Adds multiple flags to the flaggable object.
    /// </summary>
    /// <param name="flags">The flags to add.</param>
    public void AddFlags(IEnumerable<Flag> flags);
    
    /// <summary>
    /// Removes a flag from the flaggable object.
    /// </summary>
    /// <param name="flag">The flag to remove.</param>
    public void RemoveFlag(Flag flag);
    
    /// <summary>
    /// Removes multiple flags from the flaggable object.
    /// </summary>
    /// <param name="flags">The flags to remove.</param>
    public void RemoveFlags(IEnumerable<Flag> flags);
}