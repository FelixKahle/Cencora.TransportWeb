// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Callbacks;
using Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Abstractions.Dimensions;

namespace Cencora.TransportWeb.VehicleRouting.Solver.OrTools.Dimensions;

/// <summary>
/// Node dimension.
/// </summary>
internal sealed class NodeDimension : ISingleCapacityDimension
{
    private SolverCallback _nodeCallback { get; }
    
    /// <summary>
    /// The name.
    /// </summary>
    internal const string Name = "NodeDimension";
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeDimension"/> class.
    /// </summary>
    /// <param name="nodeCallback">The node callback.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="nodeCallback"/> is <see langword="null"/>.</exception>
    internal NodeDimension(SolverCallback nodeCallback)
    {
        ArgumentNullException.ThrowIfNull(nodeCallback, nameof(nodeCallback));
        
        _nodeCallback = nodeCallback;
    }
    
    /// <inheritdoc/>
    public SolverCallback GetCallback()
    {
        return _nodeCallback;
    }

    /// <inheritdoc/>
    public string GetName()
    {
        return Name;
    }

    /// <inheritdoc/>
    public long GetMaxSlack()
    {
        return 0;
    }

    /// <inheritdoc/>
    public bool ShouldStartAtZero()
    {
        return true;
    }

    /// <inheritdoc/>
    public long GetCapacity()
    {
        return long.MaxValue;
    }
}