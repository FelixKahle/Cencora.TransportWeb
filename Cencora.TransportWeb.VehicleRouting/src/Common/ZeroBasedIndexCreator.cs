// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Common;

/// <summary>
/// Represents an index creator that creates zero-based indices.
/// </summary>
public sealed class ZeroBasedIndexCreator : IIndexCreator
{
    /// <summary>
    /// The current index.
    /// </summary>
    private int _currentIndex;
    
    /// <inheritdoc/>
    public int GetNext()
    {
        return _currentIndex++;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"ZeroBasedIndexCreator({_currentIndex})";
    }
}