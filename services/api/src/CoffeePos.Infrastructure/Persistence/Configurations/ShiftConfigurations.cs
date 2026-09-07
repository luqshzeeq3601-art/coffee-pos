using CoffeePos.Domain.Shifts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("shifts");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TenantId).IsRequired();
        builder.Property(s => s.OutletId).IsRequired();
        builder.Property(s => s.CashierId).IsRequired();
        builder.Property(s => s.CashierName).HasMaxLength(150).IsRequired();
        builder.Property(s => s.Status).HasConversion<int>().IsRequired();
        builder.Property(s => s.OpenedAtUtc).IsRequired();
        builder.Property(s => s.ClosedAtUtc);
        builder.Property(s => s.OpeningFloat).HasPrecision(19, 4).IsRequired();
        builder.Property(s => s.CashSales).HasPrecision(19, 4).IsRequired();
        builder.Property(s => s.CashRefunds).HasPrecision(19, 4).IsRequired();
        builder.Property(s => s.ActualCountedCash).HasPrecision(19, 4);
        builder.Property(s => s.ClosingNotes);

        builder.HasMany(s => s.Movements)
            .WithOne()
            .HasForeignKey(m => m.ShiftId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.TenantId, s.OutletId, s.Status });
    }
}

public sealed class CashMovementConfiguration : IEntityTypeConfiguration<CashMovement>
{
    public void Configure(EntityTypeBuilder<CashMovement> builder)
    {
        builder.ToTable("cash_movements");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.TenantId).IsRequired();
        builder.Property(m => m.ShiftId).IsRequired();
        builder.Property(m => m.Type).HasConversion<int>().IsRequired();
        builder.Property(m => m.Amount).HasPrecision(19, 4).IsRequired();
        builder.Property(m => m.Reason).IsRequired();
        builder.Property(m => m.CashierId).IsRequired();
        builder.Property(m => m.CashierName).HasMaxLength(150).IsRequired();
        builder.Property(m => m.CreatedAtUtc).IsRequired();

        builder.HasIndex(m => new { m.TenantId, m.ShiftId });
    }
}
