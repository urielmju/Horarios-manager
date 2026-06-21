using System.Net;
using System.Text.Json;

namespace ScheduleSystem.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message) = ex switch
        {
            ArgumentException or InvalidOperationException => (400, ex.Message),
            UnauthorizedAccessException                    => (401, ex.Message),
            KeyNotFoundException                           => (404, ex.Message),
            _                                              => (500, "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = statusCode;

        var payload = JsonSerializer.Serialize(new { message, statusCode });
        return context.Response.WriteAsync(payload);
    }
}
