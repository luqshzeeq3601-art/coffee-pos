using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Catalog;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryCatalogStore : ICatalogStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly List<Category> _categories = new();
    private readonly List<Product> _products = new();
    private readonly List<TaxRate> _taxRates = new();
    private readonly List<Discount> _discounts = new();

    public InMemoryCatalogStore()
    {
        SeedDemoCatalog();
    }

    private void SeedDemoCatalog()
    {
        // Tax Rates: 6% Malaysian Sales & Service Tax (SST)
        var sstRate = new TaxRate(
            Guid.Parse("66666666-6666-6666-6666-666666666661"),
            DemoTenantId,
            "SST 6%",
            6.00m,
            "SST-6",
            isDefault: true,
            isActive: true);
        _taxRates.Add(sstRate);

        // Discounts
        var staffDiscount = new Discount(
            Guid.Parse("77777777-7777-7777-7777-777777777771"),
            DemoTenantId,
            "Staff Meal (50%)",
            DiscountType.Percentage,
            50.0m,
            requiresManagerApproval: false,
            isActive: true);
        var managerPromo = new Discount(
            Guid.Parse("77777777-7777-7777-7777-777777777772"),
            DemoTenantId,
            "VIP Hospitality (100%)",
            DiscountType.Percentage,
            100.0m,
            requiresManagerApproval: true,
            isActive: true);
        _discounts.AddRange(new[] { staffDiscount, managerPromo });

        // Categories
        var catEspresso = new Category(Guid.Parse("33333333-3333-3333-3333-333333333301"), DemoTenantId, "Espresso Bar", "ESP", 1);
        var catFilter = new Category(Guid.Parse("33333333-3333-3333-3333-333333333302"), DemoTenantId, "Filter & Pour Over", "FLT", 2);
        var catTea = new Category(Guid.Parse("33333333-3333-3333-3333-333333333303"), DemoTenantId, "Tea & Non-Coffee", "TEA", 3);
        var catFood = new Category(Guid.Parse("33333333-3333-3333-3333-333333333304"), DemoTenantId, "Pastries & Food", "FOD", 4);
        _categories.AddRange(new[] { catEspresso, catFilter, catTea, catFood });

        // Product 1: Oat Flat White
        var prodFlatWhite = new Product(
            Guid.Parse("44444444-4444-4444-4444-444444444401"),
            DemoTenantId,
            catEspresso.Id,
            "Oat Flat White",
            "Velvety microfoam with Oatly Barista",
            14.50m,
            isTaxInclusive: true,
            taxRateId: sstRate.Id);

        var modGroupMilk = new ModifierGroup(Guid.NewGuid(), DemoTenantId, "Milk Choice", 1, 1);
        modGroupMilk.AddModifier(new Modifier(Guid.NewGuid(), DemoTenantId, modGroupMilk.Id, "Oat Milk (Default)", 0m, true, 1));
        modGroupMilk.AddModifier(new Modifier(Guid.NewGuid(), DemoTenantId, modGroupMilk.Id, "Soy Milk", 0m, false, 2));
        modGroupMilk.AddModifier(new Modifier(Guid.NewGuid(), DemoTenantId, modGroupMilk.Id, "Almond Milk", 1.50m, false, 3));
        prodFlatWhite.AddModifierGroup(modGroupMilk);

        var modGroupExtra = new ModifierGroup(Guid.NewGuid(), DemoTenantId, "Extras", 0, 3);
        modGroupExtra.AddModifier(new Modifier(Guid.NewGuid(), DemoTenantId, modGroupExtra.Id, "Extra Espresso Shot", 3.00m, false, 1));
        modGroupExtra.AddModifier(new Modifier(Guid.NewGuid(), DemoTenantId, modGroupExtra.Id, "Vanilla Syrup", 2.00m, false, 2));
        prodFlatWhite.AddModifierGroup(modGroupExtra);

        // Product 2: Pour Over Ethiopia Guji
        var prodPourOver = new Product(
            Guid.Parse("44444444-4444-4444-4444-444444444402"),
            DemoTenantId,
            catFilter.Id,
            "Pour Over (Ethiopia Guji)",
            "Jasmine, peach, bergamot notes. V60 extraction",
            18.00m,
            isTaxInclusive: true,
            taxRateId: sstRate.Id);

        // Product 3: Almond Croissant
        var prodCroissant = new Product(
            Guid.Parse("44444444-4444-4444-4444-444444444403"),
            DemoTenantId,
            catFood.Id,
            "Almond Croissant",
            "Twice-baked French butter croissant with frangipane",
            12.00m,
            isTaxInclusive: true,
            taxRateId: sstRate.Id);

        _products.AddRange(new[] { prodFlatWhite, prodPourOver, prodCroissant });
    }

    public Task<IReadOnlyList<Category>> GetCategoriesAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var result = _categories.Where(c => c.TenantId == tenantId && c.IsActive).OrderBy(c => c.SortOrder).ToList();
        return Task.FromResult<IReadOnlyList<Category>>(result);
    }

    public Task<IReadOnlyList<Product>> GetProductsAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var result = _products.Where(p => p.TenantId == tenantId && p.IsActive).ToList();
        return Task.FromResult<IReadOnlyList<Product>>(result);
    }

    public Task<Product?> GetProductByIdAsync(Guid tenantId, Guid productId, CancellationToken cancellationToken = default)
    {
        var prod = _products.FirstOrDefault(p => p.TenantId == tenantId && p.Id == productId);
        return Task.FromResult(prod);
    }

    public Task<IReadOnlyList<TaxRate>> GetTaxRatesAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var result = _taxRates.Where(t => t.TenantId == tenantId && t.IsActive).ToList();
        return Task.FromResult<IReadOnlyList<TaxRate>>(result);
    }

    public Task<IReadOnlyList<Discount>> GetDiscountsAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var result = _discounts.Where(d => d.TenantId == tenantId && d.IsActive).ToList();
        return Task.FromResult<IReadOnlyList<Discount>>(result);
    }

    public Task SaveProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        var idx = _products.FindIndex(p => p.Id == product.Id && p.TenantId == product.TenantId);
        if (idx >= 0)
        {
            _products[idx] = product;
        }
        else
        {
            _products.Add(product);
        }
        return Task.CompletedTask;
    }
}
