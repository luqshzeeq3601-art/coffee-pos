using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class ReceiptEndpoints
{
    public static IEndpointRouteBuilder MapReceiptEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/receipts")
            .WithTags("Receipts & Printing");

        group.MapGet("/{transactionId:guid}", async (Guid transactionId, IReceiptService receiptService, CancellationToken ct) =>
        {
            var receipt = await receiptService.GetReceiptAsync(transactionId, ct);
            return receipt != null ? Results.Ok(receipt) : Results.NotFound();
        })
        .WithName("GetDigitalReceipt")
        .Produces<ReceiptDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{transactionId:guid}/escpos", async (Guid transactionId, bool kickDrawer, IReceiptService receiptService, CancellationToken ct) =>
        {
            var payload = await receiptService.GetReceiptEscPosAsync(transactionId, kickDrawer, ct);
            return payload != null ? Results.Ok(payload) : Results.NotFound();
        })
        .WithName("GetEscPosPayload")
        .Produces<ReceiptPrintPayloadDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return routes;
    }
}
