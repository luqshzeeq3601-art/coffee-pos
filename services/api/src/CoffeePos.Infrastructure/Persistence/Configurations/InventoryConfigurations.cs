using CoffeePos.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.ToTable("stock_items");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TenantId).IsRequired();
        builder.Property(s => s.OutletId).IsRequired();
        builder.Property(s => s.Sku).HasMaxLength(64).IsRequired();
        builder.Property(s => s.Name).HasMaxLength(150).IsRequired();
        builder.Property(s => s.Category).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Unit).HasConversion<int>().IsRequired();
        builder.Property(s => s.CurrentStock).HasPrecision(19, 4).IsRequired();
        builder.Property(s => s.ReorderThreshold).HasPrecision(19, 4).IsRequired();
        builder.Property(s => s.CostPerUnit).HasPrecision(19, 4).IsRequired();
        builder.Property(s => s.LastUpdatedUtc).IsRequired();

        builder.HasIndex(s => new { s.TenantId, s.OutletId });
        builder.HasIndex(s => new { s.TenantId, s.Sku }).IsUnique();
    }
}

public sealed class StockLedgerEntryConfiguration : IEntityTypeConfiguration<StockLedgerEntry>
{
    public void Configure(EntityTypeBuilder<StockLedgerEntry> builder)
    {
        builder.ToTable("stock_ledger_entries");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.TenantId).IsRequired();
        builder.Property(e => e.StockItemId).IsRequired();
        builder.Property(e => e.Type).HasConversion<int>().IsRequired();
        builder.Property(e => e.QuantityDelta).HasPrecision(19, 4).IsRequired();
        builder.Property(e => e.BalanceAfter).HasPrecision(19, 4).IsRequired();
        builder.Property(e => e.Reason).IsRequired();
        builder.Property(e => e.ReferenceId).HasMaxLength(100);
        builder.Property(e => e.PerformedById).IsRequired();
        builder.Property(e => e.PerformedByName).HasMaxLength(150).IsRequired();
        builder.Property(e => e.CreatedAtUtc).IsRequired();

        builder.HasIndex(e => new { e.TenantId, e.StockItemId });
    }
}

public sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("recipes");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.TenantId).IsRequired();
        builder.Property(r => r.ProductId).IsRequired();
        builder.Property(r => r.VariantId);
        builder.Property(r => r.ProductName).HasMaxLength(150).IsRequired();
        builder.Property(r => r.VariantName).HasMaxLength(150);

        builder.HasMany(r => r.Items)
            .WithOne()
            .HasForeignKey(i => i.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => new { r.TenantId, r.ProductId, r.VariantId });
    }
}

public sealed class RecipeItemConfiguration : IEntityTypeConfiguration<RecipeItem>
{
    public void Configure(EntityTypeBuilder<RecipeItem> builder)
    {
        builder.ToTable("recipe_items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.RecipeId).IsRequired();
        builder.Property(i => i.StockItemId).IsRequired();
        builder.Property(i => i.StockItemName).HasMaxLength(150).IsRequired();
        builder.Property(i => i.Unit).HasConversion<int>().IsRequired();
        builder.Property(i => i.QuantityRequired).HasPrecision(19, 4).IsRequired();
    }
}
