// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

/// <summary>
/// Distance dimension.
/// </summary>
internal sealed class DistanceDimension : IMultiCapacityDimension
{
    private readonly SolverCallback _distanceCallback;
    private readonly SolverModel _model;
    
    /// <summary>
    /// The name.
    /// </summary>
    internal const string Name = "DistanceDimension";
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DistanceDimension"/> class.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="distanceCallback">The distance callback.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="model"/> is <see langword="null"/>.</exception>
    public DistanceDimension(SolverModel model, SolverCallback distanceCallback)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        
        _model = model;
        _distanceCallback = distanceCallback;
    }
    
    /// <inheritdoc/>
    public SolverCallback GetCallback()
    {
        return _distanceCallback;
    }

    /// <inheritdoc/>
    public string GetName()
    {
        return Name;
    }

    /// <inheritdoc/>
    public long GetMaxSlack()
    {
        return 0;
    }

    /// <inheritdoc/>
    public bool ShouldStartAtZero()
    {
        return true;
    }

    /// <inheritdoc/>
    public long[] GetCapacities()
    {
        return _model.Vehicles.Select(vehicle => vehicle.MaxDistance).ToArray();
    }
}