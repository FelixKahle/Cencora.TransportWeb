// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// A base class for dimensions.
/// </summary>
internal abstract class DimensionBase : IDimension, IEquatable<DimensionBase>
{
    /// <summary>
    /// The name of the dimension.
    /// </summary>
    internal string Name { get; }
    
    /// <summary>
    /// The callback of the dimension.
    /// </summary>
    internal ICallback Callback { get; }
    
    /// <summary>
    /// The maximum slack value of the dimension.
    /// </summary>
    internal long MaxSlackValue { get; }
    
    /// <summary>
    /// Indicates whether the dimension should start at zero.
    /// </summary>
    internal bool StartAtZero { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionBase"/> class.
    /// </summary>
    /// <param name="name">The name of the dimension.</param>
    /// <param name="callback">The callback of the dimension.</param>
    /// <param name="maxSlackValue">The maximum slack value of the dimension.</param>
    /// <param name="startAtZero">Indicates whether the dimension should start at zero.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is null.</exception>
    internal DimensionBase(string name, ICallback callback, long maxSlackValue, bool startAtZero)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));
        
        Name = name;
        Callback = callback;
        MaxSlackValue = maxSlackValue;
        StartAtZero = startAtZero;
    }
    
    /// <inheritdoc/>
    public ICallback GetCallback()
    {
        return Callback;
    }

    /// <inheritdoc/>
    public string GetDimensionName()
    {
        return Name;
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
    public bool Equals(DimensionBase? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name.Equals(other.Name, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is DimensionBase other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Name.GetHashCode(StringComparison.Ordinal);
    }
    
    /// <summary>
    /// Converts the dimension to a string.
    /// </summary>
    /// <param name="dimension">The dimension to convert.</param>
    /// <returns>The name of the dimension.</returns>
    public static implicit operator string (DimensionBase dimension)
    {
        return dimension.Name;
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="DimensionBase"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="DimensionBase"/> to compare.</param>
    /// <param name="right">The second <see cref="DimensionBase"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> represent the same <see cref="DimensionBase"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(DimensionBase left, DimensionBase right)
    {
        return left.Equals(right);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="DimensionBase"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="DimensionBase"/> to compare.</param>
    /// <param name="right">The second <see cref="DimensionBase"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> do not represent the same <see cref="DimensionBase"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(DimensionBase left, DimensionBase right)
    {
        return !left.Equals(right);
    }
}