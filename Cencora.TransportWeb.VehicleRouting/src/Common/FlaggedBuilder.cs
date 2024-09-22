// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Flags;

namespace Cencora.TransportWeb.VehicleRouting.Common;

/// <summary>
/// Base class for builders that build objects with flags.
/// </summary>
public abstract class FlaggedBuilder
{
    /// <summary>
    /// The flags of the object being built.
    /// </summary>
    protected FlagContainer Flags { get; } = new();

    /// <summary>
    /// Adds a flag to the object being built.
    /// </summary>
    public FlaggedBuilder WithFlag(Flag flag)
    {
        Flags.AddFlag(flag);
        return this;
    }

    /// <summary>
    /// Removes a flag from the object being built.
    /// </summary>
    public FlaggedBuilder WithoutFlag(Flag flag)
    {
        Flags.RemoveFlag(flag);
        return this;
    }

    /// <summary>
    /// Adds multiple flags to the object being built.
    /// </summary>
    /// <param name="flags">The flags to add.</param>
    /// <returns>The current instance of the <see cref="FlaggedBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="flags"/> is <see langword="null"/>.</exception>
    public FlaggedBuilder WithFlags(IEnumerable<Flag> flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        Flags.AddFlags(flags);
        return this;
    }

    /// <summary>
    /// Removes multiple flags from the object being built.
    /// </summary>
    /// <param name="flags">The flags to remove.</param>
    /// <returns>The current instance of the <see cref="FlaggedBuilder"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="flags"/> is <see langword="null"/>.</exception>
    public FlaggedBuilder WithoutFlags(IEnumerable<Flag> flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        Flags.RemoveFlags(flags);
        return this;
    }
}
