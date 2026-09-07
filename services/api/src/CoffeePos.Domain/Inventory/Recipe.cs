namespace CoffeePos.Domain.Inventory;

public sealed class RecipeItem
{
    public Guid Id { get; private set; }
    public Guid RecipeId { get; private set; }
    public Guid StockItemId { get; private set; }
    public string StockItemName { get; private set; }
    public UnitOfMeasure Unit { get; private set; }
    public decimal QuantityRequired { get; private set; }

    public RecipeItem(
        Guid id,
        Guid recipeId,
        Guid stockItemId,
        string stockItemName,
        UnitOfMeasure unit,
        decimal quantityRequired)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (stockItemId == Guid.Empty) throw new ArgumentException("Stock item ID cannot be empty.", nameof(stockItemId));
        if (quantityRequired <= 0) throw new ArgumentException("Quantity required must be positive.", nameof(quantityRequired));

        Id = id;
        RecipeId = recipeId;
        StockItemId = stockItemId;
        StockItemName = stockItemName.Trim();
        Unit = unit;
        QuantityRequired = quantityRequired;
    }
}

public sealed class Recipe
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid? VariantId { get; private set; }
    public string ProductName { get; private set; }
    public string? VariantName { get; private set; }

    private readonly List<RecipeItem> _items = new();
    public IReadOnlyList<RecipeItem> Items => _items.AsReadOnly();

    public Recipe(
        Guid id,
        Guid tenantId,
        Guid productId,
        Guid? variantId,
        string productName,
        string? variantName)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (productId == Guid.Empty) throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

        Id = id;
        TenantId = tenantId;
        ProductId = productId;
        VariantId = variantId;
        ProductName = productName.Trim();
        VariantName = variantName?.Trim();
    }

    public void AddItem(RecipeItem item)
    {
        _items.Add(item);
    }
}
