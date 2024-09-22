// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Diagnostics;
using Microsoft.Extensions.Logging;

// ReSharper disable MemberCanBePrivate.Global

namespace Cencora.TransportWeb.Common.Profiling;

/// <summary>
/// A utility struct for profiling the execution time of a scope.
/// When disposed, it logs the elapsed time since its creation using the provided <see cref="ILogger"/>.
/// </summary>
public readonly struct ScopeProfiler : IDisposable
{
    private readonly Stopwatch _stopwatch;
    private readonly ILogger _logger;

    /// <summary>
    /// The name of the scope profiler.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The log level at which the elapsed time will be logged.
    /// </summary>
    public LogLevel LogLevel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopeProfiler"/> struct with the specified logger and log level.
    /// The elapsed time will be logged when the instance is disposed.
    /// </summary>
    /// <param name="logger">The logger used to log the elapsed time.</param>
    /// <param name="logLevel">The log level at which the elapsed time will be logged. Default is <see cref="LogLevel.Debug"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is <c>null</c>.</exception>
    public ScopeProfiler(ILogger logger, LogLevel logLevel = LogLevel.Debug)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        
        Name = String.Empty;
        _logger = logger;
        LogLevel = logLevel;
        _stopwatch = Stopwatch.StartNew();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopeProfiler"/> struct with the specified name, logger, and log level.
    /// The elapsed time will be logged when the instance is disposed.
    /// </summary>
    /// <param name="name">The name of the scope being profiled. It will be included in the log output.</param>
    /// <param name="logger">The logger used to log the elapsed time.</param>
    /// <param name="logLevel">The log level at which the elapsed time will be logged. Default is <see cref="LogLevel.Debug"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="logger"/> is <c>null</c>.</exception>
    public ScopeProfiler(string name, ILogger logger, LogLevel logLevel = LogLevel.Debug)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        
        Name = name;
        _logger = logger;
        LogLevel = logLevel;
        _stopwatch = Stopwatch.StartNew();
    }
    
    /// <summary>
    /// Stops the stopwatch and logs the elapsed time.
    /// If a name was provided during initialization, the name is included in the log message.
    /// </summary>
    public void Dispose()
    {
        _stopwatch.Stop();
        _logger.Log(LogLevel, String.IsNullOrEmpty(Name)
            ? $"Elapsed time: {_stopwatch.Elapsed}"
            : $"Elapsed time for '{Name}': {_stopwatch.Elapsed}");
    }
}