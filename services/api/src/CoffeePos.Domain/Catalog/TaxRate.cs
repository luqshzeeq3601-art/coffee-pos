namespace CoffeePos.Domain.Catalog;

public sealed class TaxRate
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public decimal RatePercent { get; private set; } // e.g. 6.00m
    public string Code { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; }

    public TaxRate(
        Guid id,
        Guid tenantId,
        string name,
        decimal ratePercent,
        string code,
        bool isDefault = false,
        bool isActive = true)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tax name cannot be empty.", nameof(name));
        if (ratePercent < 0) throw new ArgumentException("Tax rate cannot be negative.", nameof(ratePercent));

        Id = id;
        TenantId = tenantId;
        Name = name.Trim();
        RatePercent = ratePercent;
        Code = code.Trim().ToUpperInvariant();
        IsDefault = isDefault;
        IsActive = isActive;
    }
}

public enum DiscountType
{
    Percentage = 1,
    FixedAmount = 2
}

public sealed class Discount
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public DiscountType DiscountType { get; private set; }
    public decimal Value { get; private set; }
    public bool RequiresManagerApproval { get; private set; }
    public bool IsActive { get; private set; }

    public Discount(
        Guid id,
        Guid tenantId,
        string name,
        DiscountType discountType,
        decimal value,
        bool requiresManagerApproval = false,
        bool isActive = true)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Discount name cannot be empty.", nameof(name));
        if (value < 0) throw new ArgumentException("Discount value cannot be negative.", nameof(value));

        Id = id;
        TenantId = tenantId;
        Name = name.Trim();
        DiscountType = discountType;
        Value = value;
        RequiresManagerApproval = requiresManagerApproval;
        IsActive = isActive;
    }
}
