namespace _305.WebApi.Assistants.Logging;

/// <summary>
/// Abstraction for writing log data.
/// </summary>
public interface ILogWriter
{
    /// <summary>Writes a log entry.</summary>
    Task WriteAsync(string logLine);
}
