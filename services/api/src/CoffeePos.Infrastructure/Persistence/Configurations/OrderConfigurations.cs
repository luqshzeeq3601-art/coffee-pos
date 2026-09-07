using CoffeePos.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.TenantId).IsRequired();
        builder.Property(o => o.OutletId).IsRequired();
        builder.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(o => o.Status).HasConversion<int>().IsRequired();
        builder.Property(o => o.DiningOption).HasConversion<int>().IsRequired();
        builder.Property(o => o.TableOrCustomer).HasMaxLength(150);
        builder.Property(o => o.CashierId);
        builder.Property(o => o.CashierName).HasMaxLength(150);
        builder.Property(o => o.DiscountTotal).HasPrecision(19, 4).IsRequired();
        builder.Property(o => o.TaxPercent).HasPrecision(19, 4).IsRequired();
        builder.Property(o => o.Notes);
        builder.Property(o => o.CancelReason);
        builder.Property(o => o.CreatedAtUtc).IsRequired();
        builder.Property(o => o.UpdatedAtUtc).IsRequired();

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => new { o.TenantId, o.OutletId, o.Status });
    }
}

public sealed class OrderLineItemConfiguration : IEntityTypeConfiguration<OrderLineItem>
{
    public void Configure(EntityTypeBuilder<OrderLineItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.TenantId).IsRequired();
        builder.Property(i => i.OrderId).IsRequired();
        builder.Property(i => i.ProductId).IsRequired();
        builder.Property(i => i.VariantId);
        builder.Property(i => i.Name).HasMaxLength(200).IsRequired();
        builder.Property(i => i.UnitPrice).HasPrecision(19, 4).IsRequired();
        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.Notes);

        builder.HasMany(i => i.Modifiers)
            .WithOne()
            .HasForeignKey(m => m.OrderLineItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => new { i.TenantId, i.OrderId });
    }
}

public sealed class OrderLineModifierConfiguration : IEntityTypeConfiguration<OrderLineModifier>
{
    public void Configure(EntityTypeBuilder<OrderLineModifier> builder)
    {
        builder.ToTable("order_item_modifiers");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.TenantId).IsRequired();
        builder.Property(m => m.OrderLineItemId).IsRequired();
        builder.Property(m => m.ModifierId).IsRequired();
        builder.Property(m => m.Name).HasMaxLength(150).IsRequired();
        builder.Property(m => m.PriceDelta).HasPrecision(19, 4).IsRequired();

        builder.HasIndex(m => new { m.TenantId, m.OrderLineItemId });
    }
}
