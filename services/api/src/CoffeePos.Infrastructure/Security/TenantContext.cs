using CoffeePos.Application.Common;

namespace CoffeePos.Infrastructure.Security;

public sealed class TenantContext : ITenantContext, ICurrentUser
{
    public Guid? TenantId { get; private set; }
    public Guid? OutletId { get; private set; }
    public Guid? DeviceId { get; private set; }

    public bool IsAuthenticated { get; private set; }
    public string? Id { get; private set; }
    public string? UserType { get; private set; }
    public string? DisplayName { get; private set; }
    public IReadOnlyList<string> Roles { get; private set; } = Array.Empty<string>();
    public IReadOnlyList<string> Permissions { get; private set; } = Array.Empty<string>();

    public void SetContext(Guid tenantId, Guid? outletId = null, Guid? deviceId = null)
    {
        TenantId = tenantId;
        OutletId = outletId;
        DeviceId = deviceId;
    }

    public void SetUser(
        string id,
        string userType,
        string displayName,
        IEnumerable<string> roles,
        IEnumerable<string>? permissions = null)
    {
        IsAuthenticated = true;
        Id = id;
        UserType = userType;
        DisplayName = displayName;
        Roles = roles.ToList().AsReadOnly();
        Permissions = (permissions ?? Array.Empty<string>()).ToList().AsReadOnly();
    }

    public bool HasPermission(string permission)
    {
        return Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
    }
}
