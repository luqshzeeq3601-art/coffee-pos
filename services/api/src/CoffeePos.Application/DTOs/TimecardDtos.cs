namespace CoffeePos.Application.DTOs;

public record StaffTimecardDto(
    Guid Id,
    Guid TenantId,
    Guid OutletId,
    string OutletName,
    Guid EmployeeId,
    string EmployeeName,
    DateTime ClockInUtc,
    DateTime? ClockOutUtc,
    int DurationMinutes,
    decimal RegularHours,
    decimal OvertimeHours,
    string Status,
    string? Notes);

public record ClockInRequest(
    Guid OutletId,
    string Pin);

public record ClockOutRequest(
    string Pin,
    string? Notes);

public record ApproveTimecardRequest(
    string? Notes);
