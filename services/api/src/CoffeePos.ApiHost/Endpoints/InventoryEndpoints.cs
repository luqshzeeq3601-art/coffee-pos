using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class InventoryEndpoints
{
    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/inventory")
            .WithTags("Inventory & Stock Control");

        group.MapGet("/items", async (bool? lowStockOnly, IInventoryService inventoryService, CancellationToken ct) =>
        {
            var items = await inventoryService.GetStockItemsAsync(lowStockOnly ?? false, ct);
            return Results.Ok(items);
        })
        .WithName("GetStockItems")
        .Produces<IReadOnlyList<StockItemDto>>(StatusCodes.Status200OK);

        group.MapGet("/items/{id:guid}", async (Guid id, IInventoryService inventoryService, CancellationToken ct) =>
        {
            var item = await inventoryService.GetStockItemByIdAsync(id, ct);
            return item != null ? Results.Ok(item) : Results.NotFound();
        })
        .WithName("GetStockItemById")
        .Produces<StockItemDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/items/{id:guid}/adjust", async (Guid id, AdjustStockRequest request, IInventoryService inventoryService, CancellationToken ct) =>
        {
            var updated = await inventoryService.AdjustStockAsync(id, request, ct);
            return Results.Ok(updated);
        })
        .WithName("AdjustStock")
        .Produces<StockItemDto>(StatusCodes.Status200OK);

        group.MapPost("/items/{id:guid}/receive", async (Guid id, ReceiveStockRequest request, IInventoryService inventoryService, CancellationToken ct) =>
        {
            var updated = await inventoryService.ReceiveStockAsync(id, request, ct);
            return Results.Ok(updated);
        })
        .WithName("ReceiveStock")
        .Produces<StockItemDto>(StatusCodes.Status200OK);

        group.MapGet("/items/{id:guid}/ledger", async (Guid id, IInventoryService inventoryService, CancellationToken ct) =>
        {
            var entries = await inventoryService.GetStockLedgerAsync(id, ct);
            return Results.Ok(entries);
        })
        .WithName("GetStockLedger")
        .Produces<IReadOnlyList<StockLedgerEntryDto>>(StatusCodes.Status200OK);

        group.MapGet("/recipes", async (IInventoryService inventoryService, CancellationToken ct) =>
        {
            var recipes = await inventoryService.GetRecipesAsync(ct);
            return Results.Ok(recipes);
        })
        .WithName("GetRecipes")
        .Produces<IReadOnlyList<RecipeDto>>(StatusCodes.Status200OK);

        group.MapPost("/deplete", async (DepleteRecipeStockRequest request, IInventoryService inventoryService, CancellationToken ct) =>
        {
            await inventoryService.DepleteStockForSaleAsync(request, ct);
            return Results.Accepted();
        })
        .WithName("DepleteRecipeStock")
        .Produces(StatusCodes.Status202Accepted);

        group.MapPost("/wastage", async (RecordWastageRequest request, IInventoryService inventoryService, CancellationToken ct) =>
        {
            var wastage = await inventoryService.RecordWastageAsync(request, ct);
            return Results.Created($"/api/v1/inventory/wastage/{wastage.Id}", wastage);
        })
        .WithName("RecordWastage")
        .Produces<StockWastageDto>(StatusCodes.Status201Created);

        group.MapGet("/wastage", async (IInventoryService inventoryService, CancellationToken ct) =>
        {
            var entries = await inventoryService.GetWastageEntriesAsync(ct);
            return Results.Ok(entries);
        })
        .WithName("GetWastageEntries")
        .Produces<IReadOnlyList<StockWastageDto>>(StatusCodes.Status200OK);

        group.MapPost("/purchase-orders", async (CreatePurchaseOrderRequest request, IInventoryService inventoryService, CancellationToken ct) =>
        {
            var po = await inventoryService.CreatePurchaseOrderAsync(request, ct);
            return Results.Created($"/api/v1/inventory/purchase-orders/{po.Id}", po);
        })
        .WithName("CreatePurchaseOrder")
        .Produces<PurchaseOrderDto>(StatusCodes.Status201Created);

        group.MapGet("/purchase-orders", async (IInventoryService inventoryService, CancellationToken ct) =>
        {
            var pos = await inventoryService.GetPurchaseOrdersAsync(ct);
            return Results.Ok(pos);
        })
        .WithName("GetPurchaseOrders")
        .Produces<IReadOnlyList<PurchaseOrderDto>>(StatusCodes.Status200OK);

        group.MapPost("/purchase-orders/{id:guid}/receive", async (Guid id, IInventoryService inventoryService, CancellationToken ct) =>
        {
            var po = await inventoryService.ReceivePurchaseOrderAsync(id, ct);
            return Results.Ok(po);
        })
        .WithName("ReceivePurchaseOrder")
        .Produces<PurchaseOrderDto>(StatusCodes.Status200OK);

        return routes;
    }
}
