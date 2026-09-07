namespace CoffeePos.Application.DTOs;

public record KitchenChitItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string? VariantName,
    string? ModifiersSummary,
    string? Notes,
    int Quantity,
    bool IsPrepared);

public record KitchenChitDto(
    Guid Id,
    Guid TenantId,
    Guid OutletId,
    Guid OrderId,
    string OrderNumber,
    string DiningOption,
    string? CustomerName,
    string? TableNumber,
    string Station,
    string Status,
    int ElapsedSeconds,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    IReadOnlyList<KitchenChitItemDto> Items);

public record BumpChitRequest(string? NextStatus);
public record RecallChitRequest(string? TargetStatus);
