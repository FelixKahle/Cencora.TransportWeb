// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Collections;

namespace Cencora.TransportWeb.Common.Flags;

/// <summary>
/// Represents a container for flags.
/// </summary>
public class FlagContainer : IFlagContainer
{
    /// <summary>
    /// Holds the flags of the flaggable object.
    /// </summary>
    private HashSet<Flag> FlagsSet { get; } = [];

    /// <summary>
    /// Gets the flags of the container as a read-only set.
    /// </summary>
    public IReadOnlySet<Flag> Flags => FlagsSet;

    /// <summary>
    /// Gets an empty flag container.
    /// </summary>
    public static FlagContainer Empty => new();

    /// <summary>
    /// Initializes a new instance of the <see cref="FlagContainer"/> class.
    /// </summary>
    public FlagContainer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlagContainer"/> class.
    /// </summary>
    /// <param name="flags">The flags to add to the container.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="flags"/> is <see langword="null"/>.</exception>
    public FlagContainer(IEnumerable<Flag> flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        AddFlags(flags);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlagContainer"/> class.
    /// </summary>
    /// <param name="flags">The flags to add to the container.</param>
    public FlagContainer(params Flag[] flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        AddFlags(flags);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlagContainer"/> class.
    /// </summary>
    /// <param name="other">The flag container to copy the flags from.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="other"/> is <see langword="null"/>.</exception>
    public FlagContainer(FlagContainer other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        AddFlags(other.Flags);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlagContainer"/> class.
    /// </summary>
    /// <param name="flag">The flag to add to the container.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="flag"/> is <see langword="null"/>.</exception>
    public FlagContainer(Flag flag)
    {
        ArgumentNullException.ThrowIfNull(flag, nameof(flag));

        AddFlag(flag);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlagContainer"/> class.
    /// </summary>
    /// <param name="initialCapacity">The initial capacity of the container.</param>
    public FlagContainer(int initialCapacity)
    {
        var adjustedInitialCapacity = Math.Max(0, initialCapacity);
        FlagsSet = new(adjustedInitialCapacity);
    }

    /// <inheritdoc/>
    public void AddFlag(Flag flag)
    {
        ArgumentNullException.ThrowIfNull(flag, nameof(flag));

        FlagsSet.Add(flag);
    }

    /// <inheritdoc/>
    public void AddFlags(IEnumerable<Flag> flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        foreach (var flag in flags)
        {
            AddFlag(flag);
        }
    }

    /// <inheritdoc/>
    public void ClearFlags()
    {
        FlagsSet.Clear();
    }

    /// <inheritdoc/>
    public IEnumerable<Flag> GetFlags()
    {
        return FlagsSet;
    }

    /// <inheritdoc/>
    public bool HasAllFlags(IEnumerable<Flag> flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        return flags.All(HasFlag);
    }

    /// <inheritdoc/>
    public bool HasAnyFlag(IEnumerable<Flag> flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        return FlagsSet.Any(HasFlag);
    }

    /// <inheritdoc/>
    public bool HasFlag(Flag flag)
    {
        ArgumentNullException.ThrowIfNull(flag, nameof(flag));

        return FlagsSet.Contains(flag);
    }

    /// <inheritdoc/>
    public bool HasNoneFlag(IEnumerable<Flag> flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        return flags.All(f => !HasFlag(f));
    }

    /// <inheritdoc/>
    public void RemoveFlag(Flag flag)
    {
        ArgumentNullException.ThrowIfNull(flag, nameof(flag));

        FlagsSet.Remove(flag);
    }

    /// <inheritdoc/>
    public void RemoveFlags(IEnumerable<Flag> flags)
    {
        ArgumentNullException.ThrowIfNull(flags, nameof(flags));

        foreach (var flag in flags)
        {
            RemoveFlag(flag);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<Flag> GetEnumerator()
    {
        return FlagsSet.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return FlagsSet.GetEnumerator();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return string.Join(", ", FlagsSet);
    }

    /// <inheritdoc/>
    public int Count => FlagsSet.Count;

    /// <inheritdoc/>
    public bool Any => FlagsSet.Count == 0;

    /// <summary>
    /// Implicitly converts a <see cref="FlagContainer"/> to a <see cref="Flag"/> array.
    /// </summary>
    /// <param name="flags">The flags.</param>
    /// <returns>The flags of the container as an array.</returns>
    public static implicit operator FlagContainer(Flag[] flags)
    {
        return new FlagContainer(flags);
    }

    /// <summary>
    /// Implicitly converts a <see cref="FlagContainer"/> to a <see cref="Flag"/> array.
    /// </summary>
    /// <param name="flag">The flag.</param>
    /// <returns>The flags of the container as an array.</returns>
    public static implicit operator FlagContainer(Flag flag)
    {
        return new FlagContainer(flag);
    }
}
