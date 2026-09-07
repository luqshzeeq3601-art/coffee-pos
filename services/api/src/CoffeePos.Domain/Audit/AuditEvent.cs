namespace CoffeePos.Domain.Audit;

public sealed class AuditEvent
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid? OutletId { get; private set; }
    public Guid? DeviceId { get; private set; }
    public string ActorId { get; private set; }
    public string ActorType { get; private set; } // "User" or "Employee"
    public string Action { get; private set; }
    public string? Reason { get; private set; }
    public string? MetadataJson { get; private set; }
    public DateTime TimestampUtc { get; private set; }

    public AuditEvent(
        Guid id,
        Guid tenantId,
        Guid? outletId,
        Guid? deviceId,
        string actorId,
        string actorType,
        string action,
        string? reason = null,
        string? metadataJson = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Audit Event ID cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(actorId)) throw new ArgumentException("Actor ID cannot be empty.", nameof(actorId));
        if (string.IsNullOrWhiteSpace(action)) throw new ArgumentException("Action cannot be empty.", nameof(action));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        DeviceId = deviceId;
        ActorId = actorId.Trim();
        ActorType = actorType.Trim();
        Action = action.Trim();
        Reason = reason?.Trim();
        MetadataJson = metadataJson;
        TimestampUtc = DateTime.UtcNow;
    }
}
