using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class SaasEndpoints
{
    public static IEndpointRouteBuilder MapSaasEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/saas")
            .WithTags("Multi-Tenant SaaS Subscriptions & Branding");

        group.MapGet("/subscription", async (ISubscriptionService subscriptionService, CancellationToken ct) =>
        {
            var sub = await subscriptionService.GetSubscriptionAsync(ct);
            return Results.Ok(sub);
        })
        .WithName("GetTenantSubscription")
        .Produces<TenantSubscriptionDto>(StatusCodes.Status200OK);

        group.MapGet("/entitlements", async (ISubscriptionService subscriptionService, CancellationToken ct) =>
        {
            var ent = await subscriptionService.GetEntitlementsAsync(ct);
            return Results.Ok(ent);
        })
        .WithName("GetTenantEntitlements")
        .Produces<EntitlementsDto>(StatusCodes.Status200OK);

        group.MapPost("/upgrade", async (UpgradeSubscriptionRequest request, ISubscriptionService subscriptionService, CancellationToken ct) =>
        {
            var updated = await subscriptionService.UpgradeSubscriptionAsync(request, ct);
            return Results.Ok(updated);
        })
        .WithName("UpgradeTenantSubscription")
        .Produces<TenantSubscriptionDto>(StatusCodes.Status200OK);

        group.MapGet("/branding", async (ISubscriptionService subscriptionService, CancellationToken ct) =>
        {
            var branding = await subscriptionService.GetBrandingAsync(ct);
            return Results.Ok(branding);
        })
        .WithName("GetTenantBranding")
        .Produces<TenantBrandingDto>(StatusCodes.Status200OK);

        group.MapPut("/branding", async (UpdateBrandingRequest request, ISubscriptionService subscriptionService, CancellationToken ct) =>
        {
            var updated = await subscriptionService.UpdateBrandingAsync(request, ct);
            return Results.Ok(updated);
        })
        .WithName("UpdateTenantBranding")
        .Produces<TenantBrandingDto>(StatusCodes.Status200OK);

        return routes;
    }
}
