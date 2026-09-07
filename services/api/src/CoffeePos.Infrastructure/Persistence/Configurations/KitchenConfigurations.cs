using CoffeePos.Domain.Kitchen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class KitchenChitConfiguration : IEntityTypeConfiguration<KitchenChit>
{
    public void Configure(EntityTypeBuilder<KitchenChit> builder)
    {
        builder.ToTable("kitchen_chits");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.TenantId).IsRequired();
        builder.Property(c => c.OutletId).IsRequired();
        builder.Property(c => c.OrderId).IsRequired();
        builder.Property(c => c.OrderNumber).HasMaxLength(64).IsRequired();
        builder.Property(c => c.DiningOption).HasConversion<int>().IsRequired();
        builder.Property(c => c.CustomerName).HasMaxLength(150);
        builder.Property(c => c.TableNumber).HasMaxLength(50);
        builder.Property(c => c.Station).HasConversion<int>().IsRequired();
        builder.Property(c => c.Status).HasConversion<int>().IsRequired();
        builder.Property(c => c.CreatedAtUtc).IsRequired();
        builder.Property(c => c.StartedAtUtc);
        builder.Property(c => c.CompletedAtUtc);

        builder.HasMany(c => c.Items)
            .WithOne()
            .HasForeignKey(i => i.KitchenChitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => new { c.TenantId, c.OutletId, c.Station, c.Status });
    }
}

public sealed class KitchenChitItemConfiguration : IEntityTypeConfiguration<KitchenChitItem>
{
    public void Configure(EntityTypeBuilder<KitchenChitItem> builder)
    {
        builder.ToTable("kitchen_chit_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.KitchenChitId).IsRequired();
        builder.Property(i => i.ProductId).IsRequired();
        builder.Property(i => i.ProductName).HasMaxLength(150).IsRequired();
        builder.Property(i => i.VariantName).HasMaxLength(150);
        builder.Property(i => i.ModifiersSummary);
        builder.Property(i => i.Notes);
        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.IsPrepared).IsRequired();
    }
}
