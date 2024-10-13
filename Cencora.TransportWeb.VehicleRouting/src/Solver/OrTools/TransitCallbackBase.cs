// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de


using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Nodes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Represents a callback for the transit.
/// </summary>
internal abstract class TransitCallbackBase : ITransitCallback, IEquatable<TransitCallbackBase>
{
    /// <summary>
    /// The index of the callback.
    /// </summary>
    internal int Index { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransitCallbackBase"/> class.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    internal TransitCallbackBase(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));

        Index = new DefaultCallbackConfigurator(state).RegisterCallback(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransitCallbackBase"/> class.
    /// </summary>
    /// <param name="callbackConfigurator">The callback configurator.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="callbackConfigurator"/> is <see langword="null"/>.</exception>
    internal TransitCallbackBase(ICallbackConfigurator callbackConfigurator)
    {
        ArgumentNullException.ThrowIfNull(callbackConfigurator, nameof(callbackConfigurator));
        
        Index = callbackConfigurator.RegisterCallback(this);
    }

    /// <inheritdoc/>
    public abstract long Callback(Node from, Node to);

    /// <inheritdoc/>
    public bool Equals(TransitCallbackBase? other)
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
        return obj is TransitCallbackBase other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Index;
    }
    
    /// <summary>
    /// Converts a <see cref="TransitCallbackBase"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns>The index of the callback.</returns>
    public static implicit operator int (TransitCallbackBase callback)
    {
        return callback.Index;
    }
    
    /// <summary>
    /// Determines whether two instances of <see cref="TransitCallbackBase"/> are equal.
    /// </summary>
    /// <param name="left">The left instance.</param>
    /// <param name="right">The right instance.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(TransitCallbackBase? left, TransitCallbackBase? right)
    {
        return Equals(left, right);
    }
    
    /// <summary>
    /// Determines whether two instances of <see cref="TransitCallbackBase"/> are not equal.
    /// </summary>
    /// <param name="left">The left instance.</param>
    /// <param name="right">The right instance.</param>
    /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(TransitCallbackBase? left, TransitCallbackBase? right)
    {
        return !Equals(left, right);
    }
}