namespace CoffeePos.Domain.Tenancy;

public sealed class Outlet
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string? Address { get; private set; }
    public bool IsMainOutlet { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public Outlet(Guid id, Guid tenantId, string name, string? address = null, bool isMainOutlet = false)
    {
        if (id == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Outlet name cannot be empty.", nameof(name));

        Id = id;
        TenantId = tenantId;
        Name = name.Trim();
        Address = address?.Trim();
        IsMainOutlet = isMainOutlet;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
