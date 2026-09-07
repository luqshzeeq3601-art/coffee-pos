namespace CoffeePos.Application.DTOs;

public sealed record LoginRequestDto(
    string Email,
    string Password);

public sealed record UserProfileDto(
    string Id,
    string Email,
    string FullName,
    string Role,
    string TenantId,
    string Status);

public sealed record OutletSummaryDto(
    string Id,
    string Name);

public sealed record LoginResponseDto(
    bool RequiresMfa,
    string? MfaChallengeId,
    UserProfileDto? User,
    string? ActiveTenantId,
    IReadOnlyList<OutletSummaryDto>? AvailableOutlets,
    string? Token,
    string? ExpiresAtUtc);

public sealed record MfaVerifyRequestDto(
    string ChallengeId,
    string Code);

public sealed record PinLoginRequestDto(
    string DeviceId,
    string Pin);

public sealed record EmployeeProfileDto(
    string Id,
    string DisplayName,
    IReadOnlyList<string> Roles,
    string TenantId,
    IReadOnlyList<string> AssignedOutletIds,
    string Status);

public sealed record PinLoginResponseDto(
    EmployeeProfileDto Employee,
    IReadOnlyList<string> Permissions,
    string TenantId,
    string OutletId,
    string DeviceId,
    string Token,
    string ExpiresAtUtc);

public sealed record ManagerOverrideRequestDto(
    string ManagerPin,
    string Action,
    string? Reason,
    IReadOnlyDictionary<string, string>? Metadata);

public sealed record ManagerOverrideResponseDto(
    bool Authorized,
    string? AuthorizingEmployeeId,
    string? AuthorizingEmployeeName,
    string? AuditEventId);

public sealed record SessionStateDto(
    bool IsAuthenticated,
    string? UserType,
    string? UserId,
    string? DisplayName,
    string? TenantId,
    string? OutletId,
    string? DeviceId,
    IReadOnlyList<string> Roles,
    IReadOnlyList<string> Permissions);
