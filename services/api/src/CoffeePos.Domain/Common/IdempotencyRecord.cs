namespace CoffeePos.Domain.Common;

public sealed class IdempotencyRecord
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid? OutletId { get; private set; }
    public string Operation { get; private set; }
    public string ClientTransactionId { get; private set; }
    public string PayloadHash { get; private set; }
    public string ResponseJson { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }

    public IdempotencyRecord(
        Guid id,
        Guid tenantId,
        Guid? outletId,
        string operation,
        string clientTransactionId,
        string payloadHash,
        string responseJson,
        TimeSpan? ttl = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(operation)) throw new ArgumentException("Operation cannot be empty.", nameof(operation));
        if (string.IsNullOrWhiteSpace(clientTransactionId)) throw new ArgumentException("Client transaction ID cannot be empty.", nameof(clientTransactionId));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        Operation = operation.Trim();
        ClientTransactionId = clientTransactionId.Trim();
        PayloadHash = payloadHash;
        ResponseJson = responseJson;
        CreatedAtUtc = DateTime.UtcNow;
        ExpiresAtUtc = DateTime.UtcNow.Add(ttl ?? TimeSpan.FromDays(7));
    }
}
