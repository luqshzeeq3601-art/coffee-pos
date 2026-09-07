using CoffeePos.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.TenantId).IsRequired();
        builder.Property(c => c.Name).HasMaxLength(150).IsRequired();
        builder.Property(c => c.Code).HasMaxLength(50).IsRequired();
        builder.Property(c => c.SortOrder).IsRequired();
        builder.Property(c => c.IsActive).IsRequired();
        builder.Property(c => c.CreatedAtUtc).IsRequired();

        builder.HasIndex(c => new { c.TenantId, c.Code }).IsUnique();
    }
}

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.TenantId).IsRequired();
        builder.Property(p => p.CategoryId).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description);
        builder.Property(p => p.BasePrice).HasPrecision(19, 4).IsRequired();
        builder.Property(p => p.IsTaxInclusive).IsRequired();
        builder.Property(p => p.TaxRateId);
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.CreatedAtUtc).IsRequired();

        builder.HasMany(p => p.Variants)
            .WithOne()
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ModifierGroups)
            .WithOne()
            .HasForeignKey("ProductId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.TenantId, p.CategoryId });
    }
}

public sealed class VariantConfiguration : IEntityTypeConfiguration<Variant>
{
    public void Configure(EntityTypeBuilder<Variant> builder)
    {
        builder.ToTable("product_variants");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.TenantId).IsRequired();
        builder.Property(v => v.ProductId).IsRequired();
        builder.Property(v => v.Name).HasMaxLength(150).IsRequired();
        builder.Property(v => v.Sku).HasMaxLength(100);
        builder.Property(v => v.Barcode).HasMaxLength(100);
        builder.Property(v => v.Price).HasPrecision(19, 4).IsRequired();
        builder.Property(v => v.CostPrice).HasPrecision(19, 4);
        builder.Property(v => v.SortOrder).IsRequired();

        builder.HasIndex(v => new { v.TenantId, v.ProductId });
    }
}

public sealed class ModifierGroupConfiguration : IEntityTypeConfiguration<ModifierGroup>
{
    public void Configure(EntityTypeBuilder<ModifierGroup> builder)
    {
        builder.ToTable("modifier_groups");
        builder.HasKey(mg => mg.Id);

        builder.Property(mg => mg.TenantId).IsRequired();
        builder.Property(mg => mg.Name).HasMaxLength(150).IsRequired();
        builder.Property(mg => mg.MinSelections).IsRequired();
        builder.Property(mg => mg.MaxSelections).IsRequired();

        builder.HasMany(mg => mg.Modifiers)
            .WithOne()
            .HasForeignKey(m => m.ModifierGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(mg => mg.TenantId);
    }
}

public sealed class ModifierConfiguration : IEntityTypeConfiguration<Modifier>
{
    public void Configure(EntityTypeBuilder<Modifier> builder)
    {
        builder.ToTable("modifiers");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.TenantId).IsRequired();
        builder.Property(m => m.ModifierGroupId).IsRequired();
        builder.Property(m => m.Name).HasMaxLength(150).IsRequired();
        builder.Property(m => m.PriceDelta).HasPrecision(19, 4).IsRequired();
        builder.Property(m => m.IsDefault).IsRequired();
        builder.Property(m => m.SortOrder).IsRequired();

        builder.HasIndex(m => new { m.TenantId, m.ModifierGroupId });
    }
}

public sealed class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
{
    public void Configure(EntityTypeBuilder<TaxRate> builder)
    {
        builder.ToTable("tax_rates");
        builder.HasKey(tr => tr.Id);

        builder.Property(tr => tr.TenantId).IsRequired();
        builder.Property(tr => tr.Name).HasMaxLength(100).IsRequired();
        builder.Property(tr => tr.RatePercent).HasPrecision(19, 4).IsRequired();
        builder.Property(tr => tr.Code).HasMaxLength(20).IsRequired();
        builder.Property(tr => tr.IsDefault).IsRequired();
        builder.Property(tr => tr.IsActive).IsRequired();

        builder.HasIndex(tr => tr.TenantId);
    }
}

public sealed class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("discounts");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.TenantId).IsRequired();
        builder.Property(d => d.Name).HasMaxLength(100).IsRequired();
        builder.Property(d => d.DiscountType).HasConversion<int>().IsRequired();
        builder.Property(d => d.Value).HasPrecision(19, 4).IsRequired();
        builder.Property(d => d.RequiresManagerApproval).IsRequired();
        builder.Property(d => d.IsActive).IsRequired();

        builder.HasIndex(d => d.TenantId);
    }
}
