using CoffeePos.Domain.Staff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class StaffTimecardConfiguration : IEntityTypeConfiguration<StaffTimecard>
{
    public void Configure(EntityTypeBuilder<StaffTimecard> builder)
    {
        builder.ToTable("staff_timecards");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TenantId).IsRequired();
        builder.Property(t => t.OutletId).IsRequired();
        builder.Property(t => t.EmployeeId).IsRequired();
        builder.Property(t => t.EmployeeName).HasMaxLength(150).IsRequired();
        builder.Property(t => t.ClockInUtc).IsRequired();
        builder.Property(t => t.ClockOutUtc);
        builder.Property(t => t.DurationMinutes).IsRequired();
        builder.Property(t => t.RegularHours).HasPrecision(6, 2).IsRequired();
        builder.Property(t => t.OvertimeHours).HasPrecision(6, 2).IsRequired();
        builder.Property(t => t.Status).HasConversion<int>().IsRequired();
        builder.Property(t => t.Notes);

        builder.HasIndex(t => new { t.TenantId, t.EmployeeId });
    }
}
