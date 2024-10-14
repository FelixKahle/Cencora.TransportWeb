// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Implementation;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Solver using Google OR-Tools.
/// </summary>
public class OrToolsSolver : ISolver
{
    /// <summary>
    /// The model factory.
    /// </summary>
    private readonly ISolverModelFactory _solverModelFactory = new DefaultSolverModelFactory();
    
    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
        var model = _solverModelFactory.Create(problem);
        var state = new SolverState(model);
        
        var configurators = GetConfigurators(state, problem);
        Configure(state, configurators);

        return new SolverOutput();
    }
    
    /// <summary>
    /// Configures the solver.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="configurators">The configurators.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> or <paramref name="configurators"/> is <see langword="null"/>.</exception>
    private void Configure(SolverState state, IReadOnlyList<IConfigurator> configurators)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(configurators, nameof(configurators));
        
        foreach (var configurator in configurators)
        {
            configurator.Configure(state);
        }
    }
    
    /// <summary>
    /// Gets the configurators.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="problem">The problem.</param>
    /// <returns>The configurators.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> or <paramref name="problem"/> is <see langword="null"/>.</exception>
    private List<IConfigurator> GetConfigurators(SolverState state, Problem problem)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));
        
        return new List<IConfigurator>
        {
            new TimeConfigurator(state, problem.DirectedRouteMatrix)
        };
    }
}