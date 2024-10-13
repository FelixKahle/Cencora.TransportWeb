// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Base class for single capacity dimensions.
/// </summary>
internal class SingleCapacityDimension : ISingleCapacityDimension, IEquatable<SingleCapacityDimension>
{
    /// <summary>
    /// The callback that is used to get the values for the dimension.
    /// </summary>
    internal ICallback Callback { get; }
    
    /// <summary>
    /// The name of the dimension.
    /// </summary>
    internal string DimensionName { get; }
    
    /// <summary>
    /// The maximum slack value for the dimension.
    /// </summary>
    internal long MaxSlackValue { get; }
    
    /// <summary>
    /// Whether the dimension should start at zero.
    /// </summary>
    internal bool StartAtZero { get; }
    
    /// <summary>
    /// The internal routing dimension.
    /// </summary>
    internal RoutingDimension Dimension { get; }
    
    /// <summary>
    /// The capacity of the dimension.
    /// </summary>
    internal long Capacity { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleCapacityDimension"/> class.
    /// </summary>
    /// <param name="callback">The callback that is used to get the values for the dimension.</param>
    /// <param name="dimensionName">The name of the dimension.</param>
    /// <param name="maxSlackValue">The maximum slack value for the dimension.</param>
    /// <param name="capacity">The capacity of the dimension.</param>
    /// <param name="startAtZero">Whether the dimension should start at zero.</param>
    /// <param name="registrant">The dimension registrant.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="dimensionName"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxSlackValue"/> is negative or zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="capacity"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="registrant"/> is null.</exception>
    internal SingleCapacityDimension(ICallback callback, string dimensionName, long maxSlackValue, long capacity, bool startAtZero, IDimensionRegistrant registrant)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        ArgumentException.ThrowIfNullOrWhiteSpace(dimensionName, nameof(dimensionName));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSlackValue, nameof(maxSlackValue));
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
        ArgumentNullException.ThrowIfNull(registrant, nameof(registrant));
        
        Callback = callback;
        DimensionName = dimensionName;
        MaxSlackValue = maxSlackValue;
        StartAtZero = startAtZero;
        Capacity = capacity;
        Dimension = registrant.RegisterDimension(this);
    }
    
    /// <inheritdoc/>
    public ICallback GetCallback()
    {
        return Callback;
    }

    /// <inheritdoc/>
    public string GetDimensionName()
    {
        return DimensionName;
    }

    /// <inheritdoc/>
    public long GetMaxSlackValue()
    {
        return MaxSlackValue;
    }

    /// <inheritdoc/>
    public bool ShouldStartAtZero()
    {
        return StartAtZero;
    }

    /// <inheritdoc/>
    public RoutingDimension GetDimension()
    {
        return Dimension;
    }

    /// <inheritdoc/>
    public long GetCapacity()
    {
        return Capacity;
    }

    /// <inheritdoc/>
    public bool Equals(SingleCapacityDimension? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return DimensionName.Equals(other.DimensionName, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is SingleCapacityDimension other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return DimensionName.GetHashCode(StringComparison.Ordinal);
    }
    
    /// <summary>
    /// Implicitly converts the dimension to a string.
    /// </summary>
    /// <param name="dimension">The dimension to convert.</param>
    /// <returns>The name of the dimension.</returns>
    public static implicit operator string (SingleCapacityDimension dimension)
    {
        return dimension.DimensionName;
    }
    
    /// <summary>
    /// Implicitly converts the dimension to a <see cref="RoutingDimension"/>.
    /// </summary>
    /// <param name="dimension">The dimension to convert.</param>
    /// <returns>The internal routing dimension.</returns>
    public static implicit operator RoutingDimension (SingleCapacityDimension dimension)
    {
        return dimension.Dimension;
    }
    
    /// <summary>
    /// Determines whether two <see cref="SingleCapacityDimension"/> objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><langword>true</langword> if the objects are equal; otherwise, <langword>false</langword>.</returns>
    public static bool operator ==(SingleCapacityDimension? left, SingleCapacityDimension? right)
    {
        return Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether two <see cref="SingleCapacityDimension"/> objects are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><langword>true</langword> if the objects are not equal; otherwise, <langword>false</langword>.</returns>
    public static bool operator !=(SingleCapacityDimension? left, SingleCapacityDimension? right)
    {
        return !Equals(left, right);
    }
}