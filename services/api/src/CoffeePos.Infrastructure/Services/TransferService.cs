using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Inventory;

namespace CoffeePos.Infrastructure.Services;

public sealed class TransferService : ITransferService
{
    private readonly ITransferStore _transferStore;
    private readonly IInventoryStore _inventoryStore;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentUser _currentUser;

    public TransferService(
        ITransferStore transferStore,
        IInventoryStore inventoryStore,
        ITenantContext tenantContext,
        ICurrentUser currentUser)
    {
        _transferStore = transferStore;
        _inventoryStore = inventoryStore;
        _tenantContext = tenantContext;
        _currentUser = currentUser;
    }

    private (Guid TenantId, Guid UserId, string UserName) RequireContext()
    {
        var tenantId = _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
        var userId = _currentUser.UserId ?? Guid.Parse("33333333-3333-3333-3333-333333333331");
        var userName = _currentUser.Email ?? "Nurul Manager";
        return (tenantId, userId, userName);
    }

    public async Task<IReadOnlyList<StockTransferDto>> GetTransfersAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var transfers = await _transferStore.GetTransfersAsync(tenantId, cancellationToken);
        var stockItems = await _inventoryStore.GetStockItemsAsync(tenantId, cancellationToken);
        var stockDict = stockItems.ToDictionary(s => s.Id);

        return transfers.Select(t => MapToDto(t, stockDict)).ToList();
    }

    public async Task<StockTransferDto?> GetTransferByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var transfer = await _transferStore.GetTransferByIdAsync(tenantId, id, cancellationToken);
        if (transfer == null) return null;

        var stockItems = await _inventoryStore.GetStockItemsAsync(tenantId, cancellationToken);
        var stockDict = stockItems.ToDictionary(s => s.Id);
        return MapToDto(transfer, stockDict);
    }

    public async Task<StockTransferDto> CreateTransferAsync(CreateStockTransferRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var num = $"TR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        var transfer = new StockTransfer(Guid.NewGuid(), tenantId, num, request.SourceOutletId, request.DestinationOutletId, request.Notes);

        foreach (var it in request.Items)
        {
            transfer.AddItem(it.StockItemId, it.Quantity);
        }

        await _transferStore.SaveTransferAsync(transfer, cancellationToken);
        var stockItems = await _inventoryStore.GetStockItemsAsync(tenantId, cancellationToken);
        return MapToDto(transfer, stockItems.ToDictionary(s => s.Id));
    }

    public async Task<StockTransferDto> DispatchTransferAsync(Guid id, DispatchStockTransferRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, userId, userName) = RequireContext();
        var transfer = await _transferStore.GetTransferByIdAsync(tenantId, id, cancellationToken)
            ?? throw new KeyNotFoundException($"Transfer {id} not found.");

        transfer.Dispatch(userId, userName, request.Notes);

        // Deduct from source outlet
        foreach (var it in transfer.Items)
        {
            var item = await _inventoryStore.GetStockItemByIdAsync(tenantId, it.StockItemId, cancellationToken);
            if (item != null)
            {
                var entry = item.AdjustStock(
                    -it.Quantity,
                    StockMovementType.TransferOut,
                    $"Dispatched transfer {transfer.TransferNumber}",
                    userId,
                    userName);
                await _inventoryStore.SaveStockItemAsync(item, entry, cancellationToken);
            }
        }

        await _transferStore.SaveTransferAsync(transfer, cancellationToken);
        var stockItems = await _inventoryStore.GetStockItemsAsync(tenantId, cancellationToken);
        return MapToDto(transfer, stockItems.ToDictionary(s => s.Id));
    }

    public async Task<StockTransferDto> ReceiveTransferAsync(Guid id, ReceiveStockTransferRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, userId, userName) = RequireContext();
        var transfer = await _transferStore.GetTransferByIdAsync(tenantId, id, cancellationToken)
            ?? throw new KeyNotFoundException($"Transfer {id} not found.");

        transfer.Receive(userId, userName, request.Notes);

        // Replenish destination outlet
        foreach (var it in transfer.Items)
        {
            var item = await _inventoryStore.GetStockItemByIdAsync(tenantId, it.StockItemId, cancellationToken);
            if (item != null)
            {
                var entry = item.AdjustStock(
                    it.Quantity,
                    StockMovementType.TransferIn,
                    $"Received transfer {transfer.TransferNumber}",
                    userId,
                    userName);
                await _inventoryStore.SaveStockItemAsync(item, entry, cancellationToken);
            }
        }

        await _transferStore.SaveTransferAsync(transfer, cancellationToken);
        var stockItems = await _inventoryStore.GetStockItemsAsync(tenantId, cancellationToken);
        return MapToDto(transfer, stockItems.ToDictionary(s => s.Id));
    }

    private static StockTransferDto MapToDto(StockTransfer t, Dictionary<Guid, StockItem> stockDict)
    {
        return new StockTransferDto(
            t.Id,
            t.TenantId,
            t.TransferNumber,
            t.SourceOutletId,
            t.SourceOutletId == Guid.Parse("22222222-2222-2222-2222-222222222221") ? "Bangsar Flagship" : "Damansara Heights",
            t.DestinationOutletId,
            t.DestinationOutletId == Guid.Parse("22222222-2222-2222-2222-222222222222") ? "Damansara Heights" : "Bangsar Flagship",
            t.Status.ToString(),
            t.Items.Select(i => new StockTransferItemDto(
                i.Id,
                i.StockItemId,
                stockDict.TryGetValue(i.StockItemId, out var stk) ? stk.Name : "Unknown Item",
                stockDict.TryGetValue(i.StockItemId, out var stk2) ? stk2.Sku : "SKU-N/A",
                i.Quantity,
                stockDict.TryGetValue(i.StockItemId, out var stk3) ? stk3.Unit.ToString() : "Units")).ToList(),
            t.DispatchedByName,
            t.DispatchedAtUtc,
            t.ReceivedByName,
            t.ReceivedAtUtc,
            t.Notes,
            t.CreatedAtUtc);
    }
}
