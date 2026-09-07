using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Devices;
using CoffeePos.Domain.Identity;
using CoffeePos.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace CoffeePos.Infrastructure.Persistence.Repositories;

public sealed class PostgresIdentityStore : IIdentityStore
{
    private readonly CoffeePosDbContext _dbContext;

    public PostgresIdentityStore(CoffeePosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Tenant?> GetTenantByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);
    }

    public async Task<Outlet?> GetOutletByIdAsync(Guid outletId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Outlets.FirstOrDefaultAsync(o => o.Id == outletId, cancellationToken);
    }

    public async Task<IReadOnlyList<Outlet>> GetOutletsByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Outlets
            .Where(o => o.TenantId == tenantId && o.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive, cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);
    }

    public async Task<IReadOnlyList<Employee>> GetEmployeesByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Employees
            .Where(e => e.TenantId == tenantId && e.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Device?> GetDeviceByIdAsync(Guid deviceId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceId, cancellationToken);
    }

    public async Task<Device?> GetDeviceByEnrollmentCodeAsync(string enrollmentCode, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Devices.FirstOrDefaultAsync(d => d.EnrollmentCode == enrollmentCode, cancellationToken);
    }

    public async Task SaveDeviceAsync(Device device, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == device.Id, cancellationToken);
        if (existing == null)
        {
            await _dbContext.Devices.AddAsync(device, cancellationToken);
        }
        else
        {
            _dbContext.Devices.Update(device);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
