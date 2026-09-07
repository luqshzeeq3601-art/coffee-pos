namespace CoffeePos.Domain.Staff;

public enum TimecardStatus
{
    ClockedIn = 1,
    ClockedOut = 2,
    Approved = 3
}

public sealed class StaffTimecard
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public string EmployeeName { get; private set; }
    public DateTime ClockInUtc { get; private set; }
    public DateTime? ClockOutUtc { get; private set; }
    public int DurationMinutes { get; private set; }
    public decimal RegularHours { get; private set; }
    public decimal OvertimeHours { get; private set; }
    public TimecardStatus Status { get; private set; }
    public string? Notes { get; private set; }

    public StaffTimecard(
        Guid id,
        Guid tenantId,
        Guid outletId,
        Guid employeeId,
        string employeeName)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (employeeId == Guid.Empty) throw new ArgumentException("Employee ID cannot be empty.", nameof(employeeId));
        if (string.IsNullOrWhiteSpace(employeeName)) throw new ArgumentException("Employee name is required.", nameof(employeeName));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        EmployeeId = employeeId;
        EmployeeName = employeeName.Trim();
        ClockInUtc = DateTime.UtcNow;
        Status = TimecardStatus.ClockedIn;
    }

    public void ClockOut(string? notes = null)
    {
        if (Status != TimecardStatus.ClockedIn)
            throw new InvalidOperationException("Timecard is already clocked out.");

        ClockOutUtc = DateTime.UtcNow;
        Status = TimecardStatus.ClockedOut;
        Notes = notes?.Trim();

        var diff = ClockOutUtc.Value - ClockInUtc;
        DurationMinutes = (int)diff.TotalMinutes;

        var totalHours = (decimal)diff.TotalHours;
        if (totalHours <= 8.0m)
        {
            RegularHours = Math.Round(totalHours, 2);
            OvertimeHours = 0m;
        }
        else
        {
            RegularHours = 8.0m;
            OvertimeHours = Math.Round(totalHours - 8.0m, 2);
        }
    }

    public void Approve(string? managerNotes = null)
    {
        if (Status == TimecardStatus.ClockedIn)
            throw new InvalidOperationException("Cannot approve a timecard that is still active.");

        Status = TimecardStatus.Approved;
        if (!string.IsNullOrWhiteSpace(managerNotes))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? managerNotes.Trim() : $"{Notes} | Approved: {managerNotes.Trim()}";
        }
    }
}
