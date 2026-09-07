using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Shifts;

namespace CoffeePos.Application.Interfaces;

public interface IShiftStore
{
    Task<Shift?> GetActiveShiftAsync(Guid tenantId, Guid outletId, Guid cashierId, CancellationToken cancellationToken = default);
    Task<Shift?> GetShiftByIdAsync(Guid tenantId, Guid shiftId, CancellationToken cancellationToken = default);
    Task SaveShiftAsync(Shift shift, CancellationToken cancellationToken = default);
}

public interface IShiftService
{
    Task<ShiftDto?> GetActiveShiftAsync(CancellationToken cancellationToken = default);
    Task<ShiftDto> OpenShiftAsync(OpenShiftRequest request, CancellationToken cancellationToken = default);
    Task<ShiftDto> RecordCashMovementAsync(Guid shiftId, CashMovementRequest request, CancellationToken cancellationToken = default);
    Task<ShiftSummaryReportDto> CloseShiftAsync(Guid shiftId, CloseShiftRequest request, CancellationToken cancellationToken = default);
    Task<ShiftSummaryReportDto> GetShiftReportAsync(Guid shiftId, CancellationToken cancellationToken = default);
}
