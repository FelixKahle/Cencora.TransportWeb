// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Extensions;

/// <summary>
/// Provides extension methods for lists.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Determines whether the list contains all items in the specified collection.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="list">The list to check.</param>
    /// <param name="items">The collection of items to check for.</param>
    /// <returns><see langword="true"/> if the list contains all items in the collection; otherwise <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static bool ContainsAll<T>(this IReadOnlyList<T> list, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        HashSet<T> set = new(items);
        return list.All(set.Contains);
    }

    /// <summary>
    /// Returns the last element of the list or a default value if the list is empty.
    /// </summary>
    /// <remarks>
    /// This method is similar to the <see cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})"/> method
    /// but works on lists and is more efficient.
    /// </remarks>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="list">The list to get the last element from.</param>
    /// <param name="defaultValue">The default value to return if the list is empty.</param>
    /// <returns>The last element of the list or the default value if the list is empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <see langword="null"/>.</exception>
    public static T? LastOrDefault<T>(this IReadOnlyList<T> list, T? defaultValue = default)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));

        return list.Count > 0 ? list[list.Count - 1] : defaultValue;
    }

    /// <summary>
    /// Returns the last element of the list.
    /// </summary>
    /// <remarks>
    /// This method is similar to the <see cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/> method
    /// but works on lists and is more efficient.
    /// </remarks>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="list">The list to get the last element from.</param>
    /// <returns>The last element of the list.</returns>
    /// <exception cref="InvalidOperationException">The list is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <see langword="null"/>.</exception>
    public static T Last<T>(this IReadOnlyList<T> list)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));

        return list.Count > 0 ? list[list.Count - 1] : throw new InvalidOperationException("The list is empty.");
    }

    /// <summary>
    /// Returns the first element of the list or a default value if the list is empty.
    /// </summary>
    /// <remarks>
    /// This method is similar to the <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/> method
    /// but works on lists and is more efficient.
    /// </remarks>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="list">The list to get the first element from.</param>
    /// <param name="defaultValue">The default value to return if the list is empty.</param>
    /// <returns>The first element of the list or the default value if the list is empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <see langword="null"/>.</exception>
    public static T? FirstOrDefault<T>(this IReadOnlyList<T> list, T? defaultValue = default)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));

        return list.Count > 0 ? list[0] : defaultValue;
    }

    /// <summary>
    /// Returns the first element of the list.
    /// </summary>
    /// <remarks>
    /// This method is similar to the <see cref="Enumerable.First{TSource}(IEnumerable{TSource})"/> method
    /// but works on lists and is more efficient.
    /// </remarks>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="list">The list to get the first element from.</param>
    /// <returns>The first element of the list.</returns>
    /// <exception cref="InvalidOperationException">The list is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <see langword="null"/>.</exception>
    public static T First<T>(this IReadOnlyList<T> list)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        return list.Count > 0 ? list[0] : throw new InvalidOperationException("The list is empty.");
    }

    /// <summary>
    /// Determines whether the specified index is valid for the list.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="list">The list to check.</param>
    /// <param name="index">The index to check.</param>
    /// <returns><see langword="true"/> if the index is valid; otherwise <see langword="false"/>.</returns>
    public static bool IsIndexValid<T>(this IReadOnlyList<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }
}
