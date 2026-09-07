using CoffeePos.Domain.Audit;
using CoffeePos.Domain.Common;
using CoffeePos.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeePos.Infrastructure.Persistence.Configurations;

public sealed class AuditEventConfiguration : IEntityTypeConfiguration<AuditEvent>
{
    public void Configure(EntityTypeBuilder<AuditEvent> builder)
    {
        builder.ToTable("audit_events");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.TenantId).IsRequired();
        builder.Property(a => a.OutletId);
        builder.Property(a => a.DeviceId);
        builder.Property(a => a.ActorId).HasMaxLength(100).IsRequired();
        builder.Property(a => a.ActorType).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Action).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Reason);
        builder.Property(a => a.MetadataJson).HasColumnType("jsonb");
        builder.Property(a => a.TimestampUtc).IsRequired();

        builder.HasIndex(a => new { a.TenantId, a.TimestampUtc });
    }
}

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.TenantId).IsRequired();
        builder.Property(o => o.OutletId);
        builder.Property(o => o.EventType).HasMaxLength(100).IsRequired();
        builder.Property(o => o.PayloadJson).HasColumnType("jsonb").IsRequired();
        builder.Property(o => o.Status).HasConversion<int>().IsRequired();
        builder.Property(o => o.RetryCount).IsRequired();
        builder.Property(o => o.CreatedAtUtc).IsRequired();
        builder.Property(o => o.ProcessedAtUtc);
        builder.Property(o => o.LastError);

        builder.HasIndex(o => new { o.Status, o.CreatedAtUtc });
    }
}

public sealed class IdempotencyRecordConfiguration : IEntityTypeConfiguration<IdempotencyRecord>
{
    public void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
    {
        builder.ToTable("idempotency_records");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.TenantId).IsRequired();
        builder.Property(i => i.OutletId);
        builder.Property(i => i.Operation).HasMaxLength(100).IsRequired();
        builder.Property(i => i.ClientTransactionId).HasMaxLength(100).IsRequired();
        builder.Property(i => i.PayloadHash).HasMaxLength(255).IsRequired();
        builder.Property(i => i.ResponseJson).HasColumnType("jsonb").IsRequired();
        builder.Property(i => i.CreatedAtUtc).IsRequired();
        builder.Property(i => i.ExpiresAtUtc).IsRequired();

        builder.HasIndex(i => new { i.TenantId, i.Operation, i.ClientTransactionId }).IsUnique();
        builder.HasIndex(i => i.ExpiresAtUtc);
    }
}
