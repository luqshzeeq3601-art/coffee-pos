using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Loyalty;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryLoyaltyStore : ILoyaltyStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly List<Customer> _customers = new();
    private readonly List<LoyaltyLedgerEntry> _ledgerEntries = new();

    public InMemoryLoyaltyStore()
    {
        SeedCustomers();
    }

    private void SeedCustomers()
    {
        var cust1 = new Customer(
            Guid.Parse("66666666-6666-6666-6666-666666666001"),
            DemoTenantId,
            "Sarah Lee",
            "+60123456789",
            "sarah.lee@gmail.com");
        cust1.EarnPoints(245m, "ORD-INIT-01", Guid.Empty, "System Seed");

        var cust2 = new Customer(
            Guid.Parse("66666666-6666-6666-6666-666666666002"),
            DemoTenantId,
            "Farid Kamil",
            "+60198765432",
            "farid.k@yahoo.com");
        cust2.EarnPoints(520m, "ORD-INIT-02", Guid.Empty, "System Seed");

        var cust3 = new Customer(
            Guid.Parse("66666666-6666-6666-6666-666666666003"),
            DemoTenantId,
            "Alex Tan",
            "+60163334444",
            "alex.tan@outlook.com");
        cust3.EarnPoints(85m, "ORD-INIT-03", Guid.Empty, "System Seed");

        _customers.AddRange(new[] { cust1, cust2, cust3 });
    }

    public Task<IReadOnlyList<Customer>> SearchCustomersAsync(Guid tenantId, string query, CancellationToken cancellationToken = default)
    {
        var q = query.Trim().ToLowerInvariant();
        var matches = _customers
            .Where(c => c.TenantId == tenantId && (c.PhoneNumber.Contains(q) || c.Name.ToLowerInvariant().Contains(q) || (c.Email != null && c.Email.Contains(q))))
            .ToList();
        return Task.FromResult<IReadOnlyList<Customer>>(matches);
    }

    public Task<Customer?> GetCustomerByIdAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        var customer = _customers.FirstOrDefault(c => c.TenantId == tenantId && c.Id == customerId);
        return Task.FromResult(customer);
    }

    public Task<Customer?> GetCustomerByPhoneAsync(Guid tenantId, string phoneNumber, CancellationToken cancellationToken = default)
    {
        var customer = _customers.FirstOrDefault(c => c.TenantId == tenantId && c.PhoneNumber == phoneNumber.Trim());
        return Task.FromResult(customer);
    }

    public Task SaveCustomerAsync(Customer customer, LoyaltyLedgerEntry? ledgerEntry = null, CancellationToken cancellationToken = default)
    {
        var idx = _customers.FindIndex(c => c.Id == customer.Id && c.TenantId == customer.TenantId);
        if (idx >= 0)
        {
            _customers[idx] = customer;
        }
        else
        {
            _customers.Add(customer);
        }

        if (ledgerEntry != null)
        {
            _ledgerEntries.Add(ledgerEntry);
        }
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<LoyaltyLedgerEntry>> GetCustomerLedgerAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        var entries = _ledgerEntries
            .Where(e => e.TenantId == tenantId && e.CustomerId == customerId)
            .OrderByDescending(e => e.CreatedAtUtc)
            .ToList();
        return Task.FromResult<IReadOnlyList<LoyaltyLedgerEntry>>(entries);
    }
}
