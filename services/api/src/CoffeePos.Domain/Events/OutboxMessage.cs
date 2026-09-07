namespace CoffeePos.Domain.Events;

public enum OutboxStatus
{
    Pending = 1,
    Processing = 2,
    Published = 3,
    Failed = 4
}

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid? OutletId { get; private set; }
    public string EventType { get; private set; }
    public string PayloadJson { get; private set; }
    public OutboxStatus Status { get; private set; }
    public int RetryCount { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? ProcessedAtUtc { get; private set; }
    public string? LastError { get; private set; }

    public OutboxMessage(
        Guid id,
        Guid tenantId,
        Guid? outletId,
        string eventType,
        string payloadJson)
    {
        if (id == Guid.Empty) throw new ArgumentException("Message ID cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(eventType)) throw new ArgumentException("Event type cannot be empty.", nameof(eventType));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        EventType = eventType.Trim();
        PayloadJson = payloadJson;
        Status = OutboxStatus.Pending;
        RetryCount = 0;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkPublished()
    {
        Status = OutboxStatus.Published;
        ProcessedAtUtc = DateTime.UtcNow;
    }

    public void RecordFailure(string error)
    {
        RetryCount++;
        LastError = error;
        Status = RetryCount >= 5 ? OutboxStatus.Failed : OutboxStatus.Pending;
    }
}
