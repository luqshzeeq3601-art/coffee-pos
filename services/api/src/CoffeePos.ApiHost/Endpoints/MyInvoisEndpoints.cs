using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class MyInvoisEndpoints
{
    public static IEndpointRouteBuilder MapMyInvoisEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/myinvois")
            .WithTags("Malaysian LHDN MyInvois e-Invoicing");

        group.MapGet("/recent", async (int? limit, IMyInvoisService myInvoisService, CancellationToken ct) =>
        {
            var docs = await myInvoisService.GetRecentDocumentsAsync(limit ?? 20, ct);
            return Results.Ok(docs);
        })
        .WithName("GetRecentMyInvoisDocuments")
        .Produces<IReadOnlyList<MyInvoisDocumentDto>>(StatusCodes.Status200OK);

        group.MapPost("/submit", async (SubmitInvoiceRequest request, IMyInvoisService myInvoisService, CancellationToken ct) =>
        {
            var doc = await myInvoisService.SubmitInvoiceAsync(request, ct);
            return Results.Created($"/api/v1/myinvois/{doc.Id}", doc);
        })
        .WithName("SubmitMyInvoisDocument")
        .Produces<MyInvoisDocumentDto>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, IMyInvoisService myInvoisService, CancellationToken ct) =>
        {
            var doc = await myInvoisService.GetDocumentByIdAsync(id, ct);
            return doc != null ? Results.Ok(doc) : Results.NotFound();
        })
        .WithName("GetMyInvoisDocumentById")
        .Produces<MyInvoisDocumentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/{id:guid}/cancel", async (Guid id, CancelInvoiceRequest request, IMyInvoisService myInvoisService, CancellationToken ct) =>
        {
            var cancelled = await myInvoisService.CancelInvoiceAsync(id, request, ct);
            return Results.Ok(cancelled);
        })
        .WithName("CancelMyInvoisDocument")
        .Produces<MyInvoisDocumentDto>(StatusCodes.Status200OK);

        return routes;
    }
}
