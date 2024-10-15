// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Vehicles;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;

/// <summary>
/// Represents a callback that evaluates the cost of an arc.
/// </summary>
internal class ArcCostEvaluatorCallback : ITransitCallback
{
    private readonly IArcCostEvaluator _costEvaluator;
    private readonly DummyVehicle _dummyVehicle;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ArcCostEvaluatorCallback"/> class.
    /// </summary>
    /// <param name="costEvaluator">The cost evaluator.</param>
    /// <param name="dummyVehicle">The dummy vehicle.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="costEvaluator"/> or <paramref name="dummyVehicle"/> is <see langword="null"/>.</exception>
    internal ArcCostEvaluatorCallback(IArcCostEvaluator costEvaluator, DummyVehicle dummyVehicle)
    {
        ArgumentNullException.ThrowIfNull(costEvaluator, nameof(costEvaluator));
        ArgumentNullException.ThrowIfNull(dummyVehicle, nameof(dummyVehicle));
        
        _costEvaluator = costEvaluator;
        _dummyVehicle = dummyVehicle;
    }
    
    public long GetTransit(Node fromNode, Node toNode)
    {
        return _costEvaluator.GetCost(_dummyVehicle, fromNode, toNode);
    }
}