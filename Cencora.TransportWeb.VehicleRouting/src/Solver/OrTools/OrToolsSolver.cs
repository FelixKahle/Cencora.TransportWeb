// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools;

/// <summary>
/// Represents a solver that uses the Google OR-Tools library to solve vehicle routing problems.
/// </summary>
public sealed class OrToolsSolver : ISolver
{
    
    /// <inheritdoc/>
    public SolverOutput Solve(Problem problem)
    {
        ArgumentNullException.ThrowIfNull(problem, nameof(problem));
        
        var routeMatrix = problem.DirectedRouteMatrix;
        
        var state = new SolverState(problem);
        using var solverInterface = new SolverInterface(state);
        ICallbackRegistrant callbackRegistrant = new DefaultCallbackRegistrant(solverInterface, state);
        IDimensionRegistrant dimensionRegistrant = new DefaultDimensionRegistrant(solverInterface);

        var timeCallback = new TimeCallback(callbackRegistrant, routeMatrix);
        var timeDimension = new TimeDimension(timeCallback, dimensionRegistrant, state.Vehicles);

        return new SolverOutput();
    }
}