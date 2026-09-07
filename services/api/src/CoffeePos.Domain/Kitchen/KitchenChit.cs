using CoffeePos.Domain.Orders;

namespace CoffeePos.Domain.Kitchen;

public sealed class KitchenChitItem
{
    public Guid Id { get; private set; }
    public Guid KitchenChitId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string? VariantName { get; private set; }
    public string? ModifiersSummary { get; private set; }
    public string? Notes { get; private set; }
    public int Quantity { get; private set; }
    public bool IsPrepared { get; private set; }

    public KitchenChitItem(
        Guid id,
        Guid kitchenChitId,
        Guid productId,
        string productName,
        string? variantName,
        string? modifiersSummary,
        string? notes,
        int quantity)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (kitchenChitId == Guid.Empty) throw new ArgumentException("Kitchen Chit ID cannot be empty.", nameof(kitchenChitId));
        if (string.IsNullOrWhiteSpace(productName)) throw new ArgumentException("Product name is required.", nameof(productName));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        Id = id;
        KitchenChitId = kitchenChitId;
        ProductId = productId;
        ProductName = productName.Trim();
        VariantName = variantName?.Trim();
        ModifiersSummary = modifiersSummary?.Trim();
        Notes = notes?.Trim();
        Quantity = quantity;
        IsPrepared = false;
    }

    public void TogglePrepared()
    {
        IsPrepared = !IsPrepared;
    }
}

public sealed class KitchenChit
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public Guid OrderId { get; private set; }
    public string OrderNumber { get; private set; }
    public DiningOption DiningOption { get; private set; }
    public string? CustomerName { get; private set; }
    public string? TableNumber { get; private set; }
    public PrepStation Station { get; private set; }
    public ChitStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    private readonly List<KitchenChitItem> _items = new();
    public IReadOnlyList<KitchenChitItem> Items => _items.AsReadOnly();

    public int ElapsedSeconds => (int)Math.Max(0, ((CompletedAtUtc ?? DateTime.UtcNow) - CreatedAtUtc).TotalSeconds);

    public KitchenChit(
        Guid id,
        Guid tenantId,
        Guid outletId,
        Guid orderId,
        string orderNumber,
        DiningOption diningOption,
        PrepStation station,
        string? customerName = null,
        string? tableNumber = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (orderId == Guid.Empty) throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        if (string.IsNullOrWhiteSpace(orderNumber)) throw new ArgumentException("Order number is required.", nameof(orderNumber));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        OrderId = orderId;
        OrderNumber = orderNumber.Trim().ToUpperInvariant();
        DiningOption = diningOption;
        Station = station;
        CustomerName = customerName?.Trim();
        TableNumber = tableNumber?.Trim();
        Status = ChitStatus.Queued;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void AddItem(KitchenChitItem item)
    {
        _items.Add(item);
    }

    public void Bump(ChitStatus? nextStatus = null)
    {
        if (nextStatus.HasValue)
        {
            Status = nextStatus.Value;
            if (Status == ChitStatus.Preparing && StartedAtUtc == null)
            {
                StartedAtUtc = DateTime.UtcNow;
            }
            else if (Status == ChitStatus.Completed)
            {
                CompletedAtUtc = DateTime.UtcNow;
            }
            return;
        }

        switch (Status)
        {
            case ChitStatus.Queued:
                Status = ChitStatus.Preparing;
                StartedAtUtc = DateTime.UtcNow;
                break;
            case ChitStatus.Preparing:
                Status = ChitStatus.Ready;
                break;
            case ChitStatus.Ready:
                Status = ChitStatus.Completed;
                CompletedAtUtc = DateTime.UtcNow;
                break;
            case ChitStatus.Recalled:
                Status = ChitStatus.Preparing;
                break;
        }
    }

    public void Recall()
    {
        Status = ChitStatus.Recalled;
        CompletedAtUtc = null;
    }
}
