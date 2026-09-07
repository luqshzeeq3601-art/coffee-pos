using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Inventory;

namespace CoffeePos.Application.Interfaces;

public interface IInventoryStore
{
    Task<IReadOnlyList<StockItem>> GetStockItemsAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default);
    Task<StockItem?> GetStockItemByIdAsync(Guid tenantId, Guid itemId, CancellationToken cancellationToken = default);
    Task SaveStockItemAsync(StockItem item, StockLedgerEntry entry, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockLedgerEntry>> GetLedgerEntriesAsync(Guid tenantId, Guid stockItemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Recipe>> GetRecipesAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<Recipe?> GetRecipeForProductAsync(Guid tenantId, Guid productId, Guid? variantId, CancellationToken cancellationToken = default);
    Task SaveWastageEntryAsync(StockWastageEntry wastage, StockItem item, StockLedgerEntry entry, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockWastageEntry>> GetWastageEntriesAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PurchaseOrder>> GetPurchaseOrdersAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(Guid tenantId, Guid poId, CancellationToken cancellationToken = default);
    Task SavePurchaseOrderAsync(PurchaseOrder po, CancellationToken cancellationToken = default);
}

public interface IInventoryService
{
    Task<IReadOnlyList<StockItemDto>> GetStockItemsAsync(bool lowStockOnly = false, CancellationToken cancellationToken = default);
    Task<StockItemDto?> GetStockItemByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StockItemDto> AdjustStockAsync(Guid id, AdjustStockRequest request, CancellationToken cancellationToken = default);
    Task<StockItemDto> ReceiveStockAsync(Guid id, ReceiveStockRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockLedgerEntryDto>> GetStockLedgerAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RecipeDto>> GetRecipesAsync(CancellationToken cancellationToken = default);
    Task DepleteStockForSaleAsync(DepleteRecipeStockRequest request, CancellationToken cancellationToken = default);
    Task<StockWastageDto> RecordWastageAsync(RecordWastageRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockWastageDto>> GetWastageEntriesAsync(CancellationToken cancellationToken = default);
    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default);
    Task<PurchaseOrderDto> ReceivePurchaseOrderAsync(Guid poId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PurchaseOrderDto>> GetPurchaseOrdersAsync(CancellationToken cancellationToken = default);
}
