using CoffeePos.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.TenantId).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
        builder.Property(u => u.FullName).HasMaxLength(200).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
        builder.Property(u => u.Salt).HasMaxLength(255).IsRequired();
        builder.Property(u => u.Role).HasConversion<int>().IsRequired();
        builder.Property(u => u.IsMfaEnabled).IsRequired();
        builder.Property(u => u.MfaSecret).HasMaxLength(255);
        builder.Property(u => u.IsActive).IsRequired();
        builder.Property(u => u.CreatedAtUtc).IsRequired();

        builder.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
    }
}

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.TenantId).IsRequired();
        builder.Property(e => e.DisplayName).HasMaxLength(150).IsRequired();
        builder.Property(e => e.PinHash).HasMaxLength(255).IsRequired();
        builder.Property(e => e.PinSalt).HasMaxLength(255).IsRequired();
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedAtUtc).IsRequired();

        builder.Ignore(e => e.Roles);
        builder.Ignore(e => e.AssignedOutletIds);

        builder.HasIndex(e => e.TenantId);
    }
}
