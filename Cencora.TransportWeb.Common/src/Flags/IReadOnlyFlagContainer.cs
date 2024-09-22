// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Flags;

/// <summary>
/// Represents a container for flags.
/// </summary>
public interface IReadOnlyFlagContainer
{
    /// <summary>
    /// Gets the flags of the container as a read-only set.
    /// </summary>
    IReadOnlySet<Flag> Flags { get; }

    /// <summary>
    /// Checks if the container has any of the specified flags.
    /// </summary>
    /// <param name="flags">The flags to check for.</param>
    /// <returns><c>true</c> if the container has any of the flags, <c>false</c> otherwise.</returns>
    public bool HasAnyFlag(IEnumerable<Flag> flags);

    /// <summary>
    /// Checks if the container has all of the specified flags.
    /// </summary>
    /// <param name="flags">The flags to check for.</param>
    /// <returns><c>true</c> if the container has all of the flags, <c>false</c> otherwise.</returns>
    public bool HasAllFlags(IEnumerable<Flag> flags);

    /// <summary>
    /// Checks if the container has a specific flag.
    /// </summary>
    /// <param name="flag">The flag to check for.</param>
    /// <returns><c>true</c> if the container has the flag, <c>false</c> otherwise.</returns>
    public bool HasFlag(Flag flag);

    /// <summary>
    /// Checks if the container has none of the specified flags.
    /// </summary>
    /// <param name="flags">The flags to check for.</param>
    /// <returns><c>true</c> if the container has none of the flags, <c>false</c> otherwise.</returns>
    public bool HasNoneFlag(IEnumerable<Flag> flags);

    /// <summary>
    /// Checks if the container has any flag.
    /// </summary>
    public bool Any { get; }

    /// <summary>
    /// Gets the number of flags in the container.
    /// </summary>
    public int Count { get; }
}