using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class CatalogEndpoints
{
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/catalog")
            .WithTags("Catalog & Pricing");

        group.MapGet("/", async (ICatalogService catalogService, CancellationToken ct) =>
        {
            var catalog = await catalogService.GetFullCatalogAsync(ct);
            return Results.Ok(catalog);
        })
        .WithName("GetFullCatalog")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/categories", async (ICatalogService catalogService, CancellationToken ct) =>
        {
            var categories = await catalogService.GetCategoriesAsync(ct);
            return Results.Ok(categories);
        })
        .WithName("GetCatalogCategories")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/products", async (Guid? categoryId, ICatalogService catalogService, CancellationToken ct) =>
        {
            var products = await catalogService.GetProductsAsync(categoryId, ct);
            return Results.Ok(products);
        })
        .WithName("GetCatalogProducts")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/taxes", async (ICatalogService catalogService, CancellationToken ct) =>
        {
            var taxes = await catalogService.GetTaxRatesAsync(ct);
            return Results.Ok(taxes);
        })
        .WithName("GetTaxRates")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/discounts", async (ICatalogService catalogService, CancellationToken ct) =>
        {
            var discounts = await catalogService.GetDiscountsAsync(ct);
            return Results.Ok(discounts);
        })
        .WithName("GetDiscounts")
        .Produces(StatusCodes.Status200OK);

        return routes;
    }
}
