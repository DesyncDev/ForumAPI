using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentValidation;

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

        context.Response.ContentType = "application/problem+json";

        object problemDetails;

        if (ex is ValidationException validationException)
        {
            context.Response.StatusCode = 400;

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            problemDetails = new
            {
                type = "https://httpstatuses.com/400",
                title = "Validation error",
                status = 400,
                errors,
                instance = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            };
        }
        else
        {
            context.Response.StatusCode = 500;

            problemDetails = _env.IsDevelopment()
                ? new
                {
                    type = "https://httpstatuses.com/500",
                    title = ex.Message,
                    status = 500,
                    detail = ex.StackTrace,
                    instance = context.TraceIdentifier,
                    timestamp = DateTime.UtcNow
                }
                : new
                {
                    type = "https://httpstatuses.com/500",
                    title = "Internal server error",
                    status = 500,
                    detail = "An unexpected error occurred. Please try again later.",
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