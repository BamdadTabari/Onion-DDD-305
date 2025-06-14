using System.Text;
using _305.BuildingBlocks.IService;
using _305.WebApi.Assistants.Logging;

namespace _305.WebApi.Assistants.Middleware;

/// <summary>
/// Middleware برای لاگ گرفتن درخواست‌ها در فایل متنی.
/// </summary>
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogWriter _logWriter;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LoggingMiddleware(RequestDelegate next, ILogWriter logWriter, IDateTimeProvider dateTimeProvider)
    {
        _next = next;
        _logWriter = logWriter;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// اجرای لاگ و عبور به مرحله بعدی.
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        var logLine = $"{_dateTimeProvider.Now:yyyy-MM-dd HH:mm:ss} | {context.Request.Method} {context.Request.Path} | Origin: {context.Request.Headers["Origin"]}\n";
        await _logWriter.WriteAsync(logLine);
        await _next(context);
    }
}
