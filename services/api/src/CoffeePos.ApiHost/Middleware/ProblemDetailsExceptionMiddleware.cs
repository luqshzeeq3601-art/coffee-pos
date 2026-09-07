using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace CoffeePos.ApiHost.Middleware;

public sealed class ProblemDetailsExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ProblemDetailsExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail) = exception switch
        {
            ArgumentException argEx => (StatusCodes.Status400BadRequest, "Bad Request", argEx.Message),
            UnauthorizedAccessException unauthEx => (StatusCodes.Status401Unauthorized, "Unauthorized", unauthEx.Message),
            KeyNotFoundException notFoundEx => (StatusCodes.Status404NotFound, "Not Found", notFoundEx.Message),
            InvalidOperationException invEx => (StatusCodes.Status409Conflict, "Conflict", invEx.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var problemDetails = new
        {
            type = $"https://httpstatuses.com/{statusCode}",
            title = title,
            status = statusCode,
            detail = detail,
            instance = context.Request.Path.Value
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
