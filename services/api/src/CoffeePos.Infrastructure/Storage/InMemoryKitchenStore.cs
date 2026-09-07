using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Kitchen;
using CoffeePos.Domain.Orders;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryKitchenStore : IKitchenStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoOutletId = Guid.Parse("22222222-2222-2222-2222-222222222221");

    private readonly List<KitchenChit> _chits = new();

    public InMemoryKitchenStore()
    {
        SeedKitchenChits();
    }

    private void SeedKitchenChits()
    {
        // Chit 1: Espresso Bar (Preparing)
        var chit1 = new KitchenChit(
            Guid.Parse("99999999-9999-9999-9999-999999999001"),
            DemoTenantId,
            DemoOutletId,
            Guid.NewGuid(),
            "ORD-101",
            DiningOption.DineIn,
            PrepStation.EspressoBar,
            "Sarah Lee",
            "Table 4");

        chit1.AddItem(new KitchenChitItem(
            Guid.NewGuid(),
            chit1.Id,
            Guid.Parse("44444444-4444-4444-4444-444444444002"),
            "Oat Flat White",
            "Regular",
            "+ Oatly Oat Milk, Extra Hot",
            "Double shot please",
            2));

        chit1.Bump(ChitStatus.Preparing);

        // Chit 2: Filter & Pastry (Queued)
        var chit2 = new KitchenChit(
            Guid.Parse("99999999-9999-9999-9999-999999999002"),
            DemoTenantId,
            DemoOutletId,
            Guid.NewGuid(),
            "ORD-102",
            DiningOption.Takeaway,
            PrepStation.FilterBar,
            "Farid K.",
            null);

        chit2.AddItem(new KitchenChitItem(
            Guid.NewGuid(),
            chit2.Id,
            Guid.Parse("44444444-4444-4444-4444-444444444003"),
            "Ethiopia Guji Pour Over",
            "V60 Drip",
            "Medium grind, Light roast notes",
            "Hot cup",
            1));

        chit2.AddItem(new KitchenChitItem(
            Guid.NewGuid(),
            chit2.Id,
            Guid.Parse("44444444-4444-4444-4444-444444444004"),
            "Almond Croissant",
            null,
            null,
            "Warm up slightly",
            1));

        _chits.AddRange(new[] { chit1, chit2 });
    }

    public Task<IReadOnlyList<KitchenChit>> GetChitsAsync(Guid tenantId, Guid outletId, PrepStation? station = null, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _chits.Where(c => c.TenantId == tenantId && c.OutletId == outletId);

        if (station.HasValue && station.Value != PrepStation.All)
        {
            query = query.Where(c => c.Station == station.Value || c.Station == PrepStation.All);
        }

        if (activeOnly)
        {
            query = query.Where(c => c.Status != ChitStatus.Completed);
        }

        var results = query.OrderBy(c => c.CreatedAtUtc).ToList();
        return Task.FromResult<IReadOnlyList<KitchenChit>>(results);
    }

    public Task<KitchenChit?> GetChitByIdAsync(Guid tenantId, Guid chitId, CancellationToken cancellationToken = default)
    {
        var chit = _chits.FirstOrDefault(c => c.TenantId == tenantId && c.Id == chitId);
        return Task.FromResult(chit);
    }

    public Task SaveChitAsync(KitchenChit chit, CancellationToken cancellationToken = default)
    {
        var idx = _chits.FindIndex(c => c.Id == chit.Id && c.TenantId == chit.TenantId);
        if (idx >= 0)
        {
            _chits[idx] = chit;
        }
        else
        {
            _chits.Add(chit);
        }
        return Task.CompletedTask;
    }
}
