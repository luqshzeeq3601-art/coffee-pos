namespace CoffeePos.Domain.Orders;

public sealed class OrderLineModifier
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OrderLineItemId { get; private set; }
    public Guid ModifierId { get; private set; }
    public string Name { get; private set; }
    public decimal PriceDelta { get; private set; }

    public OrderLineModifier(
        Guid id,
        Guid tenantId,
        Guid orderLineItemId,
        Guid modifierId,
        string name,
        decimal priceDelta)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (orderLineItemId == Guid.Empty) throw new ArgumentException("OrderLineItemId cannot be empty.", nameof(orderLineItemId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Modifier name cannot be empty.", nameof(name));

        Id = id;
        TenantId = tenantId;
        OrderLineItemId = orderLineItemId;
        ModifierId = modifierId;
        Name = name.Trim();
        PriceDelta = priceDelta;
    }
}

public sealed class OrderLineItem
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid? VariantId { get; private set; }
    public string Name { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public string? Notes { get; private set; }

    private readonly List<OrderLineModifier> _modifiers = new();
    public IReadOnlyList<OrderLineModifier> Modifiers => _modifiers.AsReadOnly();

    public decimal ModifierTotalPerUnit => _modifiers.Sum(m => m.PriceDelta);
    public decimal EffectiveUnitPrice => UnitPrice + ModifierTotalPerUnit;
    public decimal LineTotal => EffectiveUnitPrice * Quantity;

    public OrderLineItem(
        Guid id,
        Guid tenantId,
        Guid orderId,
        Guid productId,
        string name,
        decimal unitPrice,
        int quantity,
        Guid? variantId = null,
        string? notes = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId cannot be empty.", nameof(orderId));
        if (productId == Guid.Empty) throw new ArgumentException("ProductId cannot be empty.", nameof(productId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Item name cannot be empty.", nameof(name));
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        Id = id;
        TenantId = tenantId;
        OrderId = orderId;
        ProductId = productId;
        VariantId = variantId;
        Name = name.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
        Notes = notes?.Trim();
    }

    public void AddModifier(OrderLineModifier modifier)
    {
        _modifiers.Add(modifier);
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
        Quantity = newQuantity;
    }
}
