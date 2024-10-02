// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.TransportWeb.VehicleRouting.Solver;

/// <summary>
/// Represents an exception specific to the Vehicle Routing Solver.
/// </summary>
public class VehicleRoutingSolverException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleRoutingSolverException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public VehicleRoutingSolverException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleRoutingSolverException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public VehicleRoutingSolverException(string message, Exception innerException) : base(message, innerException)
    {
    }
}