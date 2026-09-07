using CoffeePos.Domain.Loyalty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.TenantId).IsRequired();
        builder.Property(c => c.Name).HasMaxLength(150).IsRequired();
        builder.Property(c => c.PhoneNumber).HasMaxLength(32).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(150);
        builder.Property(c => c.Tier).HasConversion<int>().IsRequired();
        builder.Property(c => c.PointsBalance).IsRequired();
        builder.Property(c => c.TotalSpent).HasPrecision(19, 4).IsRequired();
        builder.Property(c => c.VisitCount).IsRequired();
        builder.Property(c => c.JoinedAtUtc).IsRequired();
        builder.Property(c => c.LastVisitUtc).IsRequired();

        builder.HasIndex(c => new { c.TenantId, c.PhoneNumber });
    }
}

public sealed class LoyaltyLedgerEntryConfiguration : IEntityTypeConfiguration<LoyaltyLedgerEntry>
{
    public void Configure(EntityTypeBuilder<LoyaltyLedgerEntry> builder)
    {
        builder.ToTable("loyalty_ledger_entries");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.TenantId).IsRequired();
        builder.Property(e => e.CustomerId).IsRequired();
        builder.Property(e => e.Type).HasConversion<int>().IsRequired();
        builder.Property(e => e.PointsDelta).IsRequired();
        builder.Property(e => e.BalanceAfter).IsRequired();
        builder.Property(e => e.Reason).IsRequired();
        builder.Property(e => e.OrderNumber).HasMaxLength(64);
        builder.Property(e => e.PerformedById).IsRequired();
        builder.Property(e => e.PerformedByName).HasMaxLength(150).IsRequired();
        builder.Property(e => e.CreatedAtUtc).IsRequired();

        builder.HasIndex(e => new { e.TenantId, e.CustomerId });
    }
}
