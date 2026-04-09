using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForumAPI.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Error not handled in {Method} {Path}", 
            context.Request.Method, 
            context.Request.Path);

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new
        {
            type = "https://httpstatuses.com/500",
            title = "Internal server error",
            status = 500,
            detail = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred. Please try again later.",
            instance = context.TraceIdentifier,
            timestamp = DateTime.UtcNow
        };

        if (_env.IsDevelopment())
        {
            problemDetails = new
            {
                type = "https://httpstatuses.com/500",
                title = ex.Message,
                status = 500,
                detail = ex.StackTrace,
                instance = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            }!;
        }

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }
}