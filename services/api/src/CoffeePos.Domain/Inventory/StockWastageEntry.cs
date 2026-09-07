namespace CoffeePos.Domain.Inventory;

public sealed class StockWastageEntry
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public Guid StockItemId { get; private set; }
    public string StockItemName { get; private set; }
    public decimal QuantityWasted { get; private set; }
    public UnitOfMeasure Unit { get; private set; }
    public WasteReason Reason { get; private set; }
    public decimal CostImpact { get; private set; }
    public string? Notes { get; private set; }
    public Guid LoggedById { get; private set; }
    public string LoggedByName { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public StockWastageEntry(
        Guid id,
        Guid tenantId,
        Guid outletId,
        Guid stockItemId,
        string stockItemName,
        decimal quantityWasted,
        UnitOfMeasure unit,
        WasteReason reason,
        decimal costPerUnit,
        string? notes,
        Guid loggedById,
        string loggedByName)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (stockItemId == Guid.Empty) throw new ArgumentException("Stock item ID cannot be empty.", nameof(stockItemId));
        if (quantityWasted <= 0) throw new ArgumentException("Quantity wasted must be greater than zero.", nameof(quantityWasted));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        StockItemId = stockItemId;
        StockItemName = stockItemName.Trim();
        QuantityWasted = quantityWasted;
        Unit = unit;
        Reason = reason;
        CostImpact = Math.Round(quantityWasted * costPerUnit, 2);
        Notes = notes?.Trim();
        LoggedById = loggedById;
        LoggedByName = loggedByName.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }
}

public sealed class PurchaseOrderItem
{
    public Guid Id { get; private set; }
    public Guid PurchaseOrderId { get; private set; }
    public Guid StockItemId { get; private set; }
    public string StockItemName { get; private set; }
    public UnitOfMeasure Unit { get; private set; }
    public decimal QuantityOrdered { get; private set; }
    public decimal QuantityReceived { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal LineTotal => Math.Round(QuantityOrdered * UnitCost, 2);

    public PurchaseOrderItem(
        Guid id,
        Guid purchaseOrderId,
        Guid stockItemId,
        string stockItemName,
        UnitOfMeasure unit,
        decimal quantityOrdered,
        decimal unitCost)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (stockItemId == Guid.Empty) throw new ArgumentException("Stock item ID cannot be empty.", nameof(stockItemId));
        if (quantityOrdered <= 0) throw new ArgumentException("Quantity ordered must be positive.", nameof(quantityOrdered));

        Id = id;
        PurchaseOrderId = purchaseOrderId;
        StockItemId = stockItemId;
        StockItemName = stockItemName.Trim();
        Unit = unit;
        QuantityOrdered = quantityOrdered;
        QuantityReceived = 0m;
        UnitCost = unitCost;
    }

    public void MarkReceived(decimal qty)
    {
        if (qty < 0) throw new ArgumentException("Received quantity cannot be negative.", nameof(qty));
        QuantityReceived = qty;
    }
}

public sealed class PurchaseOrder
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public string PoNumber { get; private set; }
    public string SupplierName { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? ReceivedAtUtc { get; private set; }

    private readonly List<PurchaseOrderItem> _items = new();
    public IReadOnlyList<PurchaseOrderItem> Items => _items.AsReadOnly();
    public decimal TotalCost => _items.Sum(i => i.LineTotal);

    public PurchaseOrder(
        Guid id,
        Guid tenantId,
        Guid outletId,
        string poNumber,
        string supplierName,
        string? notes = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (string.IsNullOrWhiteSpace(poNumber)) throw new ArgumentException("PO number is required.", nameof(poNumber));
        if (string.IsNullOrWhiteSpace(supplierName)) throw new ArgumentException("Supplier name is required.", nameof(supplierName));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        PoNumber = poNumber.Trim().ToUpperInvariant();
        SupplierName = supplierName.Trim();
        Status = PurchaseOrderStatus.Draft;
        Notes = notes?.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void AddItem(PurchaseOrderItem item)
    {
        _items.Add(item);
    }

    public void MarkReceived()
    {
        Status = PurchaseOrderStatus.Received;
        ReceivedAtUtc = DateTime.UtcNow;
        foreach (var item in _items)
        {
            if (item.QuantityReceived == 0)
            {
                item.MarkReceived(item.QuantityOrdered);
            }
        }
    }
}
