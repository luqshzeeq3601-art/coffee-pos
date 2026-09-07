namespace CoffeePos.Application.DTOs;

public record LoyaltyLedgerEntryDto(
    Guid Id,
    Guid TenantId,
    Guid CustomerId,
    string Type,
    int PointsDelta,
    int BalanceAfter,
    string Reason,
    string? OrderNumber,
    string PerformedByName,
    DateTime CreatedAtUtc);

public record CustomerDto(
    Guid Id,
    Guid TenantId,
    string Name,
    string PhoneNumber,
    string? Email,
    string Tier,
    int PointsBalance,
    decimal TotalSpent,
    int VisitCount,
    DateTime JoinedAtUtc,
    DateTime LastVisitUtc);

public record CreateCustomerRequest(
    string Name,
    string PhoneNumber,
    string? Email);

public record EarnPointsRequest(
    decimal AmountSpent,
    string OrderNumber);

public record RedeemPointsRequest(
    int PointsToRedeem,
    string OrderNumber);
