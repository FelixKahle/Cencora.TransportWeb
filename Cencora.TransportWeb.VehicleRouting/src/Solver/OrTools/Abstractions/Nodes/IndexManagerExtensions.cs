// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.State;
using Google.OrTools.ConstraintSolver;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Nodes;

/// <summary>
/// Contains extension methods for the <see cref="RoutingIndexManager"/> class.
/// </summary>
internal static class IndexManagerExtensions
{
    /// <summary>
    /// Converts the given index to a node.
    /// </summary>
    /// <param name="indexManager">The index manager.</param>
    /// <param name="model">The model.</param>
    /// <param name="index">The index.</param>
    /// <returns>The node.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexManager"/> or <paramref name="model"/> is <see langword="null"/>.</exception>
    internal static Node IndexToNode(this RoutingIndexManager indexManager, SolverModel model, long index)
    {
        ArgumentNullException.ThrowIfNull(indexManager, nameof(indexManager));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        
        var nodeIndex = indexManager.IndexToNode(index);
        return model.Nodes[nodeIndex];
    }
    
    /// <summary>
    /// Converts the given node to an index.
    /// </summary>
    /// <param name="indexManager">The index manager.</param>
    /// <param name="model">The model.</param>
    /// <param name="node">The node.</param>
    /// <returns>The index.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexManager"/>, <paramref name="model"/>, or <paramref name="node"/> is <see langword="null"/>.</exception>
    internal static long NodeToIndex(this RoutingIndexManager indexManager, SolverModel model, Node node)
    {
        ArgumentNullException.ThrowIfNull(indexManager, nameof(indexManager));
        ArgumentNullException.ThrowIfNull(node, nameof(node));
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        
        var nodeIndex = model.Nodes.IndexOf(node);
        return indexManager.NodeToIndex(nodeIndex);
    }
}