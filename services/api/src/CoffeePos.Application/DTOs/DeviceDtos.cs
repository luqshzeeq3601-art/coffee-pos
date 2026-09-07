namespace CoffeePos.Application.DTOs;

public sealed record DeviceDto(
    string Id,
    string TenantId,
    string OutletId,
    string Name,
    string DeviceType,
    string Status,
    string? EnrolledAtUtc,
    string? LastSeenAtUtc);

public sealed record DeviceEnrollmentRequestDto(
    string EnrollmentCode,
    string DeviceName,
    string DeviceType,
    string? HardwareFingerprint);

public sealed record DeviceEnrollmentResponseDto(
    string DeviceId,
    string TenantId,
    string OutletId,
    string DeviceToken,
    string Status);

public sealed record TenantDto(
    string Id,
    string Name,
    string CurrencyCode,
    string TimeZone,
    string CreatedAtUtc);

public sealed record OutletDto(
    string Id,
    string TenantId,
    string Name,
    string? Address,
    bool IsMainOutlet,
    string CreatedAtUtc);
