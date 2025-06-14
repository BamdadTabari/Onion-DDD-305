using System.Text;
using Microsoft.Extensions.Options;
using _305.BuildingBlocks.Configurations;

namespace _305.WebApi.Assistants.Logging;

/// <summary>
/// Writes request logs to a file.
/// </summary>
public class FileLogWriter : ILogWriter
{
    private readonly string _logPath;

    public FileLogWriter(IOptions<RequestLoggingConfig> options)
    {
        _logPath = options.Value.FilePath;
        EnsureDirectory(_logPath);
    }

    /// <inheritdoc />
    public async Task WriteAsync(string logLine)
    {
        var bytes = Encoding.UTF8.GetBytes(logLine);
        await using var stream = new FileStream(_logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, useAsync: true);
        await stream.WriteAsync(bytes, 0, bytes.Length);
    }

    private static void EnsureDirectory(string path)
    {
        var dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir!);
    }
}
