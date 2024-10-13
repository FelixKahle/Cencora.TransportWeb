// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Implementation;

/// <summary>
/// Default implementation of the <see cref="IDimensionRegistrant"/> interface.
/// </summary>
internal sealed class DefaultDimensionRegistrant : IDimensionRegistrant
{
    /// <summary>
    /// The state.
    /// </summary>
    private readonly SolverState _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultDimensionRegistrant"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
    public DefaultDimensionRegistrant(SolverState state)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));

        _state = state;
    }

    /// <inheritdoc/>
    public bool RegisterDimension(ISingleCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));

        return _state.Model.AddDimension(dimension.GetCallback().GetCallbackIndex(), dimension.GetMaxSlackValue(),
            dimension.GetCapacity(), dimension.ShouldStartAtZero(), dimension.GetDimensionName());
    }

    /// <inheritdoc/>
    public bool RegisterDimension(IMultiCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));

        return _state.Model.AddDimensionWithVehicleCapacity(dimension.GetCallback().GetCallbackIndex(), dimension.GetMaxSlackValue(),
            dimension.GetCapacities(), dimension.ShouldStartAtZero(), dimension.GetDimensionName());
    }
}