// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

/// <summary>
/// Registers dimensions with the solver.
/// </summary>
internal sealed class DefaultDimensionRegistrant : IDimensionRegistrant
{
    private readonly SolverModel _model;
    private readonly RoutingModel _routingModel;
    private readonly Dictionary<IDimension, SolverDimension> _dimensions = new();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultDimensionRegistrant"/> class.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="routingModel">The routing model.</param>
    internal DefaultDimensionRegistrant(SolverModel model, RoutingModel routingModel)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        ArgumentNullException.ThrowIfNull(routingModel, nameof(routingModel));
        
        _model = model;
        _routingModel = routingModel;
    }
    
    /// <inheritdoc/>
    public SolverDimension RegisterDimension(ISingleCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));

        var callback = dimension.GetCallback();
        var capacity = dimension.GetCapacity();
        var maxSlack = dimension.GetMaxSlack();
        var shouldStartAtZero = dimension.ShouldStartAtZero();
        var name = dimension.GetName();
        
        // Register the dimension and check if it was successful.
        var success = _routingModel.AddDimension(callback, maxSlack, capacity, shouldStartAtZero, name);
        if (!success)
        {
            throw new VehicleRoutingSolverException($"The dimension '{name}' could not be registered.");
        }

        var createdDimension = new SolverDimension(name, _routingModel.GetMutableDimension(name));
        _dimensions.TryAdd(dimension, createdDimension);
        return createdDimension;
    }

    /// <inheritdoc/>
    public SolverDimension RegisterDimension(IMultiCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));

        var callback = dimension.GetCallback();
        var capacities = dimension.GetCapacities();
        var maxSlacks = dimension.GetMaxSlack();
        var shouldStartAtZeros = dimension.ShouldStartAtZero();
        var name = dimension.GetName();
        
        // Check if the number of capacities matches the number of vehicles.
        if (capacities.Length != _model.VehicleCount)
        {
            throw new VehicleRoutingSolverException("The number of capacities must match the number of vehicles.");
        }
        
        // Register the dimension and check if it was successful.
        var success = _routingModel.AddDimensionWithVehicleCapacity(callback, maxSlacks, capacities, shouldStartAtZeros, name);
        if (!success)
        {
            throw new VehicleRoutingSolverException($"The dimension '{name}' could not be registered.");
        }

        var createdDimension = new SolverDimension(name, _routingModel.GetMutableDimension(name));
        _dimensions.TryAdd(dimension, createdDimension);
        return createdDimension;
    }
}