using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Staff;

namespace CoffeePos.Infrastructure.Services;

public sealed class TimecardService : ITimecardService
{
    private readonly ITimecardStore _timecardStore;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentUser _currentUser;

    public TimecardService(
        ITimecardStore timecardStore,
        ITenantContext tenantContext,
        ICurrentUser currentUser)
    {
        _timecardStore = timecardStore;
        _tenantContext = tenantContext;
        _currentUser = currentUser;
    }

    private (Guid TenantId, Guid UserId, string UserName) RequireContext()
    {
        var tenantId = _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
        var userId = _currentUser.UserId ?? Guid.Parse("33333333-3333-3333-3333-333333333331");
        var userName = _currentUser.Email ?? "Ahmad Cashier";
        return (tenantId, userId, userName);
    }

    public async Task<IReadOnlyList<StaffTimecardDto>> GetTimecardsAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var list = await _timecardStore.GetTimecardsAsync(tenantId, cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<StaffTimecardDto> ClockInAsync(ClockInRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, userId, userName) = RequireContext();
        var existing = await _timecardStore.GetActiveTimecardAsync(tenantId, userId, cancellationToken);
        if (existing != null)
        {
            throw new InvalidOperationException("User is already clocked in on an active shift.");
        }

        var timecard = new StaffTimecard(Guid.NewGuid(), tenantId, request.OutletId, userId, userName);
        await _timecardStore.SaveTimecardAsync(timecard, cancellationToken);
        return MapToDto(timecard);
    }

    public async Task<StaffTimecardDto> ClockOutAsync(ClockOutRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, userId, _) = RequireContext();
        var timecard = await _timecardStore.GetActiveTimecardAsync(tenantId, userId, cancellationToken)
            ?? throw new InvalidOperationException("No active clocked-in shift found for user.");

        timecard.ClockOut(request.notes);
        await _timecardStore.SaveTimecardAsync(timecard, cancellationToken);
        return MapToDto(timecard);
    }

    public async Task<StaffTimecardDto> ApproveTimecardAsync(Guid id, ApproveTimecardRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var timecard = await _timecardStore.GetTimecardByIdAsync(tenantId, id, cancellationToken)
            ?? throw new KeyNotFoundException($"Timecard {id} not found.");

        timecard.Approve(request.Notes);
        await _timecardStore.SaveTimecardAsync(timecard, cancellationToken);
        return MapToDto(timecard);
    }

    private static StaffTimecardDto MapToDto(StaffTimecard t)
    {
        return new StaffTimecardDto(
            t.Id,
            t.TenantId,
            t.OutletId,
            t.OutletId == Guid.Parse("22222222-2222-2222-2222-222222222221") ? "Bangsar Flagship" : "Damansara Heights",
            t.EmployeeId,
            t.EmployeeName,
            t.ClockInUtc,
            t.ClockOutUtc,
            t.DurationMinutes,
            t.RegularHours,
            t.OvertimeHours,
            t.Status.ToString(),
            t.Notes);
    }
}
