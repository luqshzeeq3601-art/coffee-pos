using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Loyalty;

namespace CoffeePos.Application.Interfaces;

public interface ILoyaltyStore
{
    Task<IReadOnlyList<Customer>> SearchCustomersAsync(Guid tenantId, string query, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByIdAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByPhoneAsync(Guid tenantId, string phoneNumber, CancellationToken cancellationToken = default);
    Task SaveCustomerAsync(Customer customer, LoyaltyLedgerEntry? ledgerEntry = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LoyaltyLedgerEntry>> GetCustomerLedgerAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default);
}

public interface ILoyaltyService
{
    Task<IReadOnlyList<CustomerDto>> SearchCustomersAsync(string query, CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<CustomerDto> EarnPointsAsync(Guid customerId, EarnPointsRequest request, CancellationToken cancellationToken = default);
    Task<CustomerDto> RedeemPointsAsync(Guid customerId, RedeemPointsRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LoyaltyLedgerEntryDto>> GetCustomerLedgerAsync(Guid customerId, CancellationToken cancellationToken = default);
}
