using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class KitchenEndpoints
{
    public static IEndpointRouteBuilder MapKitchenEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/kitchen")
            .WithTags("Kitchen Display System (KDS)");

        group.MapGet("/chits", async (string? station, bool? activeOnly, IKitchenService kitchenService, CancellationToken ct) =>
        {
            var chits = await kitchenService.GetChitsAsync(station, activeOnly ?? true, ct);
            return Results.Ok(chits);
        })
        .WithName("GetKitchenChits")
        .Produces<IReadOnlyList<KitchenChitDto>>(StatusCodes.Status200OK);

        group.MapGet("/chits/{id:guid}", async (Guid id, IKitchenService kitchenService, CancellationToken ct) =>
        {
            var chit = await kitchenService.GetChitByIdAsync(id, ct);
            return chit != null ? Results.Ok(chit) : Results.NotFound();
        })
        .WithName("GetKitchenChitById")
        .Produces<KitchenChitDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/chits/{id:guid}/bump", async (Guid id, BumpChitRequest request, IKitchenService kitchenService, CancellationToken ct) =>
        {
            var updated = await kitchenService.BumpChitAsync(id, request, ct);
            return Results.Ok(updated);
        })
        .WithName("BumpKitchenChit")
        .Produces<KitchenChitDto>(StatusCodes.Status200OK);

        group.MapPost("/chits/{id:guid}/recall", async (Guid id, RecallChitRequest request, IKitchenService kitchenService, CancellationToken ct) =>
        {
            var recalled = await kitchenService.RecallChitAsync(id, request, ct);
            return Results.Ok(recalled);
        })
        .WithName("RecallKitchenChit")
        .Produces<KitchenChitDto>(StatusCodes.Status200OK);

        return routes;
    }
}
