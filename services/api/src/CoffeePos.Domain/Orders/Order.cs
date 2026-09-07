namespace CoffeePos.Domain.Orders;

public sealed class Order
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public string OrderNumber { get; private set; }
    public OrderStatus Status { get; private set; }
    public DiningOption DiningOption { get; private set; }
    public string? TableOrCustomer { get; private set; }
    public Guid? CashierId { get; private set; }
    public string? CashierName { get; private set; }
    public string? Notes { get; private set; }
    public string? CancelReason { get; private set; }

    public decimal DiscountTotal { get; private set; }
    public decimal TaxPercent { get; private set; } // 6.00m
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    private readonly List<OrderLineItem> _items = new();
    public IReadOnlyList<OrderLineItem> Items => _items.AsReadOnly();

    public decimal Subtotal => _items.Sum(i => i.LineTotal);
    public decimal TaxTotal => Math.Round((Subtotal - DiscountTotal) * (TaxPercent / 100m), 2, MidpointRounding.AwayFromZero);
    public decimal GrandTotal => Math.Max(0m, Subtotal - DiscountTotal + TaxTotal);

    public Order(
        Guid id,
        Guid tenantId,
        Guid outletId,
        string orderNumber,
        DiningOption diningOption = DiningOption.DineIn,
        string? tableOrCustomer = null,
        Guid? cashierId = null,
        string? cashierName = null,
        decimal taxPercent = 6.00m,
        string? notes = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (string.IsNullOrWhiteSpace(orderNumber)) throw new ArgumentException("Order number cannot be empty.", nameof(orderNumber));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        OrderNumber = orderNumber.Trim();
        Status = OrderStatus.Draft;
        DiningOption = diningOption;
        TableOrCustomer = tableOrCustomer?.Trim();
        CashierId = cashierId;
        CashierName = cashierName?.Trim();
        TaxPercent = taxPercent;
        DiscountTotal = 0m;
        Notes = notes?.Trim();
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddItem(OrderLineItem item)
    {
        EnsureNotPaid();
        _items.Add(item);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RemoveItem(Guid lineItemId)
    {
        EnsureNotPaid();
        _items.RemoveAll(i => i.Id == lineItemId);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void HoldTicket(string tableOrCustomer, string? notes = null)
    {
        EnsureNotPaid();
        if (string.IsNullOrWhiteSpace(tableOrCustomer))
            throw new ArgumentException("Table or Customer name is required to hold an open ticket.", nameof(tableOrCustomer));

        TableOrCustomer = tableOrCustomer.Trim();
        if (notes != null) Notes = notes.Trim();
        Status = OrderStatus.Open;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ApplyDiscount(decimal discountAmount)
    {
        EnsureNotPaid();
        if (discountAmount < 0) throw new ArgumentException("Discount cannot be negative.", nameof(discountAmount));
        DiscountTotal = discountAmount;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkPaid()
    {
        if (_items.Count == 0) throw new InvalidOperationException("Cannot pay for an empty order.");
        Status = OrderStatus.Paid;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (Status == OrderStatus.Paid) throw new InvalidOperationException("Cannot cancel a completed/paid order directly. Use refund workflow.");
        Status = OrderStatus.Cancelled;
        CancelReason = reason;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void EnsureNotPaid()
    {
        if (Status == OrderStatus.Paid)
            throw new InvalidOperationException("Cannot modify an already paid order.");
    }
}
