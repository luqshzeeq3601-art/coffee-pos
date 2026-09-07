namespace CoffeePos.Domain.Identity;

public sealed class Employee
{
    private readonly List<Role> _roles = new();
    private readonly List<Guid> _assignedOutletIds = new();

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string DisplayName { get; private set; }
    public string PinHash { get; private set; }
    public string PinSalt { get; private set; }
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
    public IReadOnlyList<Guid> AssignedOutletIds => _assignedOutletIds.AsReadOnly();
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public Employee(
        Guid id,
        Guid tenantId,
        string displayName,
        string pinHash,
        string pinSalt,
        IEnumerable<Role> roles,
        IEnumerable<Guid> assignedOutletIds)
    {
        if (id == Guid.Empty) throw new ArgumentException("Employee ID cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(displayName)) throw new ArgumentException("Display name cannot be empty.", nameof(displayName));

        Id = id;
        TenantId = tenantId;
        DisplayName = displayName.Trim();
        PinHash = pinHash;
        PinSalt = pinSalt;
        _roles.AddRange(roles);
        _assignedOutletIds.AddRange(assignedOutletIds);
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public bool HasPermission(string permission)
    {
        foreach (var role in _roles)
        {
            if (Permissions.GetPermissionsForRole(role).Contains(permission))
            {
                return true;
            }
        }
        return false;
    }

    public bool CanOperateAtOutlet(Guid outletId)
    {
        return _assignedOutletIds.Count == 0 || _assignedOutletIds.Contains(outletId);
    }
}
