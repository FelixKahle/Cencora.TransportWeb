// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Implementation;

/// <summary>
/// Represents the default implementation of the simple solver model factory.
/// </summary>
internal sealed class DefaultSimpleSolverModelFactory : DefaultSolverModelFactory, ISimpleSolverModelFactory
{
    private readonly Problem _problem;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultSimpleSolverModelFactory"/> class.
    /// </summary>
    /// <param name="problem">The problem.</param>
    /// <exception cref="ArgumentNullException">Thrown when the problem is null.</exception>
    public DefaultSimpleSolverModelFactory(Problem problem)
    {
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));
        
        _problem = problem;
    }
    
    /// <inheritdoc/>
    public SolverModel Create()
    {
        // Use the implementation of the default solver model factory to create the solver model
        // to avoid code duplication.
        return Create(_problem);
    }
}