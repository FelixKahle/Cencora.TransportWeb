// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Model.RouteMatrix;
using Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools.Nodes;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.GoogleOrTools;

/// <summary>
/// Base class for Google OR-Tools solvers.
/// </summary>
public abstract class GoogleOrToolsSolverBase
{
    private RoutingIndexManager? _indexManager;
    private RoutingModel? _routingModel;
    private List<Node>? _nodes;
    private List<DummyVehicle>? _vehicles;
    private Dictionary<DummyVehicle, int>? _vehiclesToTransitCallbackIndex;
    private IReadOnlyDirectedRouteMatrix? _routeMatrix;

    /// <summary>
    /// Gets the <see cref="GetRoutingIndexManager"/>.
    /// </summary>
    /// <returns>The routing index manager.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the routing index manager has not been initialized.</exception>
    internal RoutingIndexManager GetRoutingIndexManager()
    {
        return _indexManager ?? throw new InvalidOperationException("The routing index manager has not been initialized.");
    }

    /// <summary>
    /// Gets the <see cref="RoutingModel"/>.
    /// </summary>
    /// <returns>The routing model.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the routing model has not been initialized.</exception>
    internal RoutingModel GetRoutingModel()
    {
        return _routingModel ?? throw new InvalidOperationException("The routing model has not been initialized.");
    }

    /// <summary>
    /// Gets the nodes of the solver.
    /// </summary>
    /// <returns>The nodes of the solver.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the nodes have not been initialized.</exception>
    internal List<Node> GetNodes()
    {
        return _nodes ?? throw new InvalidOperationException("The nodes have not been initialized.");
    }

    /// <summary>
    /// Gets the vehicles of the solver.
    /// </summary>
    /// <returns>The vehicles of the solver.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles have not been initialized.</exception>
    internal List<DummyVehicle> GetVehicles()
    {
        return _vehicles ?? throw new InvalidOperationException("The vehicles have not been initialized.");
    }

    /// <summary>
    /// Gets the vehicles to transit callback index.
    /// </summary>
    /// <returns>The vehicles to transit callback index.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the vehicles to transit callback index has not been initialized.</exception>
    internal Dictionary<DummyVehicle, int> GetVehiclesToTransitCallbackIndex()
    {
        return _vehiclesToTransitCallbackIndex ?? throw new InvalidOperationException("The vehicles to transit callback index has not been initialized.");
    }

    /// <summary>
    /// Gets the route matrix of the solver.
    /// </summary>
    /// <returns>The route matrix of the solver.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the route matrix has not been initialized.</exception>
    internal IReadOnlyDirectedRouteMatrix GetRouteMatrix()
    {
        return _routeMatrix ?? throw new InvalidOperationException("The route matrix has not been initialized.");
    }

    /// <summary>
    /// Returns the distance between two nodes.
    /// </summary>
    /// <param name="fromNode">The node to start from.</param>
    /// <param name="toNode">The node to end at.</param>
    /// <returns>The distance between the two nodes.</returns>
    internal long GetDistance(Node fromNode, Node toNode)
    {
        var routeMatrix = GetRouteMatrix();

        var fromLocation = fromNode.GetLocation();
        var toLocation = toNode.GetLocation();

        // Arbitrary nodes have a distance of 0.
        if (fromLocation is null || toLocation is null)
        {
            return 0;
        }

        return routeMatrix.GetEdge(fromLocation, toLocation) switch
        {
            DefinedRouteEdge definedRouteEdge => definedRouteEdge.Distance,
            _ => long.MaxValue
        };
    }
}
