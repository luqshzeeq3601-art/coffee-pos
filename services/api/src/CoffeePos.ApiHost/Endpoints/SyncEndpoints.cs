using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class SyncEndpoints
{
    public static IEndpointRouteBuilder MapSyncEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/sync")
            .WithTags("Offline Watermark & Sync Engine");

        group.MapPost("/watermark", async (WatermarkSyncRequestDto request, ISyncService syncService, CancellationToken ct) =>
        {
            var response = await syncService.ProcessWatermarkSyncAsync(request, ct);
            return Results.Ok(response);
        })
        .WithName("ProcessWatermarkSync")
        .Produces<WatermarkSyncResponseDto>(StatusCodes.Status200OK);

        return routes;
    }
}
