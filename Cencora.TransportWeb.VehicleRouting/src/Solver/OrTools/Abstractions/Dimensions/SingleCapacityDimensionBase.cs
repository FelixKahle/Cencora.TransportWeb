// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Base class for single capacity dimensions.
/// </summary>
internal abstract class SingleCapacityDimensionBase : DimensionBase, ISingleCapacityDimension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleCapacityDimensionBase"/> class.
    /// </summary>
    /// <param name="name">The name of the dimension.</param>
    /// <param name="callback">The callback of the dimension.</param>
    /// <param name="maxSlackValue">The maximum slack value of the dimension.</param>
    /// <param name="startAtZero">Indicates whether the dimension should start at zero.</param>
    protected SingleCapacityDimensionBase(string name, ICallback callback, long maxSlackValue, bool startAtZero) : base(
        name, callback, maxSlackValue, startAtZero)
    {
    }

    /// <inheritdoc/>
    public abstract long GetCapacity();
}