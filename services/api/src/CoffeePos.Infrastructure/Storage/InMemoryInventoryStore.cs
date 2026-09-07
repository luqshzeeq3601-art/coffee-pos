using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Inventory;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryInventoryStore : IInventoryStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoOutletId = Guid.Parse("22222222-2222-2222-2222-222222222221");

    private readonly List<StockItem> _stockItems = new();
    private readonly List<StockLedgerEntry> _ledgerEntries = new();
    private readonly List<Recipe> _recipes = new();
    private readonly List<StockWastageEntry> _wastageEntries = new();
    private readonly List<PurchaseOrder> _purchaseOrders = new();

    public InMemoryInventoryStore()
    {
        SeedInventoryAndRecipes();
    }

    private void SeedInventoryAndRecipes()
    {
        // 1. Stock Items
        var houseBeans = new StockItem(
            Guid.Parse("77777777-7777-7777-7777-777777777001"),
            DemoTenantId,
            DemoOutletId,
            "BEAN-HOUSE-01",
            "Artisan Espresso Blend Beans",
            "Coffee Beans",
            UnitOfMeasure.Grams,
            12500m, // 12.5 kg
            2000m,  // 2 kg threshold
            0.085m);

        var ethiopiaBeans = new StockItem(
            Guid.Parse("77777777-7777-7777-7777-777777777002"),
            DemoTenantId,
            DemoOutletId,
            "BEAN-ETH-01",
            "Ethiopia Guji Single Origin",
            "Coffee Beans",
            UnitOfMeasure.Grams,
            4500m, // 4.5 kg
            1000m,
            0.120m);

        var oatMilk = new StockItem(
            Guid.Parse("77777777-7777-7777-7777-777777777003"),
            DemoTenantId,
            DemoOutletId,
            "MILK-OAT-01",
            "Oatly Barista Edition (1L)",
            "Dairy & Alternatives",
            UnitOfMeasure.Milliliters,
            24000m, // 24 Liters
            5000m,
            0.015m);

        var freshMilk = new StockItem(
            Guid.Parse("77777777-7777-7777-7777-777777777004"),
            DemoTenantId,
            DemoOutletId,
            "MILK-FRESH-01",
            "Farm Fresh Whole Milk (1L)",
            "Dairy & Alternatives",
            UnitOfMeasure.Milliliters,
            30000m, // 30 Liters
            6000m,
            0.008m);

        var croissant = new StockItem(
            Guid.Parse("77777777-7777-7777-7777-777777777005"),
            DemoTenantId,
            DemoOutletId,
            "PSTR-CROIS-01",
            "Almond Croissant",
            "Bakery",
            UnitOfMeasure.Pieces,
            18m,
            5m,
            6.50m);

        _stockItems.AddRange(new[] { houseBeans, ethiopiaBeans, oatMilk, freshMilk, croissant });

        // 2. Recipes
        var flatWhiteRecipe = new Recipe(
            Guid.NewGuid(),
            DemoTenantId,
            Guid.Parse("44444444-4444-4444-4444-444444444002"), // Oat Flat White product
            null,
            "Oat Flat White",
            null);
        flatWhiteRecipe.AddItem(new RecipeItem(Guid.NewGuid(), flatWhiteRecipe.Id, houseBeans.Id, houseBeans.Name, UnitOfMeasure.Grams, 18.0m));
        flatWhiteRecipe.AddItem(new RecipeItem(Guid.NewGuid(), flatWhiteRecipe.Id, oatMilk.Id, oatMilk.Name, UnitOfMeasure.Milliliters, 220.0m));

        var pourOverRecipe = new Recipe(
            Guid.NewGuid(),
            DemoTenantId,
            Guid.Parse("44444444-4444-4444-4444-444444444003"), // Ethiopia Guji product
            null,
            "Pour Over (Ethiopia Guji)",
            null);
        pourOverRecipe.AddItem(new RecipeItem(Guid.NewGuid(), pourOverRecipe.Id, ethiopiaBeans.Id, ethiopiaBeans.Name, UnitOfMeasure.Grams, 16.0m));

        _recipes.AddRange(new[] { flatWhiteRecipe, pourOverRecipe });

        // 3. Seed Wastage & PO
        var wastage1 = new StockWastageEntry(
            Guid.NewGuid(),
            DemoTenantId,
            DemoOutletId,
            houseBeans.Id,
            houseBeans.Name,
            150m, // 150g dialed in
            UnitOfMeasure.Grams,
            WasteReason.CalibrationDialIn,
            houseBeans.CostPerUnit,
            "Morning grinder dial-in & extraction calibration",
            Guid.Parse("33333333-3333-3333-3333-333333333331"),
            "Ahmad Barista");
        _wastageEntries.Add(wastage1);

        var po1 = new PurchaseOrder(
            Guid.Parse("88888888-8888-8888-8888-888888888001"),
            DemoTenantId,
            DemoOutletId,
            "PO-20260819-01",
            "Oatly Malaysia Sdn Bhd",
            "Weekly barista edition replenishment");
        po1.AddItem(new PurchaseOrderItem(Guid.NewGuid(), po1.Id, oatMilk.Id, oatMilk.Name, UnitOfMeasure.Milliliters, 60000m, 0.015m));
        _purchaseOrders.Add(po1);
    }

    public Task<IReadOnlyList<StockItem>> GetStockItemsAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default)
    {
        var items = _stockItems.Where(s => s.TenantId == tenantId && s.OutletId == outletId).ToList();
        return Task.FromResult<IReadOnlyList<StockItem>>(items);
    }

    public Task<StockItem?> GetStockItemByIdAsync(Guid tenantId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var item = _stockItems.FirstOrDefault(s => s.TenantId == tenantId && s.Id == itemId);
        return Task.FromResult(item);
    }

    public Task SaveStockItemAsync(StockItem item, StockLedgerEntry entry, CancellationToken cancellationToken = default)
    {
        var idx = _stockItems.FindIndex(s => s.Id == item.Id && s.TenantId == item.TenantId);
        if (idx >= 0)
        {
            _stockItems[idx] = item;
        }
        else
        {
            _stockItems.Add(item);
        }

        _ledgerEntries.Add(entry);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<StockLedgerEntry>> GetLedgerEntriesAsync(Guid tenantId, Guid stockItemId, CancellationToken cancellationToken = default)
    {
        var list = _ledgerEntries
            .Where(e => e.TenantId == tenantId && e.StockItemId == stockItemId)
            .OrderByDescending(e => e.CreatedAtUtc)
            .ToList();
        return Task.FromResult<IReadOnlyList<StockLedgerEntry>>(list);
    }

    public Task<IReadOnlyList<Recipe>> GetRecipesAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var recipes = _recipes.Where(r => r.TenantId == tenantId).ToList();
        return Task.FromResult<IReadOnlyList<Recipe>>(recipes);
    }

    public Task<Recipe?> GetRecipeForProductAsync(Guid tenantId, Guid productId, Guid? variantId, CancellationToken cancellationToken = default)
    {
        var recipe = _recipes.FirstOrDefault(r => r.TenantId == tenantId && r.ProductId == productId && (!variantId.HasValue || r.VariantId == variantId));
        return Task.FromResult(recipe);
    }

    public Task SaveWastageEntryAsync(StockWastageEntry wastage, StockItem item, StockLedgerEntry entry, CancellationToken cancellationToken = default)
    {
        _wastageEntries.Add(wastage);
        return SaveStockItemAsync(item, entry, cancellationToken);
    }

    public Task<IReadOnlyList<StockWastageEntry>> GetWastageEntriesAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default)
    {
        var entries = _wastageEntries.Where(w => w.TenantId == tenantId && w.OutletId == outletId).OrderByDescending(w => w.CreatedAtUtc).ToList();
        return Task.FromResult<IReadOnlyList<StockWastageEntry>>(entries);
    }

    public Task<IReadOnlyList<PurchaseOrder>> GetPurchaseOrdersAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default)
    {
        var pos = _purchaseOrders.Where(p => p.TenantId == tenantId && p.OutletId == outletId).OrderByDescending(p => p.CreatedAtUtc).ToList();
        return Task.FromResult<IReadOnlyList<PurchaseOrder>>(pos);
    }

    public Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(Guid tenantId, Guid poId, CancellationToken cancellationToken = default)
    {
        var po = _purchaseOrders.FirstOrDefault(p => p.TenantId == tenantId && p.Id == poId);
        return Task.FromResult(po);
    }

    public Task SavePurchaseOrderAsync(PurchaseOrder po, CancellationToken cancellationToken = default)
    {
        var idx = _purchaseOrders.FindIndex(p => p.Id == po.Id && p.TenantId == po.TenantId);
        if (idx >= 0)
        {
            _purchaseOrders[idx] = po;
        }
        else
        {
            _purchaseOrders.Add(po);
        }
        return Task.CompletedTask;
    }
}
