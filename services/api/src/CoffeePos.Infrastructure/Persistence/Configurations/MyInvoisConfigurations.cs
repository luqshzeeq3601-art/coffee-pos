using CoffeePos.Domain.MyInvois;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class MyInvoisDocumentConfiguration : IEntityTypeConfiguration<MyInvoisDocument>
{
    public void Configure(EntityTypeBuilder<MyInvoisDocument> builder)
    {
        builder.ToTable("myinvois_documents");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.TenantId).IsRequired();
        builder.Property(d => d.SalesTransactionId).IsRequired();
        builder.Property(d => d.InvoiceNumber).HasMaxLength(64).IsRequired();
        builder.Property(d => d.Uuid).HasMaxLength(128);
        builder.Property(d => d.LongId).HasMaxLength(256);
        builder.Property(d => d.Status).HasConversion<int>().IsRequired();

        builder.OwnsOne(d => d.Buyer, b =>
        {
            b.Property(p => p.Tin).HasColumnName("buyer_tin").HasMaxLength(32).IsRequired();
            b.Property(p => p.IdType).HasColumnName("buyer_id_type").HasConversion<int>().IsRequired();
            b.Property(p => p.IdValue).HasColumnName("buyer_id_value").HasMaxLength(64).IsRequired();
            b.Property(p => p.Name).HasColumnName("buyer_name").HasMaxLength(150).IsRequired();
            b.Property(p => p.PhoneNumber).HasColumnName("buyer_phone").HasMaxLength(32);
            b.Property(p => p.Email).HasColumnName("buyer_email").HasMaxLength(150);
            b.Property(p => p.Address).HasColumnName("buyer_address");
        });

        builder.Property(d => d.TotalExcludingTax).HasPrecision(19, 4).IsRequired();
        builder.Property(d => d.TotalTaxAmount).HasPrecision(19, 4).IsRequired();
        builder.Property(d => d.TotalPayable).HasPrecision(19, 4).IsRequired();
        builder.Property(d => d.QrCodeUrl);
        builder.Property(d => d.ValidationErrorsJson).HasColumnName("validation_errors");
        builder.Property(d => d.SubmittedAtUtc);
        builder.Property(d => d.ValidatedAtUtc);
        builder.Property(d => d.CreatedAtUtc).IsRequired();

        builder.HasIndex(d => new { d.TenantId, d.SalesTransactionId });
        builder.HasIndex(d => new { d.TenantId, d.Status });
    }
}
