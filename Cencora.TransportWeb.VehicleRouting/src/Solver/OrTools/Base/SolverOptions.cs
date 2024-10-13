// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Base;

/// <summary>
/// Options for the Google OR-Tools solver.
/// </summary>
public struct SolverOptions
{
    /// <summary>
    /// The maximum time the solver is allowed to run.
    /// </summary>
    public TimeSpan MaximumComputeTime { get; }
    
    /// <summary>
    /// Creates a new instance of the <see cref="SolverOptions"/> struct.
    /// </summary>
    /// <param name="maximumComputeTime">The maximum time the solver is allowed to run.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the maximumComputeTime is negative.</exception>
    public SolverOptions(TimeSpan maximumComputeTime)
    {
        if (maximumComputeTime < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumComputeTime), "Compute time cannot be negative.");
        }

        MaximumComputeTime = maximumComputeTime;
    }
}