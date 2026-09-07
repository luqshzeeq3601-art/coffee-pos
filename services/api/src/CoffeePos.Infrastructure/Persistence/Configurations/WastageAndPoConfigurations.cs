using CoffeePos.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class StockWastageEntryConfiguration : IEntityTypeConfiguration<StockWastageEntry>
{
    public void Configure(EntityTypeBuilder<StockWastageEntry> builder)
    {
        builder.ToTable("stock_wastage_entries");
        builder.HasKey(w => w.Id);

        builder.Property(w => w.TenantId).IsRequired();
        builder.Property(w => w.OutletId).IsRequired();
        builder.Property(w => w.StockItemId).IsRequired();
        builder.Property(w => w.StockItemName).HasMaxLength(150).IsRequired();
        builder.Property(w => w.QuantityWasted).HasPrecision(19, 4).IsRequired();
        builder.Property(w => w.Unit).HasConversion<int>().IsRequired();
        builder.Property(w => w.Reason).HasConversion<int>().IsRequired();
        builder.Property(w => w.CostImpact).HasPrecision(19, 4).IsRequired();
        builder.Property(w => w.Notes);
        builder.Property(w => w.LoggedById).IsRequired();
        builder.Property(w => w.LoggedByName).HasMaxLength(150).IsRequired();
        builder.Property(w => w.CreatedAtUtc).IsRequired();

        builder.HasIndex(w => new { w.TenantId, w.OutletId });
    }
}

public sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("purchase_orders");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.TenantId).IsRequired();
        builder.Property(p => p.OutletId).IsRequired();
        builder.Property(p => p.PoNumber).HasMaxLength(64).IsRequired();
        builder.Property(p => p.SupplierName).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Status).HasConversion<int>().IsRequired();
        builder.Property(p => p.Notes);
        builder.Property(p => p.CreatedAtUtc).IsRequired();
        builder.Property(p => p.ReceivedAtUtc);

        builder.HasMany(p => p.Items)
            .WithOne()
            .HasForeignKey(i => i.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.TenantId, p.OutletId, p.Status });
        builder.HasIndex(p => new { p.TenantId, p.PoNumber }).IsUnique();
    }
}

public sealed class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.ToTable("purchase_order_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.PurchaseOrderId).IsRequired();
        builder.Property(i => i.StockItemId).IsRequired();
        builder.Property(i => i.StockItemName).HasMaxLength(150).IsRequired();
        builder.Property(i => i.Unit).HasConversion<int>().IsRequired();
        builder.Property(i => i.QuantityOrdered).HasPrecision(19, 4).IsRequired();
        builder.Property(i => i.QuantityReceived).HasPrecision(19, 4).IsRequired();
        builder.Property(i => i.UnitCost).HasPrecision(19, 4).IsRequired();
    }
}
