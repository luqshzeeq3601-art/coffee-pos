using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/auth");

        group.MapPost("/login", async (
            LoginRequestDto request,
            IAuthService authService,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var response = await authService.LoginAsync(request, cancellationToken);

            if (!string.IsNullOrWhiteSpace(response.Token))
            {
                httpContext.Response.Cookies.Append("coffee_session", response.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                });
            }

            return Results.Ok(response);
        });

        group.MapPost("/mfa/verify", async (
            MfaVerifyRequestDto request,
            IAuthService authService,
            CancellationToken cancellationToken) =>
        {
            var response = await authService.VerifyMfaAsync(request, cancellationToken);
            return Results.Ok(response);
        });

        group.MapPost("/pin-login", async (
            PinLoginRequestDto request,
            IPinAuthService pinAuthService,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var response = await pinAuthService.PinLoginAsync(request, cancellationToken);

            httpContext.Response.Cookies.Append("coffee_session", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(12)
            });

            return Results.Ok(response);
        });

        group.MapPost("/manager-override", async (
            ManagerOverrideRequestDto request,
            IPinAuthService pinAuthService,
            CancellationToken cancellationToken) =>
        {
            var response = await pinAuthService.AuthorizeManagerOverrideAsync(request, cancellationToken);
            return response.Authorized ? Results.Ok(response) : Results.Json(response, statusCode: StatusCodes.Status403Forbidden);
        });

        group.MapGet("/session", async (
            IAuthService authService,
            CancellationToken cancellationToken) =>
        {
            var session = await authService.GetCurrentSessionAsync(cancellationToken);
            return Results.Ok(session);
        });

        group.MapPost("/logout", (HttpContext httpContext) =>
        {
            httpContext.Response.Cookies.Delete("coffee_session");
            return Results.Ok(new { message = "Logged out successfully." });
        });

        return endpoints;
    }
}
