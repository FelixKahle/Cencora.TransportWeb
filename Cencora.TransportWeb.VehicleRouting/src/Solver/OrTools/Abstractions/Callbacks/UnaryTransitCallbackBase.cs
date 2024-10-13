// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Represents a unary transit callback.
/// </summary>
internal abstract class UnaryTransitCallbackBase : IUnaryTransitCallback, IEquatable<UnaryTransitCallbackBase>
{
    /// <summary>
    /// The index of the callback.
    /// </summary>
    internal int Index { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryTransitCallbackBase"/> class.
    /// </summary>
    /// <param name="callbackRegistrant">The callback configurator.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="callbackRegistrant"/> is <see langword="null"/>.</exception>
    internal UnaryTransitCallbackBase(ICallbackRegistrant callbackRegistrant)
    {
        ArgumentNullException.ThrowIfNull(callbackRegistrant, nameof(callbackRegistrant));
        
        Index = callbackRegistrant.RegisterCallback(this);
    }

    /// <inheritdoc/>
    public abstract long Callback(Node node);
    
    /// <inheritdoc/>
    public bool Equals(UnaryTransitCallbackBase? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Index.Equals(other.Index);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is UnaryTransitCallbackBase other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Index;
    }

    /// <inheritdoc/>
    public int GetCallbackIndex()
    {
        return Index;
    }

    /// <summary>
    /// Converts the specified <see cref="UnaryTransitCallbackBase"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns>The index of the callback.</returns>
    public static implicit operator int(UnaryTransitCallbackBase callback)
    {
        return callback.Index;
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="UnaryTransitCallbackBase"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="UnaryTransitCallbackBase"/> to compare.</param>
    /// <param name="right">The second <see cref="UnaryTransitCallbackBase"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> represent the same value; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(UnaryTransitCallbackBase left, UnaryTransitCallbackBase right)
    {
        return left.Equals(right);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="UnaryTransitCallbackBase"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="UnaryTransitCallbackBase"/> to compare.</param>
    /// <param name="right">The second <see cref="UnaryTransitCallbackBase"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> do not represent the same value; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(UnaryTransitCallbackBase left, UnaryTransitCallbackBase right)
    {
        return !left.Equals(right);
    }
}