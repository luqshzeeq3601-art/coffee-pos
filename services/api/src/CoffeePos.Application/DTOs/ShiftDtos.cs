namespace CoffeePos.Application.DTOs;

public record CashMovementDto(
    Guid Id,
    Guid ShiftId,
    string Type,
    decimal Amount,
    string Reason,
    Guid CashierId,
    string CashierName,
    DateTime CreatedAtUtc);

public record ShiftDto(
    Guid Id,
    Guid TenantId,
    Guid OutletId,
    Guid CashierId,
    string CashierName,
    string Status,
    DateTime OpenedAtUtc,
    DateTime? ClosedAtUtc,
    decimal OpeningFloat,
    decimal CashSales,
    decimal CashRefunds,
    decimal CashInTotal,
    decimal CashOutTotal,
    decimal ExpectedCash,
    decimal? ActualCountedCash,
    decimal? Variance,
    IReadOnlyList<CashMovementDto> Movements);

public record OpenShiftRequest(
    decimal OpeningFloat);

public record CashMovementRequest(
    string Type,
    decimal Amount,
    string Reason);

public record CloseShiftRequest(
    decimal ActualCountedCash,
    string? ClosingNotes);

public record ShiftSummaryReportDto(
    Guid ShiftId,
    string OutletName,
    string CashierName,
    DateTime OpenedAtUtc,
    DateTime? ClosedAtUtc,
    bool IsClosed,
    decimal OpeningFloat,
    decimal CashSales,
    decimal CardSales,
    decimal QrSales,
    decimal GrossSales,
    decimal DiscountsTotal,
    decimal TaxTotal,
    decimal NetSales,
    decimal CashIn,
    decimal CashOut,
    decimal ExpectedCashInDrawer,
    decimal? ActualCountedCash,
    decimal? Variance);
