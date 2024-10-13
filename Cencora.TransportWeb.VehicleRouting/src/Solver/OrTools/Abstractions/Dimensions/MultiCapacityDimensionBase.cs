// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Base class for multi capacity dimensions.
/// </summary>
internal abstract class MultiCapacityDimensionBase : DimensionBase, IMultiCapacityDimension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiCapacityDimensionBase"/> class.
    /// </summary>
    /// <param name="name">The name of the dimension.</param>
    /// <param name="callback">The callback of the dimension.</param>
    /// <param name="maxSlackValue">The maximum slack value of the dimension.</param>
    /// <param name="startAtZero">A flag indicating whether the dimension needs to start from zero.</param>
    protected MultiCapacityDimensionBase(string name, ICallback callback, long maxSlackValue, bool startAtZero) : base(name, callback, maxSlackValue, startAtZero)
    {
    }

    /// <inheritdoc/>
    public abstract long[] GetCapacities();
}