using System.Collections.Concurrent;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Devices;
using CoffeePos.Domain.Identity;
using CoffeePos.Domain.Tenancy;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryIdentityStore : IIdentityStore
{
    private readonly ConcurrentDictionary<Guid, Tenant> _tenants = new();
    private readonly ConcurrentDictionary<Guid, Outlet> _outlets = new();
    private readonly ConcurrentDictionary<Guid, User> _users = new();
    private readonly ConcurrentDictionary<Guid, Employee> _employees = new();
    private readonly ConcurrentDictionary<Guid, Device> _devices = new();

    public InMemoryIdentityStore(IPasswordHasher passwordHasher)
    {
        SeedDefaults(passwordHasher);
    }

    private void SeedDefaults(IPasswordHasher hasher)
    {
        var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var tenant = new Tenant(tenantId, "Roast Ledger Coffee", "MYR", "Asia/Kuala_Lumpur");
        _tenants[tenantId] = tenant;

        var outletId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var outlet = new Outlet(outletId, tenantId, "Bangsar Flagship", "Jalan Telawi 3, Bangsar", isMainOutlet: true);
        _outlets[outletId] = outlet;

        var (ownerHash, ownerSalt) = hasher.HashPassword("OwnerPass123!");
        var ownerUser = new User(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            tenantId,
            "owner@coffee-pos.local",
            "Shop Owner",
            ownerHash,
            ownerSalt,
            Role.Owner);
        _users[ownerUser.Id] = ownerUser;

        var (mgrHash, mgrSalt) = hasher.HashPin("1234");
        var managerEmp = new Employee(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            tenantId,
            "Siti Manager",
            mgrHash,
            mgrSalt,
            new[] { Role.Manager },
            new[] { outletId });
        _employees[managerEmp.Id] = managerEmp;

        var (cashierHash, cashierSalt) = hasher.HashPin("5678");
        var cashierEmp = new Employee(
            Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            tenantId,
            "Ahmad Cashier",
            cashierHash,
            cashierSalt,
            new[] { Role.Cashier },
            new[] { outletId });
        _employees[cashierEmp.Id] = cashierEmp;

        var deviceId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var device = new Device(deviceId, tenantId, outletId, "POS Register 1", DeviceType.POS, enrollmentCode: "ENROLL123");
        device.Enroll("POS-TERMINAL-01-FINGERPRINT");
        _devices[deviceId] = device;
    }

    public Task<Tenant?> GetTenantByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        _tenants.TryGetValue(tenantId, out var tenant);
        return Task.FromResult(tenant);
    }

    public Task<Outlet?> GetOutletByIdAsync(Guid outletId, CancellationToken cancellationToken = default)
    {
        _outlets.TryGetValue(outletId, out var outlet);
        return Task.FromResult(outlet);
    }

    public Task<IReadOnlyList<Outlet>> GetOutletsByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Outlet> results = _outlets.Values
            .Where(o => o.TenantId == tenantId && o.IsActive)
            .ToList();
        return Task.FromResult(results);
    }

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = _users.Values.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        _users.TryGetValue(userId, out var user);
        return Task.FromResult(user);
    }

    public Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        _employees.TryGetValue(employeeId, out var employee);
        return Task.FromResult(employee);
    }

    public Task<IReadOnlyList<Employee>> GetEmployeesByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Employee> results = _employees.Values
            .Where(e => e.TenantId == tenantId && e.IsActive)
            .ToList();
        return Task.FromResult(results);
    }

    public Task<Device?> GetDeviceByIdAsync(Guid deviceId, CancellationToken cancellationToken = default)
    {
        _devices.TryGetValue(deviceId, out var device);
        return Task.FromResult(device);
    }

    public Task<Device?> GetDeviceByEnrollmentCodeAsync(string enrollmentCode, CancellationToken cancellationToken = default)
    {
        var device = _devices.Values.FirstOrDefault(d => string.Equals(d.EnrollmentCode, enrollmentCode, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(device);
    }

    public Task SaveDeviceAsync(Device device, CancellationToken cancellationToken = default)
    {
        _devices[device.Id] = device;
        return Task.CompletedTask;
    }
}
