namespace CoffeePos.Application.DTOs;

public record OutboxEntryDto(
    Guid Id,
    string EntityKind,
    string Action,
    string PayloadJson,
    string IdempotencyKey,
    DateTime CreatedAtUtc,
    string SyncStatus,
    int RetryCount,
    string? LastError);

public record WatermarkSyncRequestDto(
    Guid OutletId,
    Guid DeviceId,
    DateTime SinceWatermarkUtc,
    IReadOnlyList<OutboxEntryDto> ClientOutboxBatch);

public record FailedSyncItemDto(
    Guid Id,
    string IdempotencyKey,
    string Error);

public record SyncSummaryDto(
    int ReceivedCount,
    int ProcessedCount,
    int FailedCount);

public record WatermarkSyncResponseDto(
    DateTime ServerWatermarkUtc,
    IReadOnlyList<Guid> ProcessedOutboxIds,
    IReadOnlyList<FailedSyncItemDto> FailedOutboxItems,
    SyncSummaryDto SyncSummary);
