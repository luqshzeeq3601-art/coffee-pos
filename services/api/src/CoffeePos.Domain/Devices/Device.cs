namespace CoffeePos.Domain.Devices;

public enum DeviceType
{
    POS = 1,
    KDS = 2,
    KitchenDisplay = 3,
    BarcodeScanner = 4
}

public enum DeviceStatus
{
    PendingEnrollment = 1,
    Active = 2,
    Revoked = 3,
    Offline = 4
}

public sealed class Device
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public string Name { get; private set; }
    public DeviceType DeviceType { get; private set; }
    public DeviceStatus Status { get; private set; }
    public string? EnrollmentCode { get; private set; }
    public string? HardwareFingerprint { get; private set; }
    public DateTime? EnrolledAtUtc { get; private set; }
    public DateTime? LastSeenAtUtc { get; private set; }

    public Device(
        Guid id,
        Guid tenantId,
        Guid outletId,
        string name,
        DeviceType deviceType,
        string? enrollmentCode = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Device ID cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Device name cannot be empty.", nameof(name));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        Name = name.Trim();
        DeviceType = deviceType;
        Status = enrollmentCode != null ? DeviceStatus.PendingEnrollment : DeviceStatus.Active;
        EnrollmentCode = enrollmentCode;
        if (Status == DeviceStatus.Active)
        {
            EnrolledAtUtc = DateTime.UtcNow;
        }
    }

    public void Enroll(string? hardwareFingerprint)
    {
        Status = DeviceStatus.Active;
        HardwareFingerprint = hardwareFingerprint;
        EnrollmentCode = null;
        EnrolledAtUtc = DateTime.UtcNow;
        LastSeenAtUtc = DateTime.UtcNow;
    }

    public void Touch()
    {
        LastSeenAtUtc = DateTime.UtcNow;
    }

    public void Revoke()
    {
        Status = DeviceStatus.Revoked;
    }
}
