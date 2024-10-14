// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

/// <summary>
/// Time dimension.
/// </summary>
internal sealed class TimeDimension : IMultiCapacityDimension
{
    private readonly SolverCallback _timeCallback;
    private readonly SolverModel _model;
    
    /// <summary>
    /// The name.
    /// </summary>
    internal const string Name = "TimeDimension";
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeDimension"/> class.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="timeCallback">The time callback.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="model"/> is <see langword="null"/>.</exception>
    public TimeDimension(SolverModel model, SolverCallback timeCallback)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        
        _model = model;
        _timeCallback = timeCallback;
    }
    
    
    /// <inheritdoc/>
    public SolverCallback GetCallback()
    {
        return _timeCallback;
    }

    /// <inheritdoc/>
    public string GetName()
    {
        return Name;
    }

    /// <inheritdoc/>
    public long GetMaxSlack()
    {
        return long.MaxValue;
    }

    /// <inheritdoc/>
    public bool ShouldStartAtZero()
    {
        return false;
    }

    /// <inheritdoc/>
    public long[] GetCapacities()
    {
        return _model.Vehicles.Select(v => v.MaxDuration).ToArray();
    }
}