using System.Text.Json;
using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Devices;
using CoffeePos.Domain.Identity;

namespace CoffeePos.Infrastructure.Services;

public sealed class PinAuthService : IPinAuthService
{
    private readonly IIdentityStore _store;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _audit;

    public PinAuthService(
        IIdentityStore store,
        IPasswordHasher hasher,
        ITokenService tokenService,
        ITenantContext tenantContext,
        ICurrentUser currentUser,
        IAuditService audit)
    {
        _store = store;
        _hasher = hasher;
        _tokenService = tokenService;
        _tenantContext = tenantContext;
        _currentUser = currentUser;
        _audit = audit;
    }

    public async Task<PinLoginResponseDto> PinLoginAsync(PinLoginRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceId) || !Guid.TryParse(request.DeviceId, out var deviceId))
        {
            throw new ArgumentException("Valid DeviceId is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Pin) || request.Pin.Length < 4)
        {
            throw new ArgumentException("PIN must be at least 4 digits.");
        }

        var device = await _store.GetDeviceByIdAsync(deviceId, cancellationToken);
        if (device == null || device.Status != DeviceStatus.Active)
        {
            throw new UnauthorizedAccessException("Device is not enrolled or inactive.");
        }

        var employees = await _store.GetEmployeesByTenantAsync(device.TenantId, cancellationToken);
        Employee? matchedEmployee = null;

        foreach (var emp in employees)
        {
            if (_hasher.VerifyPin(request.Pin, emp.PinHash, emp.PinSalt))
            {
                matchedEmployee = emp;
                break;
            }
        }

        if (matchedEmployee == null || !matchedEmployee.IsActive)
        {
            await _audit.RecordAsync(
                "unknown",
                "Employee",
                "auth:pin_failed",
                device.TenantId,
                device.OutletId,
                device.Id,
                reason: "Invalid employee PIN",
                cancellationToken: cancellationToken);

            throw new UnauthorizedAccessException("Invalid PIN.");
        }

        if (!matchedEmployee.CanOperateAtOutlet(device.OutletId))
        {
            await _audit.RecordAsync(
                matchedEmployee.Id.ToString(),
                "Employee",
                "auth:pin_denied",
                device.TenantId,
                device.OutletId,
                device.Id,
                reason: "Employee not assigned to outlet",
                cancellationToken: cancellationToken);

            throw new UnauthorizedAccessException("Employee is not assigned to this outlet.");
        }

        device.Touch();
        await _store.SaveDeviceAsync(device, cancellationToken);

        var token = _tokenService.GenerateEmployeeToken(matchedEmployee, device.OutletId, device.Id);
        var expiresAt = DateTime.UtcNow.AddHours(12).ToString("o");

        var permissions = matchedEmployee.Roles
            .SelectMany(Permissions.GetPermissionsForRole)
            .Distinct()
            .ToList();

        await _audit.RecordAsync(
            matchedEmployee.Id.ToString(),
            "Employee",
            "auth:pin_success",
            device.TenantId,
            device.OutletId,
            device.Id,
            cancellationToken: cancellationToken);

        return new PinLoginResponseDto(
            Employee: new EmployeeProfileDto(
                matchedEmployee.Id.ToString(),
                matchedEmployee.DisplayName,
                matchedEmployee.Roles.Select(static r => r.ToString()).ToList(),
                matchedEmployee.TenantId.ToString(),
                matchedEmployee.AssignedOutletIds.Select(static o => o.ToString()).ToList(),
                matchedEmployee.IsActive ? "Active" : "Inactive"),
            Permissions: permissions,
            TenantId: device.TenantId.ToString(),
            OutletId: device.OutletId.ToString(),
            DeviceId: device.Id.ToString(),
            Token: token,
            ExpiresAtUtc: expiresAt);
    }

    public async Task<ManagerOverrideResponseDto> AuthorizeManagerOverrideAsync(
        ManagerOverrideRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.ManagerPin))
        {
            throw new ArgumentException("Manager PIN is required.");
        }

        if (!_tenantContext.HasTenant)
        {
            throw new UnauthorizedAccessException("Active tenant context is required for manager override.");
        }

        var tenantId = _tenantContext.TenantId!.Value;
        var employees = await _store.GetEmployeesByTenantAsync(tenantId, cancellationToken);
        Employee? managerEmp = null;

        foreach (var emp in employees)
        {
            if (emp.HasPermission(Permissions.OverrideAuthorize) && _hasher.VerifyPin(request.ManagerPin, emp.PinHash, emp.PinSalt))
            {
                managerEmp = emp;
                break;
            }
        }

        if (managerEmp == null || !managerEmp.IsActive)
        {
            await _audit.RecordAsync(
                _currentUser.Id ?? "unknown",
                _currentUser.UserType ?? "Unknown",
                "override:denied",
                tenantId,
                _tenantContext.OutletId,
                _tenantContext.DeviceId,
                reason: $"Failed manager PIN for action '{request.Action}'",
                cancellationToken: cancellationToken);

            return new ManagerOverrideResponseDto(
                Authorized: false,
                AuthorizingEmployeeId: null,
                AuthorizingEmployeeName: null,
                AuditEventId: null);
        }

        var auditEventId = Guid.NewGuid();
        var metadataJson = request.Metadata != null ? JsonSerializer.Serialize(request.Metadata) : null;

        await _audit.RecordAsync(
            managerEmp.Id.ToString(),
            "Employee",
            $"override:{request.Action}",
            tenantId,
            _tenantContext.OutletId,
            _tenantContext.DeviceId,
            reason: request.Reason ?? "Manager approved override",
            metadataJson: metadataJson,
            cancellationToken: cancellationToken);

        return new ManagerOverrideResponseDto(
            Authorized: true,
            AuthorizingEmployeeId: managerEmp.Id.ToString(),
            AuthorizingEmployeeName: managerEmp.DisplayName,
            AuditEventId: auditEventId.ToString());
    }
}
