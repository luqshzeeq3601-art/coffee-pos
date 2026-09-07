using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Audit;
using CoffeePos.Domain.Inventory;

namespace CoffeePos.Infrastructure.Services;

public sealed class InventoryService : IInventoryService
{
    private readonly IInventoryStore _inventoryStore;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _auditService;

    public InventoryService(
        IInventoryStore inventoryStore,
        ITenantContext tenantContext,
        ICurrentUser currentUser,
        IAuditService auditService)
    {
        _inventoryStore = inventoryStore;
        _tenantContext = tenantContext;
        _currentUser = currentUser;
        _auditService = auditService;
    }

    private (Guid TenantId, Guid OutletId, Guid UserId, string UserName) RequireContext()
    {
        var tenantId = _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
        var outletId = _tenantContext.OutletId ?? Guid.Parse("22222222-2222-2222-2222-222222222221");
        var userId = _currentUser.UserId ?? Guid.Parse("33333333-3333-3333-3333-333333333331");
        var userName = _currentUser.Email ?? "Ahmad Manager";
        return (tenantId, outletId, userId, userName);
    }

    public async Task<IReadOnlyList<StockItemDto>> GetStockItemsAsync(bool lowStockOnly = false, CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId, _, _) = RequireContext();
        var items = await _inventoryStore.GetStockItemsAsync(tenantId, outletId, cancellationToken);
        if (lowStockOnly)
        {
            items = items.Where(i => i.IsLowStock).ToList();
        }
        return items.Select(MapToDto).ToList();
    }

    public async Task<StockItemDto?> GetStockItemByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _, _) = RequireContext();
        var item = await _inventoryStore.GetStockItemByIdAsync(tenantId, id, cancellationToken);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<StockItemDto> AdjustStockAsync(Guid id, AdjustStockRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, userId, userName) = RequireContext();
        var item = await _inventoryStore.GetStockItemByIdAsync(tenantId, id, cancellationToken)
            ?? throw new KeyNotFoundException($"Stock item with ID {id} not found.");

        var delta = request.NewQuantity - item.CurrentStock;
        var entry = item.ApplyMovement(
            StockMovementType.ManualAdjustment,
            delta,
            request.Reason,
            null,
            userId,
            userName);

        await _inventoryStore.SaveStockItemAsync(item, entry, cancellationToken);

        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            userName,
            "User",
            "AdjustStock",
            $"Manually adjusted {item.Name} ({item.Sku}) from {entry.BalanceAfter - delta:F2} to {entry.BalanceAfter:F2} {item.Unit}. Reason: {request.Reason}"));

        return MapToDto(item);
    }

    public async Task<StockItemDto> ReceiveStockAsync(Guid id, ReceiveStockRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, userId, userName) = RequireContext();
        var item = await _inventoryStore.GetStockItemByIdAsync(tenantId, id, cancellationToken)
            ?? throw new KeyNotFoundException($"Stock item with ID {id} not found.");

        var reason = $"Received stock: +{request.QuantityReceived:F2} {item.Unit}" +
            (string.IsNullOrEmpty(request.SupplierInvoiceNumber) ? "" : $" (Inv #{request.SupplierInvoiceNumber})");

        var entry = item.ApplyMovement(
            StockMovementType.ReceiveStock,
            request.QuantityReceived,
            reason,
            request.SupplierInvoiceNumber,
            userId,
            userName);

        await _inventoryStore.SaveStockItemAsync(item, entry, cancellationToken);

        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            userName,
            "User",
            "ReceiveStock",
            $"Received {request.QuantityReceived:F2} {item.Unit} for {item.Name} ({item.Sku}). Balance now: {entry.BalanceAfter:F2}"));

        return MapToDto(item);
    }

    public async Task<IReadOnlyList<StockLedgerEntryDto>> GetStockLedgerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _, _) = RequireContext();
        var entries = await _inventoryStore.GetLedgerEntriesAsync(tenantId, id, cancellationToken);
        return entries.Select(e => new StockLedgerEntryDto(
            e.Id,
            e.TenantId,
            e.StockItemId,
            e.Type.ToString(),
            e.QuantityDelta,
            e.BalanceAfter,
            e.Reason,
            e.ReferenceId,
            e.PerformedByName,
            e.CreatedAtUtc)).ToList();
    }

    public async Task<IReadOnlyList<RecipeDto>> GetRecipesAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _, _) = RequireContext();
        var recipes = await _inventoryStore.GetRecipesAsync(tenantId, cancellationToken);
        return recipes.Select(r => new RecipeDto(
            r.Id,
            r.ProductId,
            r.VariantId,
            r.ProductName,
            r.VariantName,
            r.Items.Select(i => new RecipeItemDto(
                i.Id,
                i.StockItemId,
                i.StockItemName,
                i.Unit.ToString(),
                i.QuantityRequired)).ToList())).ToList();
    }

    public async Task DepleteStockForSaleAsync(DepleteRecipeStockRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, userId, userName) = RequireContext();
        var recipe = await _inventoryStore.GetRecipeForProductAsync(tenantId, request.ProductId, request.VariantId, cancellationToken);
        if (recipe == null) return;

        foreach (var ingredient in recipe.Items)
        {
            var item = await _inventoryStore.GetStockItemByIdAsync(tenantId, ingredient.StockItemId, cancellationToken);
            if (item != null)
            {
                var totalDepletion = ingredient.QuantityRequired * request.QuantitySold;
                var entry = item.ApplyMovement(
                    StockMovementType.SaleDepletion,
                    -totalDepletion,
                    $"Recipe depletion for Order {request.OrderNumber} ({request.QuantitySold}x {recipe.ProductName})",
                    request.OrderNumber,
                    userId,
                    userName);

                await _inventoryStore.SaveStockItemAsync(item, entry, cancellationToken);
            }
        }
    }

    public async Task<StockWastageDto> RecordWastageAsync(RecordWastageRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId, userId, userName) = RequireContext();
        var item = await _inventoryStore.GetStockItemByIdAsync(tenantId, request.StockItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Stock item with ID {request.StockItemId} not found.");

        Enum.TryParse<WasteReason>(request.Reason, true, out var reason);
        if (reason == 0) reason = WasteReason.Spillage;

        var wastage = new StockWastageEntry(
            Guid.NewGuid(),
            tenantId,
            outletId,
            item.Id,
            item.Name,
            request.QuantityWasted,
            item.Unit,
            reason,
            item.CostPerUnit,
            request.Notes,
            userId,
            userName);

        var ledgerEntry = item.ApplyMovement(
            StockMovementType.WasteWritedown,
            -request.QuantityWasted,
            $"Wastage write-down ({reason}): -{request.QuantityWasted:F2} {item.Unit}. Cost Impact: RM {wastage.CostImpact:F2}",
            wastage.Id.ToString(),
            userId,
            userName);

        await _inventoryStore.SaveWastageEntryAsync(wastage, item, ledgerEntry, cancellationToken);

        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            userName,
            "User",
            "RecordWastage",
            $"Logged {reason} wastage of {request.QuantityWasted:F2} {item.Unit} for {item.Name}. Cost impact: RM {wastage.CostImpact:F2}"));

        return MapToWastageDto(wastage);
    }

    public async Task<IReadOnlyList<StockWastageDto>> GetWastageEntriesAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId, _, _) = RequireContext();
        var entries = await _inventoryStore.GetWastageEntriesAsync(tenantId, outletId, cancellationToken);
        return entries.Select(MapToWastageDto).ToList();
    }

    public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId, _, _) = RequireContext();
        var poNum = $"PO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpperInvariant()}";
        var po = new PurchaseOrder(Guid.NewGuid(), tenantId, outletId, poNum, request.SupplierName, request.Notes);

        foreach (var it in request.Items)
        {
            var stockItem = await _inventoryStore.GetStockItemByIdAsync(tenantId, it.StockItemId, cancellationToken)
                ?? throw new KeyNotFoundException($"Stock item with ID {it.StockItemId} not found.");

            po.AddItem(new PurchaseOrderItem(
                Guid.NewGuid(),
                po.Id,
                stockItem.Id,
                stockItem.Name,
                stockItem.Unit,
                it.QuantityOrdered,
                it.UnitCost));
        }

        await _inventoryStore.SavePurchaseOrderAsync(po, cancellationToken);
        return MapToPoDto(po);
    }

    public async Task<PurchaseOrderDto> ReceivePurchaseOrderAsync(Guid poId, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, userId, userName) = RequireContext();
        var po = await _inventoryStore.GetPurchaseOrderByIdAsync(tenantId, poId, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase order with ID {poId} not found.");

        po.MarkReceived();
        await _inventoryStore.SavePurchaseOrderAsync(po, cancellationToken);

        foreach (var item in po.Items)
        {
            var stockItem = await _inventoryStore.GetStockItemByIdAsync(tenantId, item.StockItemId, cancellationToken);
            if (stockItem != null)
            {
                var entry = stockItem.ApplyMovement(
                    StockMovementType.ReceiveStock,
                    item.QuantityReceived,
                    $"Received PO #{po.PoNumber} ({po.SupplierName})",
                    po.PoNumber,
                    userId,
                    userName);

                await _inventoryStore.SaveStockItemAsync(stockItem, entry, cancellationToken);
            }
        }

        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            userName,
            "User",
            "ReceivePurchaseOrder",
            $"Received PO #{po.PoNumber} from {po.SupplierName} with total cost RM {po.TotalCost:F2}"));

        return MapToPoDto(po);
    }

    public async Task<IReadOnlyList<PurchaseOrderDto>> GetPurchaseOrdersAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId, _, _) = RequireContext();
        var pos = await _inventoryStore.GetPurchaseOrdersAsync(tenantId, outletId, cancellationToken);
        return pos.Select(MapToPoDto).ToList();
    }

    private static StockItemDto MapToDto(StockItem s)
    {
        return new StockItemDto(
            s.Id,
            s.TenantId,
            s.OutletId,
            s.Sku,
            s.Name,
            s.Category,
            s.Unit.ToString(),
            s.CurrentStock,
            s.ReorderThreshold,
            s.CostPerUnit,
            s.IsLowStock,
            s.LastUpdatedUtc);
    }

    private static StockWastageDto MapToWastageDto(StockWastageEntry w)
    {
        return new StockWastageDto(
            w.Id,
            w.TenantId,
            w.OutletId,
            w.StockItemId,
            w.StockItemName,
            w.QuantityWasted,
            w.Unit.ToString(),
            w.Reason.ToString(),
            w.CostImpact,
            w.Notes,
            w.LoggedByName,
            w.CreatedAtUtc);
    }

    private static PurchaseOrderDto MapToPoDto(PurchaseOrder p)
    {
        return new PurchaseOrderDto(
            p.Id,
            p.TenantId,
            p.OutletId,
            p.PoNumber,
            p.SupplierName,
            p.Status.ToString(),
            p.TotalCost,
            p.Notes,
            p.CreatedAtUtc,
            p.ReceivedAtUtc,
            p.Items.Select(i => new PurchaseOrderItemDto(
                i.Id,
                i.StockItemId,
                i.StockItemName,
                i.Unit.ToString(),
                i.QuantityOrdered,
                i.QuantityReceived,
                i.UnitCost,
                i.LineTotal)).ToList());
    }
}
