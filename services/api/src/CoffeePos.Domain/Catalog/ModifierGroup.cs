namespace CoffeePos.Domain.Catalog;

public sealed class Modifier
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ModifierGroupId { get; private set; }
    public string Name { get; private set; }
    public decimal PriceDelta { get; private set; }
    public bool IsDefault { get; private set; }
    public int SortOrder { get; private set; }

    public Modifier(
        Guid id,
        Guid tenantId,
        Guid modifierGroupId,
        string name,
        decimal priceDelta = 0m,
        bool isDefault = false,
        int sortOrder = 0)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (modifierGroupId == Guid.Empty) throw new ArgumentException("Modifier Group ID cannot be empty.", nameof(modifierGroupId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Modifier name cannot be empty.", nameof(name));

        Id = id;
        TenantId = tenantId;
        ModifierGroupId = modifierGroupId;
        Name = name.Trim();
        PriceDelta = priceDelta;
        IsDefault = isDefault;
        SortOrder = sortOrder;
    }
}

public sealed class ModifierGroup
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public int MinSelections { get; private set; }
    public int MaxSelections { get; private set; }
    public bool IsRequired => MinSelections > 0;
    private readonly List<Modifier> _modifiers = new();
    public IReadOnlyList<Modifier> Modifiers => _modifiers.AsReadOnly();

    public ModifierGroup(
        Guid id,
        Guid tenantId,
        string name,
        int minSelections = 0,
        int maxSelections = 1)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Group name cannot be empty.", nameof(name));

        Id = id;
        TenantId = tenantId;
        Name = name.Trim();
        MinSelections = minSelections;
        MaxSelections = maxSelections;
    }

    public void AddModifier(Modifier modifier)
    {
        _modifiers.Add(modifier);
    }
}
