namespace CoffeePos.Domain.Catalog;

public sealed class Variant
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ProductId { get; private set; }
    public string Name { get; private set; }
    public string? Sku { get; private set; }
    public string? Barcode { get; private set; }
    public decimal Price { get; private set; }
    public decimal? CostPrice { get; private set; }
    public int SortOrder { get; private set; }

    public Variant(
        Guid id,
        Guid tenantId,
        Guid productId,
        string name,
        decimal price,
        string? sku = null,
        string? barcode = null,
        decimal? costPrice = null,
        int sortOrder = 0)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (productId == Guid.Empty) throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Variant name cannot be empty.", nameof(name));
        if (price < 0) throw new ArgumentException("Price cannot be negative.", nameof(price));

        Id = id;
        TenantId = tenantId;
        ProductId = productId;
        Name = name.Trim();
        Price = price;
        Sku = sku?.Trim();
        Barcode = barcode?.Trim();
        CostPrice = costPrice;
        SortOrder = sortOrder;
    }
}

public sealed class Product
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal BasePrice { get; private set; }
    public bool IsTaxInclusive { get; private set; }
    public Guid? TaxRateId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private readonly List<Variant> _variants = new();
    public IReadOnlyList<Variant> Variants => _variants.AsReadOnly();

    private readonly List<ModifierGroup> _modifierGroups = new();
    public IReadOnlyList<ModifierGroup> ModifierGroups => _modifierGroups.AsReadOnly();

    public Product(
        Guid id,
        Guid tenantId,
        Guid categoryId,
        string name,
        string description,
        decimal basePrice,
        bool isTaxInclusive = true,
        Guid? taxRateId = null,
        bool isActive = true)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (categoryId == Guid.Empty) throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name cannot be empty.", nameof(name));
        if (basePrice < 0) throw new ArgumentException("Base price cannot be negative.", nameof(basePrice));

        Id = id;
        TenantId = tenantId;
        CategoryId = categoryId;
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        BasePrice = basePrice;
        IsTaxInclusive = isTaxInclusive;
        TaxRateId = taxRateId;
        IsActive = isActive;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void AddVariant(Variant variant)
    {
        _variants.Add(variant);
    }

    public void AddModifierGroup(ModifierGroup group)
    {
        _modifierGroups.Add(group);
    }
}
