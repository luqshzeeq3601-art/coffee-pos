using System.Security.Claims;
using CoffeePos.Application.Common;
using CoffeePos.Application.Interfaces;
using CoffeePos.Infrastructure.Security;
using Microsoft.AspNetCore.Http;

namespace CoffeePos.ApiHost.Middleware;

public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITokenService tokenService,
        ITenantContext tenantContext,
        ICurrentUser currentUser)
    {
        string? token = null;

        if (context.Request.Cookies.TryGetValue("coffee_session", out var cookieToken) && !string.IsNullOrWhiteSpace(cookieToken))
        {
            token = cookieToken;
        }
        else if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var headerValue = authHeader.ToString();
            if (headerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = headerValue["Bearer ".Length..].Trim();
            }
        }

        if (!string.IsNullOrWhiteSpace(token))
        {
            var principal = tokenService.ValidateToken(token);
            if (principal != null)
            {
                context.User = principal;

                var tenantIdClaim = principal.FindFirst("tenant_id")?.Value;
                var outletIdClaim = principal.FindFirst("outlet_id")?.Value;
                var deviceIdClaim = principal.FindFirst("device_id")?.Value;
                var subClaim = principal.FindFirst("sub")?.Value;
                var userTypeClaim = principal.FindFirst("user_type")?.Value ?? "User";
                var nameClaim = principal.FindFirst("name")?.Value ?? "Authenticated User";
                var roles = principal.FindAll("role").Select(static c => c.Value).ToList();
                var permissions = principal.FindAll("permissions").Select(static c => c.Value).ToList();

                if (Guid.TryParse(tenantIdClaim, out var tenantId))
                {
                    Guid? outletId = Guid.TryParse(outletIdClaim, out var parsedOutletId) ? parsedOutletId : null;
                    Guid? deviceId = Guid.TryParse(deviceIdClaim, out var parsedDeviceId) ? parsedDeviceId : null;

                    tenantContext.SetContext(tenantId, outletId, deviceId);
                }

                if (tenantContext is TenantContext mutableContext && !string.IsNullOrWhiteSpace(subClaim))
                {
                    mutableContext.SetUser(subClaim, userTypeClaim, nameClaim, roles, permissions);
                }
            }
        }

        await _next(context);
    }
}
