namespace CoffeePos.Application.Common;

public interface ITenantContext
{
    Guid? TenantId { get; }
    Guid? OutletId { get; }
    Guid? DeviceId { get; }
    bool HasTenant => TenantId.HasValue && TenantId.Value != Guid.Empty;
    bool HasOutlet => OutletId.HasValue && OutletId.Value != Guid.Empty;
    void SetContext(Guid tenantId, Guid? outletId = null, Guid? deviceId = null);
}

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? Id { get; }
    string? UserType { get; } // "User" or "Employee"
    string? DisplayName { get; }
    IReadOnlyList<string> Roles { get; }
    IReadOnlyList<string> Permissions { get; }
    bool HasPermission(string permission);
}
