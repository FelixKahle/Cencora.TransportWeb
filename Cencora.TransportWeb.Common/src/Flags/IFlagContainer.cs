// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Flags;

/// <summary>
/// Represents a container for flags.
/// </summary>
public interface IFlagContainer : IReadOnlyFlagContainer
{
    /// <summary>
    /// Adds a flag to the container.
    /// </summary>
    /// <param name="flag">The flag to add.</param>
    public void AddFlag(Flag flag);

    /// <summary>
    /// Adds multiple flags to the container.
    /// </summary>
    /// <param name="flags">The flags to add.</param>
    public void AddFlags(IEnumerable<Flag> flags);

    /// <summary>
    /// Removes a flag from the container.
    /// </summary>
    /// <param name="flag">The flag to remove.</param>
    public void RemoveFlag(Flag flag);

    /// <summary>
    /// Removes multiple flags from the container.
    /// </summary>
    /// <param name="flags">The flags to remove.</param>
    public void RemoveFlags(IEnumerable<Flag> flags);

    /// <summary>
    /// Gets all flags in the container.
    /// </summary>
    /// <returns>All flags in the container.</returns>
    public IEnumerable<Flag> GetFlags();

    /// <summary>
    /// Clears all flags from the container.
    /// </summary>
    public void ClearFlags();
}
