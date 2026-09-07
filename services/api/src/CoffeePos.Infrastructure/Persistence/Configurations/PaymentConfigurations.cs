using CoffeePos.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class SalesTransactionConfiguration : IEntityTypeConfiguration<SalesTransaction>
{
    public void Configure(EntityTypeBuilder<SalesTransaction> builder)
    {
        builder.ToTable("sales_transactions");
        builder.HasKey(st => st.Id);

        builder.Property(st => st.TenantId).IsRequired();
        builder.Property(st => st.OutletId).IsRequired();
        builder.Property(st => st.OrderId).IsRequired();
        builder.Property(st => st.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(st => st.ReceiptNumber).HasMaxLength(50).IsRequired();
        builder.Property(st => st.CashierId).IsRequired();
        builder.Property(st => st.CashierName).HasMaxLength(150).IsRequired();
        builder.Property(st => st.Subtotal).HasPrecision(19, 4).IsRequired();
        builder.Property(st => st.DiscountTotal).HasPrecision(19, 4).IsRequired();
        builder.Property(st => st.TaxTotal).HasPrecision(19, 4).IsRequired();
        builder.Property(st => st.GrandTotal).HasPrecision(19, 4).IsRequired();
        builder.Property(st => st.PaidAmount).HasPrecision(19, 4).IsRequired();
        builder.Property(st => st.ChangeAmount).HasPrecision(19, 4).IsRequired();
        builder.Property(st => st.CreatedAtUtc).IsRequired();

        builder.HasMany(st => st.Payments)
            .WithOne()
            .HasForeignKey(p => p.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(st => st.Refunds)
            .WithOne()
            .HasForeignKey(r => r.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(st => new { st.TenantId, st.ReceiptNumber }).IsUnique();
        builder.HasIndex(st => new { st.TenantId, st.OutletId, st.CreatedAtUtc });
    }
}

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.TenantId).IsRequired();
        builder.Property(p => p.TransactionId).IsRequired();
        builder.Property(p => p.PaymentMethod).HasConversion<int>().IsRequired();
        builder.Property(p => p.Amount).HasPrecision(19, 4).IsRequired();
        builder.Property(p => p.TenderedAmount).HasPrecision(19, 4);
        builder.Property(p => p.ChangeAmount).HasPrecision(19, 4);
        builder.Property(p => p.ReferenceCode).HasMaxLength(100);
        builder.Property(p => p.Status).HasConversion<int>().IsRequired();
        builder.Property(p => p.CreatedAtUtc).IsRequired();

        builder.HasIndex(p => new { p.TenantId, p.TransactionId });
    }
}

public sealed class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("refunds");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.TenantId).IsRequired();
        builder.Property(r => r.TransactionId).IsRequired();
        builder.Property(r => r.Amount).HasPrecision(19, 4).IsRequired();
        builder.Property(r => r.Reason).IsRequired();
        builder.Property(r => r.ApprovedByUserId).IsRequired();
        builder.Property(r => r.ApprovedByUserName).HasMaxLength(150).IsRequired();
        builder.Property(r => r.CreatedAtUtc).IsRequired();

        builder.HasIndex(r => new { r.TenantId, r.TransactionId });
    }
}
