using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class LoyaltyEndpoints
{
    public static IEndpointRouteBuilder MapLoyaltyEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/loyalty")
            .WithTags("Customer Loyalty & Points");

        group.MapGet("/customers", async (string? query, ILoyaltyService loyaltyService, CancellationToken ct) =>
        {
            var results = await loyaltyService.SearchCustomersAsync(query ?? "", ct);
            return Results.Ok(results);
        })
        .WithName("SearchCustomers")
        .Produces<IReadOnlyList<CustomerDto>>(StatusCodes.Status200OK);

        group.MapPost("/customers", async (CreateCustomerRequest request, ILoyaltyService loyaltyService, CancellationToken ct) =>
        {
            var customer = await loyaltyService.CreateCustomerAsync(request, ct);
            return Results.Created($"/api/v1/loyalty/customers/{customer.Id}", customer);
        })
        .WithName("CreateCustomer")
        .Produces<CustomerDto>(StatusCodes.Status201Created);

        group.MapGet("/customers/{id:guid}", async (Guid id, ILoyaltyService loyaltyService, CancellationToken ct) =>
        {
            var customer = await loyaltyService.GetCustomerByIdAsync(id, ct);
            return customer != null ? Results.Ok(customer) : Results.NotFound();
        })
        .WithName("GetCustomerById")
        .Produces<CustomerDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/customers/{id:guid}/earn", async (Guid id, EarnPointsRequest request, ILoyaltyService loyaltyService, CancellationToken ct) =>
        {
            var updated = await loyaltyService.EarnPointsAsync(id, request, ct);
            return Results.Ok(updated);
        })
        .WithName("EarnLoyaltyPoints")
        .Produces<CustomerDto>(StatusCodes.Status200OK);

        group.MapPost("/customers/{id:guid}/redeem", async (Guid id, RedeemPointsRequest request, ILoyaltyService loyaltyService, CancellationToken ct) =>
        {
            var updated = await loyaltyService.RedeemPointsAsync(id, request, ct);
            return Results.Ok(updated);
        })
        .WithName("RedeemLoyaltyPoints")
        .Produces<CustomerDto>(StatusCodes.Status200OK);

        group.MapGet("/customers/{id:guid}/ledger", async (Guid id, ILoyaltyService loyaltyService, CancellationToken ct) =>
        {
            var entries = await loyaltyService.GetCustomerLedgerAsync(id, ct);
            return Results.Ok(entries);
        })
        .WithName("GetCustomerPointsLedger")
        .Produces<IReadOnlyList<LoyaltyLedgerEntryDto>>(StatusCodes.Status200OK);

        return routes;
    }
}
