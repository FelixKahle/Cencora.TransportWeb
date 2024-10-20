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
    /// <returns>The configurators.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="problem"/> is null.</exception>
    private IReadOnlyList<IConfigurator<Dimension>> GetConfigurators(Problem problem)
    {
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));
        
        return new List<IConfigurator<Dimension>>()
        {
            new TimeConfigurator(problem.DirectedRouteMatrix),
            new VehicleConfigurator(),
            new NodeConfigurator(),
            new DistanceConfigurator(problem.DirectedRouteMatrix),
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
    
    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
        // Create the solver state.
        using var state = new SolverState<Dimension>(new DefaultSimpleSolverModelFactory(problem));
        
        // Get the configurators and configure the solver.
        var configurators = GetConfigurators(problem);
        Configure(state, configurators);
        
        // Solve the problem.
        var solution = state.Solve(_options.MaximumComputeTime);
        
        // Create the output.
        var outputFactory = new DefaultSolverOutputFactory();
        return outputFactory.CreateOutput(problem, state, solution);
    }
}