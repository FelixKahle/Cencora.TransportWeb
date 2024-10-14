// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

/// <summary>
/// Manages the registration of dimensions with the solver.
/// </summary>
internal sealed class SolverDimensionManager
{
    private readonly RoutingModel _routingModel;
    private readonly SolverCallbackManager _callbackManager;
    private readonly Dictionary<IDimension, SolverDimension> _dimensions = new();
    private readonly IIndexCreator _indexCreator = new ZeroBasedIndexCreator();
    private readonly int _vehicleCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="SolverDimensionManager"/> class.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="routingModel">The routing model.</param>
    /// <param name="callbackManager">The callback manager.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="routingModel"/> or <paramref name="callbackManager"/> is <see langword="null"/>.</exception>
    internal SolverDimensionManager(SolverModel model, RoutingModel routingModel, SolverCallbackManager callbackManager)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        ArgumentNullException.ThrowIfNull(routingModel, nameof(routingModel));
        ArgumentNullException.ThrowIfNull(callbackManager, nameof(callbackManager));
        
        _routingModel = routingModel;
        _callbackManager = callbackManager;
        _vehicleCount = model.VehicleCount;
    }

    /// <summary>
    /// Determines whether the specified dimension is registered.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <returns><see langword="true"/> if the specified dimension is registered; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dimension"/> is <see langword="null"/>.</exception>
    internal bool IsDimensionRegistered(IDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        
        return _dimensions.ContainsKey(dimension);
    }

    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dimension"/> is <see langword="null"/>.</exception>
    /// <exception cref="VehicleRoutingSolverException">Thrown if the dimension could not be registered.</exception>
    /// <exception cref="VehicleRoutingSolverException">Thrown if the dimension could not be retrieved.</exception>
    public void RegisterDimension(ISingleCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        
        if (IsDimensionRegistered(dimension))
        {
            return;
        }

        _callbackManager.RegisterCallback(dimension.GetCallback());
        var callback = _callbackManager.Get(dimension.GetCallback());
        var name = _indexCreator.GetNext().ToString();

        if (!_routingModel.AddDimension(
                callback,
                dimension.GetMaxSlack(),
                dimension.GetCapacity(),
                dimension.ShouldStartAtZero(),
                name))
        {
            throw new VehicleRoutingSolverException("Failed to create dimension.");
        }

        var newDimension = _routingModel.GetMutableDimension(name)
                          ?? throw new VehicleRoutingSolverException("Failed to get dimension.");

        _dimensions[dimension] = new SolverDimension(name, newDimension);
    }

    /// <summary>
    /// Registers the specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dimension"/> is <see langword="null"/>.</exception>
    /// <exception cref="VehicleRoutingSolverException">Thrown if the dimension could not be registered.</exception>
    /// <exception cref="VehicleRoutingSolverException">Thrown if the dimension could not be retrieved.</exception>
    /// <exception cref="VehicleRoutingSolverException">Thrown if the number of capacities does not match the number of vehicles.</exception>
    public void RegisterDimension(IMultiCapacityDimension dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension, nameof(dimension));
        
        if (IsDimensionRegistered(dimension))
        {
            return;
        }

        _callbackManager.RegisterCallback(dimension.GetCallback());
        var callback = _callbackManager.Get(dimension.GetCallback());
        var name = _indexCreator.GetNext().ToString();
        var capacities = dimension.GetCapacities();
        
        // Make sure the number of capacities matches the number of vehicles.
        if (capacities.Length != _vehicleCount)
        {
            throw new VehicleRoutingSolverException("The number of capacities must match the number of vehicles.");
        }

        if (!_routingModel.AddDimensionWithVehicleCapacity(
                callback,
                dimension.GetMaxSlack(),
                dimension.GetCapacities(),
                dimension.ShouldStartAtZero(),
                name))
        {
            throw new VehicleRoutingSolverException("Failed to create dimension.");
        }

        var newDimension = _routingModel.GetMutableDimension(name)
                          ?? throw new VehicleRoutingSolverException("Failed to get dimension.");

        _dimensions[dimension] = new SolverDimension(name, newDimension);
    }

    /// <summary>
    /// Gets the count of registered dimensions.
    /// </summary>
    public int DimensionCount => _dimensions.Count;
}