using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Staff;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryTimecardStore : ITimecardStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly List<StaffTimecard> _timecards = new();

    public InMemoryTimecardStore()
    {
        SeedTimecards();
    }

    private void SeedTimecards()
    {
        var tc1 = new StaffTimecard(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
            DemoTenantId,
            Guid.Parse("22222222-2222-2222-2222-222222222221"),
            Guid.Parse("33333333-3333-3333-3333-333333333331"),
            "Ahmad Cashier");
        tc1.ClockOut("Morning shift complete");
        tc1.Approve("Manager approved");

        var tc2 = new StaffTimecard(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
            DemoTenantId,
            Guid.Parse("22222222-2222-2222-2222-222222222221"),
            Guid.Parse("33333333-3333-3333-3333-333333333332"),
            "Danial Barista");

        _timecards.AddRange(new[] { tc1, tc2 });
    }

    public Task<IReadOnlyList<StaffTimecard>> GetTimecardsAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var list = _timecards
            .Where(t => t.TenantId == tenantId)
            .OrderByDescending(t => t.ClockInUtc)
            .ToList();
        return Task.FromResult<IReadOnlyList<StaffTimecard>>(list);
    }

    public Task<StaffTimecard?> GetActiveTimecardAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken = default)
    {
        var tc = _timecards.FirstOrDefault(t => t.TenantId == tenantId && t.EmployeeId == employeeId && t.Status == TimecardStatus.ClockedIn);
        return Task.FromResult(tc);
    }

    public Task<StaffTimecard?> GetTimecardByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        var tc = _timecards.FirstOrDefault(t => t.TenantId == tenantId && t.Id == id);
        return Task.FromResult(tc);
    }

    public Task SaveTimecardAsync(StaffTimecard timecard, CancellationToken cancellationToken = default)
    {
        var idx = _timecards.FindIndex(t => t.Id == timecard.Id && t.TenantId == timecard.TenantId);
        if (idx >= 0)
        {
            _timecards[idx] = timecard;
        }
        else
        {
            _timecards.Add(timecard);
        }
        return Task.CompletedTask;
    }
}
