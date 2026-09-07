using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Saas;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemorySubscriptionStore : ISubscriptionStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly List<TenantSubscription> _subscriptions = new();
    private readonly List<TenantBranding> _brandings = new();

    public InMemorySubscriptionStore()
    {
        SeedData();
    }

    private void SeedData()
    {
        var sub = new TenantSubscription(
            Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccc01"),
            DemoTenantId,
            SubscriptionTier.Growth,
            SubscriptionStatus.Active);
        _subscriptions.Add(sub);

        var branding = new TenantBranding(
            Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddd01"),
            DemoTenantId,
            "Artisan Roast Co.",
            "#005D52");
        branding.Update(
            "Artisan Roast Co.",
            "https://artisanroast.my/assets/logo.png",
            "#005D52",
            "Artisan Specialty Coffee & Fine Pastries",
            "Terima Kasih! Follow us on Instagram @artisanroastkl",
            "W10-1808-32000123",
            true,
            "ArtisanRoast-Guest",
            "espresso2026");
        _brandings.Add(branding);
    }

    public Task<TenantSubscription?> GetSubscriptionAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var sub = _subscriptions.FirstOrDefault(s => s.TenantId == tenantId);
        return Task.FromResult(sub);
    }

    public Task SaveSubscriptionAsync(TenantSubscription subscription, CancellationToken cancellationToken = default)
    {
        var idx = _subscriptions.FindIndex(s => s.TenantId == subscription.TenantId);
        if (idx >= 0)
        {
            _subscriptions[idx] = subscription;
        }
        else
        {
            _subscriptions.Add(subscription);
        }
        return Task.CompletedTask;
    }

    public Task<TenantBranding?> GetBrandingAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var b = _brandings.FirstOrDefault(br => br.TenantId == tenantId);
        return Task.FromResult(b);
    }

    public Task SaveBrandingAsync(TenantBranding branding, CancellationToken cancellationToken = default)
    {
        var idx = _brandings.FindIndex(b => b.TenantId == branding.TenantId);
        if (idx >= 0)
        {
            _brandings[idx] = branding;
        }
        else
        {
            _brandings.Add(branding);
        }
        return Task.CompletedTask;
    }
}
