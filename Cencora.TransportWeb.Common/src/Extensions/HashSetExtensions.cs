// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Extensions;

public static class HashSetExtensions
{
    /// <summary>
    /// Adds the elements of the specified collection to the set.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    /// <param name="set">The set to add the elements to.</param>
    /// <param name="collection">The collection whose elements should be added to the set.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="set"/> or <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        foreach (var item in collection)
        {
            set.Add(item);
        }
    }
    
    /// <summary>
    /// Adds the specified item to the set if the condition is <see langword="true"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    /// <param name="set">The set to add the item to.</param>
    /// <param name="condition">The condition that must be <see langword="true"/> to add the item.</param>
    /// <param name="item">The item to add to the set.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="set"/> is <see langword="null"/>.</exception>
    public static void AddIf<T>(this HashSet<T> set, bool condition, T item)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));
        
        if (condition)
        {
            set.Add(item);
        }
    }

    /// <summary>
    /// Removes the elements of the specified collection from the set.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    /// <param name="set">The set to remove the elements from.</param>
    /// <param name="collection">The collection whose elements should be removed from the set.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="set"/> or <paramref name="collection"/> is <see langword="null"/>.</exception>
    public static void RemoveRange<T>(this HashSet<T> set, IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        foreach (var item in collection)
        {
            set.Remove(item);
        }
    }
}