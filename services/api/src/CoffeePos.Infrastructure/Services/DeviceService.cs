using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Devices;

namespace CoffeePos.Infrastructure.Services;

public sealed class DeviceService : IDeviceService
{
    private readonly IIdentityStore _store;
    private readonly ITokenService _tokenService;
    private readonly IAuditService _audit;

    public DeviceService(IIdentityStore store, ITokenService tokenService, IAuditService audit)
    {
        _store = store;
        _tokenService = tokenService;
        _audit = audit;
    }

    public async Task<DeviceEnrollmentResponseDto> EnrollDeviceAsync(
        DeviceEnrollmentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.EnrollmentCode))
        {
            throw new ArgumentException("Enrollment code is required.");
        }

        var device = await _store.GetDeviceByEnrollmentCodeAsync(request.EnrollmentCode, cancellationToken);
        if (device == null || device.Status != DeviceStatus.PendingEnrollment)
        {
            throw new UnauthorizedAccessException("Invalid or expired enrollment code.");
        }

        device.Enroll(request.HardwareFingerprint);
        await _store.SaveDeviceAsync(device, cancellationToken);

        await _audit.RecordAsync(
            device.Id.ToString(),
            "Device",
            "device:enrolled",
            device.TenantId,
            device.OutletId,
            device.Id,
            reason: $"Enrolled as {request.DeviceName}",
            cancellationToken: cancellationToken);

        return new DeviceEnrollmentResponseDto(
            DeviceId: device.Id.ToString(),
            TenantId: device.TenantId.ToString(),
            OutletId: device.OutletId.ToString(),
            DeviceToken: Guid.NewGuid().ToString("N"),
            Status: device.Status.ToString());
    }

    public async Task<DeviceDto?> GetDeviceAsync(Guid deviceId, CancellationToken cancellationToken = default)
    {
        var device = await _store.GetDeviceByIdAsync(deviceId, cancellationToken);
        if (device == null) return null;

        return new DeviceDto(
            Id: device.Id.ToString(),
            TenantId: device.TenantId.ToString(),
            OutletId: device.OutletId.ToString(),
            Name: device.Name,
            DeviceType: device.DeviceType.ToString(),
            Status: device.Status.ToString(),
            EnrolledAtUtc: device.EnrolledAtUtc?.ToString("o"),
            LastSeenAtUtc: device.LastSeenAtUtc?.ToString("o"));
    }
}
