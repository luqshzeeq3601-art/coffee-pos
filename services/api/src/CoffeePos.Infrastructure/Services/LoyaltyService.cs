using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Loyalty;

namespace CoffeePos.Infrastructure.Services;

public sealed class LoyaltyService : ILoyaltyService
{
    private readonly ILoyaltyStore _loyaltyStore;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentUser _currentUser;

    public LoyaltyService(
        ILoyaltyStore loyaltyStore,
        ITenantContext tenantContext,
        ICurrentUser currentUser)
    {
        _loyaltyStore = loyaltyStore;
        _tenantContext = tenantContext;
        _currentUser = currentUser;
    }

    private (Guid TenantId, Guid UserId, string UserName) RequireContext()
    {
        var tenantId = _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
        var userId = _currentUser.UserId ?? Guid.Parse("33333333-3333-3333-3333-333333333331");
        var userName = _currentUser.Email ?? "Ahmad Cashier";
        return (tenantId, userId, userName);
    }

    public async Task<IReadOnlyList<CustomerDto>> SearchCustomersAsync(string query, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var list = await _loyaltyStore.SearchCustomersAsync(tenantId, query, cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var customer = await _loyaltyStore.GetCustomerByIdAsync(tenantId, id, cancellationToken);
        return customer != null ? MapToDto(customer) : null;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var existing = await _loyaltyStore.GetCustomerByPhoneAsync(tenantId, request.PhoneNumber, cancellationToken);
        if (existing != null)
        {
            throw new InvalidOperationException($"Customer with phone number {request.PhoneNumber} already exists.");
        }

        var customer = new Customer(Guid.NewGuid(), tenantId, request.Name, request.PhoneNumber, request.Email);
        await _loyaltyStore.SaveCustomerAsync(customer, null, cancellationToken);
        return MapToDto(customer);
    }

    public async Task<CustomerDto> EarnPointsAsync(Guid customerId, EarnPointsRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, userId, userName) = RequireContext();
        var customer = await _loyaltyStore.GetCustomerByIdAsync(tenantId, customerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        var entry = customer.EarnPoints(request.AmountSpent, request.OrderNumber, userId, userName);
        await _loyaltyStore.SaveCustomerAsync(customer, entry, cancellationToken);
        return MapToDto(customer);
    }

    public async Task<CustomerDto> RedeemPointsAsync(Guid customerId, RedeemPointsRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, userId, userName) = RequireContext();
        var customer = await _loyaltyStore.GetCustomerByIdAsync(tenantId, customerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        var entry = customer.RedeemPoints(request.PointsToRedeem, request.OrderNumber, userId, userName);
        await _loyaltyStore.SaveCustomerAsync(customer, entry, cancellationToken);
        return MapToDto(customer);
    }

    public async Task<IReadOnlyList<LoyaltyLedgerEntryDto>> GetCustomerLedgerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var (tenantId, _, _) = RequireContext();
        var list = await _loyaltyStore.GetCustomerLedgerAsync(tenantId, customerId, cancellationToken);
        return list.Select(e => new LoyaltyLedgerEntryDto(
            e.Id,
            e.TenantId,
            e.CustomerId,
            e.Type.ToString(),
            e.PointsDelta,
            e.BalanceAfter,
            e.Reason,
            e.OrderNumber,
            e.PerformedByName,
            e.CreatedAtUtc)).ToList();
    }

    private static CustomerDto MapToDto(Customer c)
    {
        return new CustomerDto(
            c.Id,
            c.TenantId,
            c.Name,
            c.PhoneNumber,
            c.Email,
            c.Tier.ToString(),
            c.PointsBalance,
            c.TotalSpent,
            c.VisitCount,
            c.JoinedAtUtc,
            c.LastVisitUtc);
    }
}
