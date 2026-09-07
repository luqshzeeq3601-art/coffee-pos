using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Audit;

namespace CoffeePos.Infrastructure.Services;

public sealed class SyncService : ISyncService
{
    private readonly ITenantContext _tenantContext;
    private readonly IAuditService _auditService;
    private readonly HashSet<string> _seenIdempotencyKeys = new();

    public SyncService(
        ITenantContext tenantContext,
        IAuditService auditService)
    {
        _tenantContext = tenantContext;
        _auditService = auditService;
    }

    private Guid RequireTenantId()
    {
        return _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required for sync.");
    }

    public async Task<WatermarkSyncResponseDto> ProcessWatermarkSyncAsync(WatermarkSyncRequestDto request, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var processedIds = new List<Guid>();
        var failedItems = new List<FailedSyncItemDto>();

        foreach (var item in request.ClientOutboxBatch)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(item.IdempotencyKey))
                {
                    failedItems.Add(new FailedSyncItemDto(item.Id, item.IdempotencyKey, "Idempotency key is required."));
                    continue;
                }

                // Check for duplicate submission
                if (_seenIdempotencyKeys.Contains(item.IdempotencyKey))
                {
                    // Already processed - idempotent ack
                    processedIds.Add(item.Id);
                    continue;
                }

                // In a production backend with persistent DbContext, this routes to respective command handlers
                _seenIdempotencyKeys.Add(item.IdempotencyKey);
                processedIds.Add(item.Id);

                await _auditService.RecordAsync(new AuditEvent(
                    Guid.NewGuid(),
                    tenantId,
                    $"Device:{request.DeviceId}",
                    "SyncEngine",
                    "OutboxBatchItemIngested",
                    $"Successfully ingested offline {item.EntityKind} {item.Action} (IdempotencyKey: {item.IdempotencyKey})"));
            }
            catch (Exception ex)
            {
                failedItems.Add(new FailedSyncItemDto(item.Id, item.IdempotencyKey, ex.Message));
            }
        }

        var serverWatermark = DateTime.UtcNow;
        var summary = new SyncSummaryDto(
            request.ClientOutboxBatch.Count,
            processedIds.Count,
            failedItems.Count);

        return new WatermarkSyncResponseDto(
            serverWatermark,
            processedIds,
            failedItems,
            summary);
    }
}
