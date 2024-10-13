// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Default implementation of the <see cref="IDimensionRegistrant"/> interface.
/// </summary>
internal sealed class DefaultDimensionRegistrant : IDimensionRegistrant
{
    /// <summary>
    /// The solver interface.
    /// </summary>
    private readonly SolverInterface _solverInterface;

    /// <summary>
    /// The dimensions.
    /// We need to keep a reference to the dimensions to prevent them from being garbage collected.
    /// </summary>
    private readonly List<IDimension> _dimensions;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultDimensionRegistrant"/> class.
    /// </summary>
    /// <param name="solverInterface">The solver interface.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="solverInterface"/> is <see langword="null"/>.</exception>
    public DefaultDimensionRegistrant(SolverInterface solverInterface)
    {
        ArgumentNullException.ThrowIfNull(solverInterface, nameof(solverInterface));

        _solverInterface = solverInterface;
        _dimensions = new List<IDimension>();
    }

    /// <inheritdoc/>
    public RoutingDimension RegisterDimension(ISingleCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));

        var success = _solverInterface.Model.AddDimension(dimension.GetCallback().GetCallbackIndex(), dimension.GetMaxSlackValue(),
            dimension.GetCapacity(), dimension.ShouldStartAtZero(), dimension.GetDimensionName());

        if (!success)
        {
            throw new VehicleRoutingSolverException($"Failed to add dimension '{dimension.GetDimensionName()}'.");
        }
        
        var created = _solverInterface.Model.GetMutableDimension(dimension.GetDimensionName()) ?? 
                      throw new VehicleRoutingSolverException($"Failed to get mutable dimension '{dimension.GetDimensionName()}'.");
        
        _dimensions.Add(dimension);
        return created;
    }

    /// <inheritdoc/>
    public RoutingDimension RegisterDimension(IMultiCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));

        var success = _solverInterface.Model.AddDimensionWithVehicleCapacity(dimension.GetCallback().GetCallbackIndex(), dimension.GetMaxSlackValue(),
            dimension.GetCapacities(), dimension.ShouldStartAtZero(), dimension.GetDimensionName());

        if (!success)
        {
            throw new VehicleRoutingSolverException($"Failed to add dimension '{dimension.GetDimensionName()}'.");
        }

        var created = _solverInterface.Model.GetMutableDimension(dimension.GetDimensionName()) ??
                      throw new VehicleRoutingSolverException($"Failed to get mutable dimension '{dimension.GetDimensionName()}'.");
        
        _dimensions.Add(dimension);
        return created;
    }
    
    /// <inheritdoc/>
    public int GetDimensionCount()
    {
        return _dimensions.Count;
    }
}