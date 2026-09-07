using System.Collections.Concurrent;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Audit;

namespace CoffeePos.Infrastructure.Audit;

public sealed class InMemoryAuditService : IAuditService
{
    private readonly ConcurrentBag<AuditEvent> _events = new();

    public Task RecordAsync(
        string actorId,
        string actorType,
        string action,
        Guid tenantId,
        Guid? outletId = null,
        Guid? deviceId = null,
        string? reason = null,
        string? metadataJson = null,
        CancellationToken cancellationToken = default)
    {
        var auditEvent = new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            outletId,
            deviceId,
            actorId,
            actorType,
            action,
            reason,
            metadataJson);

        _events.Add(auditEvent);
        return Task.CompletedTask;
    }

    public IReadOnlyList<AuditEvent> GetEvents() => _events.ToArray();
}
