using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Saas;

namespace CoffeePos.Infrastructure.Services;

public sealed class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionStore _subscriptionStore;
    private readonly ITenantContext _tenantContext;

    public SubscriptionService(
        ISubscriptionStore subscriptionStore,
        ITenantContext tenantContext)
    {
        _subscriptionStore = subscriptionStore;
        _tenantContext = tenantContext;
    }

    private Guid RequireTenantId()
    {
        return _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
    }

    public async Task<TenantSubscriptionDto> GetSubscriptionAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var sub = await _subscriptionStore.GetSubscriptionAsync(tenantId, cancellationToken);
        if (sub == null)
        {
            sub = new TenantSubscription(Guid.NewGuid(), tenantId, SubscriptionTier.Growth, SubscriptionStatus.Active);
            await _subscriptionStore.SaveSubscriptionAsync(sub, cancellationToken);
        }

        return MapToDto(sub);
    }

    public async Task<EntitlementsDto> GetEntitlementsAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var sub = await _subscriptionStore.GetSubscriptionAsync(tenantId, cancellationToken);
        var tier = sub?.Tier ?? SubscriptionTier.Growth;

        return new EntitlementsDto(
            CanUseMyInvois: tier != SubscriptionTier.Starter,
            CanUseMultiOutlet: tier != SubscriptionTier.Starter,
            CanUseTimecards: tier != SubscriptionTier.Starter,
            CanUseAdvancedInventory: tier == SubscriptionTier.Enterprise,
            CanUseCustomBranding: true,
            MaxOutlets: sub?.MaxOutlets ?? 3,
            MaxRegisters: sub?.MaxRegisters ?? 6);
    }

    public async Task<TenantSubscriptionDto> UpgradeSubscriptionAsync(UpgradeSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var sub = await _subscriptionStore.GetSubscriptionAsync(tenantId, cancellationToken)
            ?? new TenantSubscription(Guid.NewGuid(), tenantId, SubscriptionTier.Starter);

        if (Enum.TryParse<SubscriptionTier>(request.Tier, true, out var newTier))
        {
            sub.UpgradeTier(newTier);
            await _subscriptionStore.SaveSubscriptionAsync(sub, cancellationToken);
        }

        return MapToDto(sub);
    }

    public async Task<TenantBrandingDto> GetBrandingAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var b = await _subscriptionStore.GetBrandingAsync(tenantId, cancellationToken);
        if (b == null)
        {
            b = new TenantBranding(Guid.NewGuid(), tenantId, "Artisan Roast Co.");
            await _subscriptionStore.SaveBrandingAsync(b, cancellationToken);
        }

        return new TenantBrandingDto(
            b.TenantId,
            b.BrandName,
            b.LogoUrl,
            b.PrimaryColorHex,
            b.HeaderText,
            b.FooterText,
            b.TaxRegistrationNumber,
            b.ShowWifiInfo,
            b.WifiSsid,
            b.WifiPassword);
    }

    public async Task<TenantBrandingDto> UpdateBrandingAsync(UpdateBrandingRequest request, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var b = await _subscriptionStore.GetBrandingAsync(tenantId, cancellationToken)
            ?? new TenantBranding(Guid.NewGuid(), tenantId, request.BrandName);

        b.Update(
            request.BrandName,
            request.LogoUrl,
            request.PrimaryColorHex,
            request.HeaderText,
            request.FooterText,
            request.TaxRegistrationNumber,
            request.ShowWifiInfo,
            request.WifiSsid,
            request.WifiPassword);

        await _subscriptionStore.SaveBrandingAsync(b, cancellationToken);
        return new TenantBrandingDto(
            b.TenantId,
            b.BrandName,
            b.LogoUrl,
            b.PrimaryColorHex,
            b.HeaderText,
            b.FooterText,
            b.TaxRegistrationNumber,
            b.ShowWifiInfo,
            b.WifiSsid,
            b.WifiPassword);
    }

    private static TenantSubscriptionDto MapToDto(TenantSubscription s)
    {
        var features = new List<string> { "Cashier POS", "ESC/POS Printing", "Offline Synchronization", "Digital DuitNow QR" };
        if (s.Tier != SubscriptionTier.Starter)
        {
            features.AddRange(new[] { "Multi-Outlet Management", "Staff Timecards", "MyInvois e-Invoicing Compliance", "Customer Loyalty Engine" });
        }
        if (s.Tier == SubscriptionTier.Enterprise)
        {
            features.AddRange(new[] { "Multi-Warehouse Stock Dispatch", "Custom API Webhooks", "Dedicated SLA Support" });
        }

        return new TenantSubscriptionDto(
            s.Id,
            s.TenantId,
            s.Tier.ToString(),
            s.Status.ToString(),
            s.MonthlyPriceMyr,
            s.MaxOutlets,
            s.MaxRegisters,
            features,
            s.TrialEndUtc,
            s.CurrentPeriodEndUtc);
    }
}
