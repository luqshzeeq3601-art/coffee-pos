using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Inventory;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryTransferStore : ITransferStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly List<StockTransfer> _transfers = new();

    public InMemoryTransferStore()
    {
        SeedTransfers();
    }

    private void SeedTransfers()
    {
        var tr1 = new StockTransfer(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
            DemoTenantId,
            "TR-2026-0001",
            Guid.Parse("22222222-2222-2222-2222-222222222221"), // Bangsar
            Guid.Parse("22222222-2222-2222-2222-222222222222"), // Damansara
            "Weekly bean supply replenishment");
        tr1.AddItem(Guid.Parse("88888888-8888-8888-8888-888888888001"), 5000.0000m); // 5kg beans
        tr1.Dispatch(Guid.Empty, "Nurul Manager", "Dispatched via store van");
        tr1.Receive(Guid.Empty, "Kamal Supervisor", "Received in good condition");

        var tr2 = new StockTransfer(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
            DemoTenantId,
            "TR-2026-0002",
            Guid.Parse("22222222-2222-2222-2222-222222222221"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Emergency oat milk restock");
        tr2.AddItem(Guid.Parse("88888888-8888-8888-8888-888888888004"), 6000.0000m); // 6L Oatly
        tr2.Dispatch(Guid.Empty, "Nurul Manager", "In transit");

        _transfers.AddRange(new[] { tr1, tr2 });
    }

    public Task<IReadOnlyList<StockTransfer>> GetTransfersAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var list = _transfers
            .Where(t => t.TenantId == tenantId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult<IReadOnlyList<StockTransfer>>(list);
    }

    public Task<StockTransfer?> GetTransferByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        var tr = _transfers.FirstOrDefault(t => t.TenantId == tenantId && t.Id == id);
        return Task.FromResult(tr);
    }

    public Task SaveTransferAsync(StockTransfer transfer, CancellationToken cancellationToken = default)
    {
        var idx = _transfers.FindIndex(t => t.Id == transfer.Id && t.TenantId == transfer.TenantId);
        if (idx >= 0)
        {
            _transfers[idx] = transfer;
        }
        else
        {
            _transfers.Add(transfer);
        }
        return Task.CompletedTask;
    }
}
