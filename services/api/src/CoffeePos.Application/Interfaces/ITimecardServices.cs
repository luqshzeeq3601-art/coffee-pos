using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Staff;

namespace CoffeePos.Application.Interfaces;

public interface ITimecardStore
{
    Task<IReadOnlyList<StaffTimecard>> GetTimecardsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<StaffTimecard?> GetActiveTimecardAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken = default);
    Task<StaffTimecard?> GetTimecardByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default);
    Task SaveTimecardAsync(StaffTimecard timecard, CancellationToken cancellationToken = default);
}

public interface ITimecardService
{
    Task<IReadOnlyList<StaffTimecardDto>> GetTimecardsAsync(CancellationToken cancellationToken = default);
    Task<StaffTimecardDto> ClockInAsync(ClockInRequest request, CancellationToken cancellationToken = default);
    Task<StaffTimecardDto> ClockOutAsync(ClockOutRequest request, CancellationToken cancellationToken = default);
    Task<StaffTimecardDto> ApproveTimecardAsync(Guid id, ApproveTimecardRequest request, CancellationToken cancellationToken = default);
}
