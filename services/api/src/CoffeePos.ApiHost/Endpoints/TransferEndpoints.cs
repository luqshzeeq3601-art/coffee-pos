using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class TransferEndpoints
{
    public static IEndpointRouteBuilder MapTransferEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/transfers")
            .WithTags("Multi-Outlet Stock Transfers");

        group.MapGet("/", async (ITransferService transferService, CancellationToken ct) =>
        {
            var list = await transferService.GetTransfersAsync(ct);
            return Results.Ok(list);
        })
        .WithName("GetStockTransfers")
        .Produces<IReadOnlyList<StockTransferDto>>(StatusCodes.Status200OK);

        group.MapPost("/", async (CreateStockTransferRequest request, ITransferService transferService, CancellationToken ct) =>
        {
            var created = await transferService.CreateTransferAsync(request, ct);
            return Results.Created($"/api/v1/transfers/{created.Id}", created);
        })
        .WithName("CreateStockTransfer")
        .Produces<StockTransferDto>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ITransferService transferService, CancellationToken ct) =>
        {
            var tr = await transferService.GetTransferByIdAsync(id, ct);
            return tr != null ? Results.Ok(tr) : Results.NotFound();
        })
        .WithName("GetStockTransferById")
        .Produces<StockTransferDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/{id:guid}/dispatch", async (Guid id, DispatchStockTransferRequest request, ITransferService transferService, CancellationToken ct) =>
        {
            var dispatched = await transferService.DispatchTransferAsync(id, request, ct);
            return Results.Ok(dispatched);
        })
        .WithName("DispatchStockTransfer")
        .Produces<StockTransferDto>(StatusCodes.Status200OK);

        group.MapPost("/{id:guid}/receive", async (Guid id, ReceiveStockTransferRequest request, ITransferService transferService, CancellationToken ct) =>
        {
            var received = await transferService.ReceiveTransferAsync(id, request, ct);
            return Results.Ok(received);
        })
        .WithName("ReceiveStockTransfer")
        .Produces<StockTransferDto>(StatusCodes.Status200OK);

        return routes;
    }
}
