// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Implementation;
using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Solver using Google OR-Tools.
/// </summary>
public class OrToolsSolver : ISolver
{
    private readonly SolverOptions _options;
    private readonly ISolverModelFactory _solverModelFactory = new DefaultSolverModelFactory();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="OrToolsSolver"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public OrToolsSolver(SolverOptions options)
    {
        _options = options;
    }
    
    /// <summary>
    /// Gets the configurators.
    /// </summary>
    /// <param name="problem">The problem.</param>
    /// <param name="state">The state.</param>
    /// <returns>The configurators.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="problem"/> or <paramref name="state"/> is null.</exception>
    private IReadOnlyList<IConfigurator<Dimension>> GetConfigurators(Problem problem, SolverState<Dimension> state)
    {
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        
        return new List<IConfigurator<Dimension>>()
        {
            new TimeConfigurator(state, problem.DirectedRouteMatrix),
            new VehicleConfigurator(),
            new NodeConfigurator(state),
            new ArcCostEvaluatorConfigurator(problem.DirectedRouteMatrix)
        };
    }
    
    /// <summary>
    /// Configures the solver.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="configurators">The configurators.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> or <paramref name="configurators"/> is null.</exception>
    private void Configure(SolverState<Dimension> state, IReadOnlyList<IConfigurator<Dimension>> configurators)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(configurators, nameof(configurators));
        
        foreach (var configurator in configurators)
        {
            configurator.Configure(state);
        }
    }
    
    /// <summary>
    /// Gets the search parameters.
    /// </summary>
    /// <returns>The search parameters.</returns>
    private RoutingSearchParameters GetSearchParameters()
    {
        var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
        searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
        searchParameters.LocalSearchMetaheuristic = LocalSearchMetaheuristic.Types.Value.GuidedLocalSearch;
        searchParameters.TimeLimit = new Duration { Seconds = _options.MaximumComputeTime.Seconds };
        return searchParameters;
    }
    
    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
        var model = _solverModelFactory.Create(problem);
        var state = new SolverState<Dimension>(model);
        var configurators = GetConfigurators(problem, state);
        Configure(state, configurators);
        var searchParameters = GetSearchParameters();
        using var solution = state.SolverInterface.RoutingModel.SolveWithParameters(searchParameters);
        var outputFactory = new DefaultSolverOutputFactory();
        return outputFactory.CreateOutput(problem, state, solution);
    }
}