// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.Common.Extensions;

public static class DictionaryExtensions
{
    /// <summary>
    /// Adds the specified key and value to the dictionary if the condition is <see langword="true"/>.
    /// </summary>
    /// <typeparam name="T1">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="T2">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to add the key and value to.</param>
    /// <param name="condition">The condition that must be <see langword="true"/> to add the key and value.</param>
    /// <param name="key">The key of the element to add to the dictionary.</param>
    /// <param name="value">The value of the element to add to the dictionary.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary"/> is <see langword="null"/>.</exception>
    public static void AddIf<T1, T2>(this Dictionary<T1, T2> dictionary, bool condition, T1 key, T2 value)
        where T1 : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));

        if (condition)
        {
            dictionary.Add(key, value);
        }
    }
}