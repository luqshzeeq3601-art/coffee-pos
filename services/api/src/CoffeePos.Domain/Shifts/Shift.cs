namespace CoffeePos.Domain.Shifts;

public sealed class CashMovement
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ShiftId { get; private set; }
    public CashMovementType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string Reason { get; private set; }
    public Guid CashierId { get; private set; }
    public string CashierName { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public CashMovement(
        Guid id,
        Guid tenantId,
        Guid shiftId,
        CashMovementType type,
        decimal amount,
        string reason,
        Guid cashierId,
        string cashierName)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (shiftId == Guid.Empty) throw new ArgumentException("Shift ID cannot be empty.", nameof(shiftId));
        if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Reason is required for cash movements.", nameof(reason));

        Id = id;
        TenantId = tenantId;
        ShiftId = shiftId;
        Type = type;
        Amount = amount;
        Reason = reason.Trim();
        CashierId = cashierId;
        CashierName = cashierName.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }
}

public sealed class Shift
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public Guid CashierId { get; private set; }
    public string CashierName { get; private set; }
    public ShiftStatus Status { get; private set; }
    public DateTime OpenedAtUtc { get; private set; }
    public DateTime? ClosedAtUtc { get; private set; }

    public decimal OpeningFloat { get; private set; }
    public decimal CashSales { get; private set; }
    public decimal CashRefunds { get; private set; }
    public decimal? ActualCountedCash { get; private set; }
    public string? ClosingNotes { get; private set; }

    private readonly List<CashMovement> _movements = new();
    public IReadOnlyList<CashMovement> Movements => _movements.AsReadOnly();

    public decimal CashInTotal => _movements.Where(m => m.Type == CashMovementType.CashIn).Sum(m => m.Amount);
    public decimal CashOutTotal => _movements.Where(m => m.Type == CashMovementType.CashOut).Sum(m => m.Amount);
    public decimal ExpectedCash => OpeningFloat + CashSales - CashRefunds + CashInTotal - CashOutTotal;
    public decimal? Variance => ActualCountedCash.HasValue ? ActualCountedCash.Value - ExpectedCash : null;

    public Shift(
        Guid id,
        Guid tenantId,
        Guid outletId,
        Guid cashierId,
        string cashierName,
        decimal openingFloat)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (openingFloat < 0) throw new ArgumentException("Opening float cannot be negative.", nameof(openingFloat));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        CashierId = cashierId;
        CashierName = cashierName.Trim();
        Status = ShiftStatus.Open;
        OpeningFloat = openingFloat;
        CashSales = 0m;
        CashRefunds = 0m;
        OpenedAtUtc = DateTime.UtcNow;
    }

    public void AddCashSale(decimal amount)
    {
        EnsureOpen();
        CashSales += amount;
    }

    public void AddCashRefund(decimal amount)
    {
        EnsureOpen();
        CashRefunds += amount;
    }

    public void AddCashMovement(CashMovement movement)
    {
        EnsureOpen();
        _movements.Add(movement);
    }

    public void CloseShift(decimal actualCountedCash, string? notes = null)
    {
        EnsureOpen();
        if (actualCountedCash < 0) throw new ArgumentException("Actual counted cash cannot be negative.", nameof(actualCountedCash));

        ActualCountedCash = actualCountedCash;
        ClosingNotes = notes?.Trim();
        Status = ShiftStatus.Closed;
        ClosedAtUtc = DateTime.UtcNow;
    }

    private void EnsureOpen()
    {
        if (Status == ShiftStatus.Closed)
            throw new InvalidOperationException("Cannot perform cash operations on a closed shift.");
    }
}
