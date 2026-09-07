using CoffeePos.Domain.Devices;
using CoffeePos.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
        builder.Property(t => t.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(t => t.TimeZone).HasMaxLength(100).IsRequired();
        builder.Property(t => t.IsActive).IsRequired();
        builder.Property(t => t.CreatedAtUtc).IsRequired();
    }
}

public sealed class OutletConfiguration : IEntityTypeConfiguration<Outlet>
{
    public void Configure(EntityTypeBuilder<Outlet> builder)
    {
        builder.ToTable("outlets");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.TenantId).IsRequired();
        builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
        builder.Property(o => o.Address);
        builder.Property(o => o.IsMainOutlet).IsRequired();
        builder.Property(o => o.IsActive).IsRequired();
        builder.Property(o => o.CreatedAtUtc).IsRequired();

        builder.HasIndex(o => new { o.TenantId, o.Name }).IsUnique();
    }
}

public sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("devices");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.TenantId).IsRequired();
        builder.Property(d => d.OutletId).IsRequired();
        builder.Property(d => d.Name).HasMaxLength(150).IsRequired();
        builder.Property(d => d.DeviceType).HasConversion<int>().IsRequired();
        builder.Property(d => d.Status).HasConversion<int>().IsRequired();
        builder.Property(d => d.EnrollmentCode).HasMaxLength(50);
        builder.Property(d => d.HardwareFingerprint).HasMaxLength(255);

        builder.HasIndex(d => d.EnrollmentCode).IsUnique();
        builder.HasIndex(d => new { d.TenantId, d.OutletId });
    }
}
