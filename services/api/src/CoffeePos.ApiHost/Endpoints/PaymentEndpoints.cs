using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class PaymentEndpoints
{
    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/payments")
            .WithTags("Payments & Sales Ledgers");

        group.MapPost("/process", async (ProcessPaymentRequest request, IPaymentService paymentService, CancellationToken ct) =>
        {
            var transaction = await paymentService.ProcessPaymentAsync(request, ct);
            return Results.Ok(transaction);
        })
        .WithName("ProcessPayment")
        .Produces<SalesTransactionDto>(StatusCodes.Status200OK);

        group.MapPost("/split", async (ProcessSplitPaymentRequest request, IPaymentService paymentService, CancellationToken ct) =>
        {
            var transaction = await paymentService.ProcessSplitPaymentAsync(request, ct);
            return Results.Ok(transaction);
        })
        .WithName("ProcessSplitPayment")
        .Produces<SalesTransactionDto>(StatusCodes.Status200OK);

        group.MapPost("/refund", async (ProcessRefundRequest request, IPaymentService paymentService, CancellationToken ct) =>
        {
            var refund = await paymentService.ProcessRefundAsync(request, ct);
            return Results.Ok(refund);
        })
        .WithName("ProcessRefund")
        .Produces<RefundDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/transactions/{id:guid}", async (Guid id, IPaymentService paymentService, CancellationToken ct) =>
        {
            var transaction = await paymentService.GetTransactionByIdAsync(id, ct);
            return transaction != null ? Results.Ok(transaction) : Results.NotFound();
        })
        .WithName("GetTransactionById")
        .Produces<SalesTransactionDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return routes;
    }
}
