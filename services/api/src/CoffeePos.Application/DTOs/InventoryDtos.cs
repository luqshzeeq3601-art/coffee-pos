namespace CoffeePos.Application.DTOs;

public record StockLedgerEntryDto(
    Guid Id,
    Guid TenantId,
    Guid StockItemId,
    string Type,
    decimal QuantityDelta,
    decimal BalanceAfter,
    string Reason,
    string? ReferenceId,
    string PerformedByName,
    DateTime CreatedAtUtc);

public record StockItemDto(
    Guid Id,
    Guid TenantId,
    Guid OutletId,
    string Sku,
    string Name,
    string Category,
    string Unit,
    decimal CurrentStock,
    decimal ReorderThreshold,
    decimal CostPerUnit,
    bool IsLowStock,
    DateTime LastUpdatedUtc);

public record RecipeItemDto(
    Guid Id,
    Guid StockItemId,
    string StockItemName,
    string Unit,
    decimal QuantityRequired);

public record RecipeDto(
    Guid Id,
    Guid ProductId,
    Guid? VariantId,
    string ProductName,
    string? VariantName,
    IReadOnlyList<RecipeItemDto> Items);

public record AdjustStockRequest(
    decimal NewQuantity,
    string Reason);

public record ReceiveStockRequest(
    decimal QuantityReceived,
    decimal? CostPerUnit,
    string? SupplierInvoiceNumber,
    string? Notes);

public record DepleteRecipeStockRequest(
    Guid ProductId,
    Guid? VariantId,
    decimal QuantitySold,
    string OrderNumber);

public record StockWastageDto(
    Guid Id,
    Guid TenantId,
    Guid OutletId,
    Guid StockItemId,
    string StockItemName,
    decimal QuantityWasted,
    string Unit,
    string Reason,
    decimal CostImpact,
    string? Notes,
    string LoggedByName,
    DateTime CreatedAtUtc);

public record RecordWastageRequest(
    Guid StockItemId,
    decimal QuantityWasted,
    string Reason,
    string? Notes);

public record PurchaseOrderItemDto(
    Guid Id,
    Guid StockItemId,
    string StockItemName,
    string Unit,
    decimal QuantityOrdered,
    decimal QuantityReceived,
    decimal UnitCost,
    decimal LineTotal);

public record PurchaseOrderDto(
    Guid Id,
    Guid TenantId,
    Guid OutletId,
    string PoNumber,
    string SupplierName,
    string Status,
    decimal TotalCost,
    string? Notes,
    DateTime CreatedAtUtc,
    DateTime? ReceivedAtUtc,
    IReadOnlyList<PurchaseOrderItemDto> Items);

public record CreatePurchaseOrderItemRequest(
    Guid StockItemId,
    decimal QuantityOrdered,
    decimal UnitCost);

public record CreatePurchaseOrderRequest(
    string SupplierName,
    string? Notes,
    IReadOnlyList<CreatePurchaseOrderItemRequest> Items);
