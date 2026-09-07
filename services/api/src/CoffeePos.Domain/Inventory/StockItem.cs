namespace CoffeePos.Domain.Inventory;

public sealed class StockItem
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public string Category { get; private set; }
    public UnitOfMeasure Unit { get; private set; }
    public decimal CurrentStock { get; private set; }
    public decimal ReorderThreshold { get; private set; }
    public decimal CostPerUnit { get; private set; }
    public DateTime LastUpdatedUtc { get; private set; }

    public bool IsLowStock => CurrentStock <= ReorderThreshold;

    public StockItem(
        Guid id,
        Guid tenantId,
        Guid outletId,
        string sku,
        string name,
        string category,
        UnitOfMeasure unit,
        decimal initialStock,
        decimal reorderThreshold,
        decimal costPerUnit)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("SKU is required.", nameof(sku));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (initialStock < 0) throw new ArgumentException("Initial stock cannot be negative.", nameof(initialStock));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        Sku = sku.Trim().ToUpperInvariant();
        Name = name.Trim();
        Category = category.Trim();
        Unit = unit;
        CurrentStock = initialStock;
        ReorderThreshold = reorderThreshold;
        CostPerUnit = costPerUnit;
        LastUpdatedUtc = DateTime.UtcNow;
    }

    public StockLedgerEntry ApplyMovement(
        StockMovementType type,
        decimal quantityDelta,
        string reason,
        string? referenceId,
        Guid performedById,
        string performedByName)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("A valid reason is required for every inventory movement.", nameof(reason));

        var newBalance = CurrentStock + quantityDelta;
        if (newBalance < 0)
        {
            // Floor at zero to prevent negative stock in fast coffee bar operations
            newBalance = 0;
        }

        CurrentStock = newBalance;
        LastUpdatedUtc = DateTime.UtcNow;

        return new StockLedgerEntry(
            Guid.NewGuid(),
            TenantId,
            Id,
            type,
            quantityDelta,
            newBalance,
            reason.Trim(),
            referenceId?.Trim(),
            performedById,
            performedByName.Trim());
    }
}

public sealed class StockLedgerEntry
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid StockItemId { get; private set; }
    public StockMovementType Type { get; private set; }
    public decimal QuantityDelta { get; private set; }
    public decimal BalanceAfter { get; private set; }
    public string Reason { get; private set; }
    public string? ReferenceId { get; private set; }
    public Guid PerformedById { get; private set; }
    public string PerformedByName { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public StockLedgerEntry(
        Guid id,
        Guid tenantId,
        Guid stockItemId,
        StockMovementType type,
        decimal quantityDelta,
        decimal balanceAfter,
        string reason,
        string? referenceId,
        Guid performedById,
        string performedByName)
    {
        Id = id;
        TenantId = tenantId;
        StockItemId = stockItemId;
        Type = type;
        QuantityDelta = quantityDelta;
        BalanceAfter = balanceAfter;
        Reason = reason;
        ReferenceId = referenceId;
        PerformedById = performedById;
        PerformedByName = performedByName;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
