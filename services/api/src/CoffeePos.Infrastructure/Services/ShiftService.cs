using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Audit;
using CoffeePos.Domain.Shifts;

namespace CoffeePos.Infrastructure.Services;

public sealed class ShiftService : IShiftService
{
    private readonly IShiftStore _shiftStore;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _auditService;

    public ShiftService(
        IShiftStore shiftStore,
        ITenantContext tenantContext,
        ICurrentUser currentUser,
        IAuditService auditService)
    {
        _shiftStore = shiftStore;
        _tenantContext = tenantContext;
        _currentUser = currentUser;
        _auditService = auditService;
    }

    private (Guid TenantId, Guid OutletId, Guid CashierId, string CashierName) RequireContext()
    {
        var tenantId = _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
        var outletId = _tenantContext.OutletId ?? Guid.Parse("22222222-2222-2222-2222-222222222221");
        var cashierId = _currentUser.UserId ?? Guid.Parse("33333333-3333-3333-3333-333333333331");
        var cashierName = _currentUser.Email ?? "Ahmad Cashier";
        return (tenantId, outletId, cashierId, cashierName);
    }

    public async Task<ShiftDto?> GetActiveShiftAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId, cashierId, _) = RequireContext();
        var shift = await _shiftStore.GetActiveShiftAsync(tenantId, outletId, cashierId, cancellationToken);
        return shift != null ? MapToDto(shift) : null;
    }

    public async Task<ShiftDto> OpenShiftAsync(OpenShiftRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId, cashierId, cashierName) = RequireContext();

        var existing = await _shiftStore.GetActiveShiftAsync(tenantId, outletId, cashierId, cancellationToken);
        if (existing != null)
        {
            throw new InvalidOperationException("Cashier already has an active open shift.");
        }

        var shift = new Shift(
            Guid.NewGuid(),
            tenantId,
            outletId,
            cashierId,
            cashierName,
            request.OpeningFloat);

        await _shiftStore.SaveShiftAsync(shift, cancellationToken);

        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            cashierName,
            "User",
            "OpenShift",
            $"Opened shift with RM {request.OpeningFloat:F2} opening float"));

        return MapToDto(shift);
    }

    public async Task<ShiftDto> RecordCashMovementAsync(Guid shiftId, CashMovementRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, cashierId, cashierName) = RequireContext();
        var shift = await _shiftStore.GetShiftByIdAsync(tenantId, shiftId, cancellationToken)
            ?? throw new KeyNotFoundException($"Shift with ID {shiftId} not found.");

        Enum.TryParse<CashMovementType>(request.Type, true, out var movementType);
        if (movementType == 0) movementType = CashMovementType.CashOut;

        var movement = new CashMovement(
            Guid.NewGuid(),
            tenantId,
            shift.Id,
            movementType,
            request.Amount,
            request.Reason,
            cashierId,
            cashierName);

        shift.AddCashMovement(movement);
        await _shiftStore.SaveShiftAsync(shift, cancellationToken);

        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            cashierName,
            "User",
            "CashMovement",
            $"Recorded {movementType} of RM {request.Amount:F2}. Reason: {request.Reason}"));

        return MapToDto(shift);
    }

    public async Task<ShiftSummaryReportDto> CloseShiftAsync(Guid shiftId, CloseShiftRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _, cashierName) = RequireContext();
        var shift = await _shiftStore.GetShiftByIdAsync(tenantId, shiftId, cancellationToken)
            ?? throw new KeyNotFoundException($"Shift with ID {shiftId} not found.");

        shift.CloseShift(request.ActualCountedCash, request.ClosingNotes);
        await _shiftStore.SaveShiftAsync(shift, cancellationToken);

        var variance = shift.Variance ?? 0m;
        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            cashierName,
            "User",
            "CloseShift",
            $"Closed shift with Counted Cash: RM {request.ActualCountedCash:F2}, Expected: RM {shift.ExpectedCash:F2}, Variance: RM {variance:F2}"));

        return GenerateReport(shift);
    }

    public async Task<ShiftSummaryReportDto> GetShiftReportAsync(Guid shiftId, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _, _) = RequireContext();
        var shift = await _shiftStore.GetShiftByIdAsync(tenantId, shiftId, cancellationToken)
            ?? throw new KeyNotFoundException($"Shift with ID {shiftId} not found.");

        return GenerateReport(shift);
    }

    private static ShiftDto MapToDto(Shift s)
    {
        var movements = s.Movements.Select(m => new CashMovementDto(
            m.Id,
            m.ShiftId,
            m.Type.ToString(),
            m.Amount,
            m.Reason,
            m.CashierId,
            m.CashierName,
            m.CreatedAtUtc)).ToList();

        return new ShiftDto(
            s.Id,
            s.TenantId,
            s.OutletId,
            s.CashierId,
            s.CashierName,
            s.Status.ToString(),
            s.OpenedAtUtc,
            s.ClosedAtUtc,
            s.OpeningFloat,
            s.CashSales,
            s.CashRefunds,
            s.CashInTotal,
            s.CashOutTotal,
            s.ExpectedCash,
            s.ActualCountedCash,
            s.Variance,
            movements);
    }

    private static ShiftSummaryReportDto GenerateReport(Shift s)
    {
        var grossSales = s.CashSales * 1.8m; // Demo simulation gross across all tenders
        var cardSales = s.CashSales * 0.5m;
        var qrSales = s.CashSales * 0.3m;
        var taxTotal = Math.Round(grossSales * 0.06m, 2);
        var netSales = grossSales - taxTotal;

        return new ShiftSummaryReportDto(
            s.Id,
            "Bangsar Flagship",
            s.CashierName,
            s.OpenedAtUtc,
            s.ClosedAtUtc,
            s.Status == ShiftStatus.Closed,
            s.OpeningFloat,
            s.CashSales,
            cardSales,
            qrSales,
            grossSales,
            0m,
            taxTotal,
            netSales,
            s.CashInTotal,
            s.CashOutTotal,
            s.ExpectedCash,
            s.ActualCountedCash,
            s.Variance);
    }
}
