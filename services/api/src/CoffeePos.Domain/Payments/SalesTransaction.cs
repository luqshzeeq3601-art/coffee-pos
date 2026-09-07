namespace CoffeePos.Domain.Payments;

public sealed class SalesTransaction
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OutletId { get; private set; }
    public Guid OrderId { get; private set; }
    public string OrderNumber { get; private set; }
    public string ReceiptNumber { get; private set; }
    public Guid CashierId { get; private set; }
    public string CashierName { get; private set; }

    public decimal Subtotal { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal TaxTotal { get; private set; }
    public decimal GrandTotal { get; private set; }
    public decimal PaidAmount { get; private set; }
    public decimal ChangeAmount { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private readonly List<Payment> _payments = new();
    public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

    private readonly List<Refund> _refunds = new();
    public IReadOnlyList<Refund> Refunds => _refunds.AsReadOnly();

    public decimal TotalRefunded => _refunds.Sum(r => r.Amount);
    public bool IsFullyRefunded => TotalRefunded >= GrandTotal;

    public SalesTransaction(
        Guid id,
        Guid tenantId,
        Guid outletId,
        Guid orderId,
        string orderNumber,
        string receiptNumber,
        Guid cashierId,
        string cashierName,
        decimal subtotal,
        decimal discountTotal,
        decimal taxTotal,
        decimal grandTotal,
        decimal paidAmount,
        decimal changeAmount)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (outletId == Guid.Empty) throw new ArgumentException("Outlet ID cannot be empty.", nameof(outletId));
        if (orderId == Guid.Empty) throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        if (string.IsNullOrWhiteSpace(orderNumber)) throw new ArgumentException("Order number cannot be empty.", nameof(orderNumber));
        if (string.IsNullOrWhiteSpace(receiptNumber)) throw new ArgumentException("Receipt number cannot be empty.", nameof(receiptNumber));
        if (paidAmount < grandTotal) throw new ArgumentException("Paid amount cannot be less than grand total.", nameof(paidAmount));

        Id = id;
        TenantId = tenantId;
        OutletId = outletId;
        OrderId = orderId;
        OrderNumber = orderNumber.Trim();
        ReceiptNumber = receiptNumber.Trim();
        CashierId = cashierId;
        CashierName = cashierName.Trim();
        Subtotal = subtotal;
        DiscountTotal = discountTotal;
        TaxTotal = taxTotal;
        GrandTotal = grandTotal;
        PaidAmount = paidAmount;
        ChangeAmount = changeAmount;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void AddPayment(Payment payment)
    {
        _payments.Add(payment);
    }

    public void AddRefund(Refund refund)
    {
        if (TotalRefunded + refund.Amount > GrandTotal)
            throw new InvalidOperationException($"Cumulative refunds ({TotalRefunded + refund.Amount:F2}) cannot exceed sale total ({GrandTotal:F2}).");

        _refunds.Add(refund);
    }
}
