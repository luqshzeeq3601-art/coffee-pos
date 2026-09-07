using CoffeePos.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class StockTransferConfiguration : IEntityTypeConfiguration<StockTransfer>
{
    public void Configure(EntityTypeBuilder<StockTransfer> builder)
    {
        builder.ToTable("stock_transfers");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TenantId).IsRequired();
        builder.Property(t => t.TransferNumber).HasMaxLength(64).IsRequired();
        builder.Property(t => t.SourceOutletId).IsRequired();
        builder.Property(t => t.DestinationOutletId).IsRequired();
        builder.Property(t => t.Status).HasConversion<int>().IsRequired();
        builder.Property(t => t.DispatchedById);
        builder.Property(t => t.DispatchedByName).HasMaxLength(150);
        builder.Property(t => t.DispatchedAtUtc);
        builder.Property(t => t.ReceivedById);
        builder.Property(t => t.ReceivedByName).HasMaxLength(150);
        builder.Property(t => t.ReceivedAtUtc);
        builder.Property(t => t.Notes);
        builder.Property(t => t.CreatedAtUtc).IsRequired();

        builder.HasMany(t => t.Items)
            .WithOne()
            .HasForeignKey(i => i.TransferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => new { t.TenantId, t.TransferNumber });
    }
}

public sealed class StockTransferItemConfiguration : IEntityTypeConfiguration<StockTransferItem>
{
    public void Configure(EntityTypeBuilder<StockTransferItem> builder)
    {
        builder.ToTable("stock_transfer_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.TransferId).IsRequired();
        builder.Property(i => i.StockItemId).IsRequired();
        builder.Property(i => i.Quantity).HasPrecision(19, 4).IsRequired();
    }
}
