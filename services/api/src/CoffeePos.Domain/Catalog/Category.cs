namespace CoffeePos.Domain.Catalog;

public sealed class Category
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public Category(
        Guid id,
        Guid tenantId,
        string name,
        string code,
        int sortOrder = 0,
        bool isActive = true)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Category code cannot be empty.", nameof(code));

        Id = id;
        TenantId = tenantId;
        Name = name.Trim();
        Code = code.Trim().ToUpperInvariant();
        SortOrder = sortOrder;
        IsActive = isActive;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Update(string name, string code, int sortOrder, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Category code cannot be empty.", nameof(code));

        Name = name.Trim();
        Code = code.Trim().ToUpperInvariant();
        SortOrder = sortOrder;
        IsActive = isActive;
    }
}
