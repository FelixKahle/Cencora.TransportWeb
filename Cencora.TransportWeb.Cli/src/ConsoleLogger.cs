// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Microsoft.Extensions.Logging;

namespace Cencora.TransportWeb.Cli;

/// <summary>
/// A logger that writes log messages to the console.
/// </summary>
public class ConsoleLogger<T> : ILogger<T>, IDisposable
{
    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        // Construct the log message with more details (timestamp, log level, event ID, etc.)
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logLevelString = logLevel.ToString().ToUpper();
        var eventIdString = eventId.Id != 0 ? $"[EventId: {eventId.Id}]" : string.Empty;
        var exceptionMessage = exception != null ? $" | Exception: {exception.Message}" : string.Empty;

        // Format the log message
        var message = $"{timestamp} [{logLevelString}] {eventIdString} {formatter(state, exception)}{exceptionMessage}";

        Console.ForegroundColor = logLevel switch
        {
            LogLevel.Error or LogLevel.Critical => ConsoleColor.Red,
            LogLevel.Warning => ConsoleColor.Yellow,
            _ => ConsoleColor.White
        };

        Console.WriteLine(message);

        // Reset the console color
        Console.ResetColor();
    }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    /// <inheritdoc/>
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return this;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }
}