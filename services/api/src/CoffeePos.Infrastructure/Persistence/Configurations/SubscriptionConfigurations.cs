using CoffeePos.Domain.Saas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class TenantSubscriptionConfiguration : IEntityTypeConfiguration<TenantSubscription>
{
    public void Configure(EntityTypeBuilder<TenantSubscription> builder)
    {
        builder.ToTable("tenant_subscriptions");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TenantId).IsRequired();
        builder.Property(s => s.Tier).HasConversion<int>().IsRequired();
        builder.Property(s => s.Status).HasConversion<int>().IsRequired();
        builder.Property(s => s.MonthlyPriceMyr).HasPrecision(10, 2).IsRequired();
        builder.Property(s => s.MaxOutlets).IsRequired();
        builder.Property(s => s.MaxRegisters).IsRequired();
        builder.Property(s => s.TrialEndUtc);
        builder.Property(s => s.CurrentPeriodEndUtc).IsRequired();

        builder.HasIndex(s => s.TenantId).IsUnique();
    }
}

public sealed class TenantBrandingConfiguration : IEntityTypeConfiguration<TenantBranding>
{
    public void Configure(EntityTypeBuilder<TenantBranding> builder)
    {
        builder.ToTable("tenant_brandings");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.TenantId).IsRequired();
        builder.Property(b => b.BrandName).HasMaxLength(150).IsRequired();
        builder.Property(b => b.LogoUrl);
        builder.Property(b => b.PrimaryColorHex).HasMaxLength(16).IsRequired();
        builder.Property(b => b.HeaderText);
        builder.Property(b => b.FooterText);
        builder.Property(b => b.TaxRegistrationNumber).HasMaxLength(64);
        builder.Property(b => b.ShowWifiInfo).IsRequired();
        builder.Property(b => b.WifiSsid).HasMaxLength(100);
        builder.Property(b => b.WifiPassword).HasMaxLength(100);

        builder.HasIndex(b => b.TenantId).IsUnique();
    }
}
