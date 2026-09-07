namespace CoffeePos.Domain.Payments;

public sealed class Payment
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid TransactionId { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public decimal Amount { get; private set; }
    public decimal? TenderedAmount { get; private set; }
    public decimal? ChangeAmount { get; private set; }
    public string? ReferenceCode { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public Payment(
        Guid id,
        Guid tenantId,
        Guid transactionId,
        PaymentMethod paymentMethod,
        decimal amount,
        decimal? tenderedAmount = null,
        decimal? changeAmount = null,
        string? referenceCode = null,
        PaymentStatus status = PaymentStatus.Completed)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (transactionId == Guid.Empty) throw new ArgumentException("Transaction ID cannot be empty.", nameof(transactionId));
        if (amount <= 0) throw new ArgumentException("Payment amount must be greater than zero.", nameof(amount));

        Id = id;
        TenantId = tenantId;
        TransactionId = transactionId;
        PaymentMethod = paymentMethod;
        Amount = amount;
        TenderedAmount = tenderedAmount;
        ChangeAmount = changeAmount;
        ReferenceCode = referenceCode?.Trim();
        Status = status;
        CreatedAtUtc = DateTime.UtcNow;
    }
}

public sealed class Refund
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public string Reason { get; private set; }
    public Guid ApprovedByUserId { get; private set; }
    public string ApprovedByUserName { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public Refund(
        Guid id,
        Guid tenantId,
        Guid transactionId,
        decimal amount,
        string reason,
        Guid approvedByUserId,
        string approvedByUserName)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (transactionId == Guid.Empty) throw new ArgumentException("Transaction ID cannot be empty.", nameof(transactionId));
        if (amount <= 0) throw new ArgumentException("Refund amount must be greater than zero.", nameof(amount));
        if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Refund reason is required.", nameof(reason));

        Id = id;
        TenantId = tenantId;
        TransactionId = transactionId;
        Amount = amount;
        Reason = reason.Trim();
        ApprovedByUserId = approvedByUserId;
        ApprovedByUserName = approvedByUserName.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }
}
