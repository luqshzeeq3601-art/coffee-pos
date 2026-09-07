using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Saas;

namespace CoffeePos.Application.Interfaces;

public interface ISubscriptionStore
{
    Task<TenantSubscription?> GetSubscriptionAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task SaveSubscriptionAsync(TenantSubscription subscription, CancellationToken cancellationToken = default);
    Task<TenantBranding?> GetBrandingAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task SaveBrandingAsync(TenantBranding branding, CancellationToken cancellationToken = default);
}

public interface ISubscriptionService
{
    Task<TenantSubscriptionDto> GetSubscriptionAsync(CancellationToken cancellationToken = default);
    Task<EntitlementsDto> GetEntitlementsAsync(CancellationToken cancellationToken = default);
    Task<TenantSubscriptionDto> UpgradeSubscriptionAsync(UpgradeSubscriptionRequest request, CancellationToken cancellationToken = default);
    Task<TenantBrandingDto> GetBrandingAsync(CancellationToken cancellationToken = default);
    Task<TenantBrandingDto> UpdateBrandingAsync(UpdateBrandingRequest request, CancellationToken cancellationToken = default);
}
