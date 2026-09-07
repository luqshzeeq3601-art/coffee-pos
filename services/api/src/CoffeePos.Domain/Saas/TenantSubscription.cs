namespace CoffeePos.Domain.Saas;

public enum SubscriptionTier
{
    Starter = 1,
    Growth = 2,
    Enterprise = 3
}

public enum SubscriptionStatus
{
    Trialing = 1,
    Active = 2,
    PastDue = 3,
    Canceled = 4
}

public sealed class TenantSubscription
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public SubscriptionTier Tier { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public decimal MonthlyPriceMyr { get; private set; }
    public int MaxOutlets { get; private set; }
    public int MaxRegisters { get; private set; }
    public DateTime? TrialEndUtc { get; private set; }
    public DateTime CurrentPeriodEndUtc { get; private set; }

    public TenantSubscription(
        Guid id,
        Guid tenantId,
        SubscriptionTier tier,
        SubscriptionStatus status = SubscriptionStatus.Active)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));

        Id = id;
        TenantId = tenantId;
        Status = status;
        SetTierLimits(tier);

        if (status == SubscriptionStatus.Trialing)
        {
            TrialEndUtc = DateTime.UtcNow.AddDays(14);
            CurrentPeriodEndUtc = DateTime.UtcNow.AddDays(14);
        }
        else
        {
            CurrentPeriodEndUtc = DateTime.UtcNow.AddMonths(1);
        }
    }

    public void UpgradeTier(SubscriptionTier newTier)
    {
        SetTierLimits(newTier);
        Status = SubscriptionStatus.Active;
        CurrentPeriodEndUtc = DateTime.UtcNow.AddMonths(1);
    }

    private void SetTierLimits(SubscriptionTier tier)
    {
        Tier = tier;
        switch (tier)
        {
            case SubscriptionTier.Starter:
                MonthlyPriceMyr = 79.00m;
                MaxOutlets = 1;
                MaxRegisters = 1;
                break;
            case SubscriptionTier.Growth:
                MonthlyPriceMyr = 199.00m;
                MaxOutlets = 3;
                MaxRegisters = 6;
                break;
            case SubscriptionTier.Enterprise:
                MonthlyPriceMyr = 499.00m;
                MaxOutlets = 99;
                MaxRegisters = 99;
                break;
        }
    }
}
