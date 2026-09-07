using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Audit;
using CoffeePos.Domain.Devices;
using CoffeePos.Domain.Identity;
using CoffeePos.Domain.Tenancy;

namespace CoffeePos.Application.Interfaces;

public interface IIdentityStore
{
    Task<Tenant?> GetTenantByIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<Outlet?> GetOutletByIdAsync(Guid outletId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Outlet>> GetOutletsByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Employee>> GetEmployeesByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<Device?> GetDeviceByIdAsync(Guid deviceId, CancellationToken cancellationToken = default);
    Task<Device?> GetDeviceByEnrollmentCodeAsync(string enrollmentCode, CancellationToken cancellationToken = default);
    Task SaveDeviceAsync(Device device, CancellationToken cancellationToken = default);
}

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<LoginResponseDto> VerifyMfaAsync(MfaVerifyRequestDto request, CancellationToken cancellationToken = default);
    Task<SessionStateDto> GetCurrentSessionAsync(CancellationToken cancellationToken = default);
}

public interface IPinAuthService
{
    Task<PinLoginResponseDto> PinLoginAsync(PinLoginRequestDto request, CancellationToken cancellationToken = default);
    Task<ManagerOverrideResponseDto> AuthorizeManagerOverrideAsync(ManagerOverrideRequestDto request, CancellationToken cancellationToken = default);
}

public interface IDeviceService
{
    Task<DeviceEnrollmentResponseDto> EnrollDeviceAsync(DeviceEnrollmentRequestDto request, CancellationToken cancellationToken = default);
    Task<DeviceDto?> GetDeviceAsync(Guid deviceId, CancellationToken cancellationToken = default);
}

public interface IAuditService
{
    Task RecordAsync(
        string actorId,
        string actorType,
        string action,
        Guid tenantId,
        Guid? outletId = null,
        Guid? deviceId = null,
        string? reason = null,
        string? metadataJson = null,
        CancellationToken cancellationToken = default);
}
