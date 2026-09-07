using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Shifts;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryShiftStore : IShiftStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoOutletId = Guid.Parse("22222222-2222-2222-2222-222222222221");
    private static readonly Guid DemoCashierId = Guid.Parse("33333333-3333-3333-3333-333333333331");

    private readonly List<Shift> _shifts = new();

    public InMemoryShiftStore()
    {
        SeedActiveShift();
    }

    private void SeedActiveShift()
    {
        var shift = new Shift(
            Guid.Parse("99999999-9999-9999-9999-999999999901"),
            DemoTenantId,
            DemoOutletId,
            DemoCashierId,
            "Ahmad Cashier",
            300.00m); // RM 300.00 opening float

        shift.AddCashSale(1420.50m);
        shift.AddCashMovement(new CashMovement(
            Guid.NewGuid(),
            DemoTenantId,
            shift.Id,
            CashMovementType.CashOut,
            25.00m,
            "Emergency ice bag purchase",
            DemoCashierId,
            "Ahmad Cashier"));

        _shifts.Add(shift);
    }

    public Task<Shift?> GetActiveShiftAsync(Guid tenantId, Guid outletId, Guid cashierId, CancellationToken cancellationToken = default)
    {
        var shift = _shifts.FirstOrDefault(s => s.TenantId == tenantId && s.OutletId == outletId && s.CashierId == cashierId && s.Status == ShiftStatus.Open);
        // Fallback for demo mode
        if (shift == null)
        {
            shift = _shifts.FirstOrDefault(s => s.TenantId == tenantId && s.Status == ShiftStatus.Open);
        }
        return Task.FromResult(shift);
    }

    public Task<Shift?> GetShiftByIdAsync(Guid tenantId, Guid shiftId, CancellationToken cancellationToken = default)
    {
        var shift = _shifts.FirstOrDefault(s => s.TenantId == tenantId && s.Id == shiftId);
        return Task.FromResult(shift);
    }

    public Task SaveShiftAsync(Shift shift, CancellationToken cancellationToken = default)
    {
        var idx = _shifts.FindIndex(s => s.Id == shift.Id && s.TenantId == shift.TenantId);
        if (idx >= 0)
        {
            _shifts[idx] = shift;
        }
        else
        {
            _shifts.Add(shift);
        }
        return Task.CompletedTask;
    }
}
