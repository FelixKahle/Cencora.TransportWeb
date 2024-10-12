// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.Common.Id;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Represents a dimension of a vehicle routing problem.
/// </summary>
internal abstract class DimensionBase : IDimension, IEquatable<DimensionBase>
{
    /// <summary>
    /// The internal Id of the dimension.
    /// </summary>
    public Id Id { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionBase"/> class.
    /// </summary>
    internal DimensionBase()
    {
        Id = NextId();
    }
    
    /// <summary>
    /// Keeps track of the next id to be assigned to a dimension.
    /// </summary>
    private static int _idCounter = int.MinValue;

    /// <summary>
    /// Creates a new Id for a dimension.
    /// </summary>
    /// <returns>The new Id.</returns>
    private static Id NextId()
    {
        int id = _idCounter++;
        return new Id(id.ToString());
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
        
        return Id.Equals(other.Id);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is DimensionBase other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="DimensionBase"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="DimensionBase"/> to compare.</param>
    /// <param name="right">The second <see cref="DimensionBase"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> represent the same dimension; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(DimensionBase? left, DimensionBase? right)
    {
        return Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="DimensionBase"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="DimensionBase"/> to compare.</param>
    /// <param name="right">The second <see cref="DimensionBase"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> do not represent the same dimension; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(DimensionBase? left, DimensionBase? right)
    {
        return !Equals(left, right);
    }
}