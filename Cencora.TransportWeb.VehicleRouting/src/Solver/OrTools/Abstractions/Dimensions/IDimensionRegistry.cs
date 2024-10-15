// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Represents a dimension registrant.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
internal interface IDimensionRegistry<in TKey>
    where TKey : notnull
{
    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The registered dimension.</returns>
    SolverDimension RegisterDimension(TKey key, ISingleCapacityDimension dimension);
    
    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="dimension">The dimension.</param>
    /// <returns>The registered dimension.</returns>
    SolverDimension RegisterDimension(TKey key, IMultiCapacityDimension dimension);
    
    /// <summary>
    /// Gets the dimension.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The dimension.</returns>
    /// <exception cref="KeyNotFoundException">The key was not found.</exception>
    SolverDimension GetDimension(TKey key);
    
    /// <summary>
    /// Gets the count of dimensions.
    /// </summary>
    int DimensionCount { get; }
}