// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Configurators;
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
    
    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
        var model = _solverModelFactory.Create(problem);
        var state = new SolverState(model);

        var timeConfigurator = new TimeConfigurator(state, problem.DirectedRouteMatrix);
        var vehicleConfigurator = new VehicleConfigurator();
        var nodeConfigurator = new NodeConfigurator(state);
        var arcCostConfigurator = new ArcCostEvaluatorConfigurator(problem.DirectedRouteMatrix);
        
        timeConfigurator.Configure(state);
        vehicleConfigurator.Configure(state);
        nodeConfigurator.Configure(state);
        arcCostConfigurator.Configure(state);
        
        var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
        searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
        searchParameters.LocalSearchMetaheuristic = LocalSearchMetaheuristic.Types.Value.GuidedLocalSearch;
        searchParameters.TimeLimit = new Duration { Seconds = _options.MaximumComputeTime.Seconds };
        using var solution = state.SolverInterface.RoutingModel.SolveWithParameters(searchParameters);

        var outputFactory = new DefaultSolverOutputFactory(timeConfigurator.TimeDimension);
        return outputFactory.CreateOutput(problem, state, solution);
    }
}