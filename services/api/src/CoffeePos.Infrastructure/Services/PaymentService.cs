using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Audit;
using CoffeePos.Domain.Payments;

namespace CoffeePos.Infrastructure.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly IPaymentStore _paymentStore;
    private readonly IOrderStore _orderStore;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _auditService;

    public PaymentService(
        IPaymentStore paymentStore,
        IOrderStore orderStore,
        ITenantContext tenantContext,
        ICurrentUser currentUser,
        IAuditService auditService)
    {
        _paymentStore = paymentStore;
        _orderStore = orderStore;
        _tenantContext = tenantContext;
        _currentUser = currentUser;
        _auditService = auditService;
    }

    private (Guid TenantId, Guid OutletId) RequireContext()
    {
        var tenantId = _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
        var outletId = _tenantContext.OutletId ?? Guid.Parse("22222222-2222-2222-2222-222222222221");
        return (tenantId, outletId);
    }

    public async Task<SalesTransactionDto> ProcessPaymentAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId) = RequireContext();

        // 1. Check existing transaction by orderId (Idempotency)
        var existingTxn = await _paymentStore.GetTransactionByOrderIdAsync(tenantId, request.OrderId, cancellationToken);
        if (existingTxn != null)
        {
            return MapToDto(existingTxn);
        }

        // 2. Fetch order
        var order = await _orderStore.GetOrderByIdAsync(tenantId, request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");

        Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out var method);
        if (method == 0) method = PaymentMethod.Cash;

        var tendered = request.TenderedAmount ?? request.Amount;
        var change = method == PaymentMethod.Cash ? Math.Max(0m, tendered - order.GrandTotal) : 0m;
        var receiptNumber = await _paymentStore.GenerateNextReceiptNumberAsync(tenantId, outletId, cancellationToken);

        var cashierId = _currentUser.UserId ?? Guid.Parse("33333333-3333-3333-3333-333333333331");
        var cashierName = _currentUser.Email ?? "Cashier";

        var transaction = new SalesTransaction(
            Guid.NewGuid(),
            tenantId,
            outletId,
            order.Id,
            order.OrderNumber,
            receiptNumber,
            cashierId,
            cashierName,
            order.Subtotal,
            order.DiscountTotal,
            order.TaxTotal,
            order.GrandTotal,
            tendered,
            change);

        var payment = new Payment(
            Guid.NewGuid(),
            tenantId,
            transaction.Id,
            method,
            order.GrandTotal,
            tendered,
            change,
            request.ReferenceCode);

        transaction.AddPayment(payment);

        // 3. Mark order as paid
        order.MarkPaid();
        await _orderStore.SaveOrderAsync(order, cancellationToken);
        await _paymentStore.SaveTransactionAsync(transaction, cancellationToken);

        // 4. Audit Log
        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            cashierName,
            "User",
            "ProcessPayment",
            $"Processed {method} payment of RM {order.GrandTotal:F2} for order {order.OrderNumber} (Receipt: {receiptNumber})"));

        return MapToDto(transaction);
    }

    public async Task<SalesTransactionDto> ProcessSplitPaymentAsync(ProcessSplitPaymentRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId) = RequireContext();

        var existingTxn = await _paymentStore.GetTransactionByOrderIdAsync(tenantId, request.OrderId, cancellationToken);
        if (existingTxn != null)
        {
            return MapToDto(existingTxn);
        }

        var order = await _orderStore.GetOrderByIdAsync(tenantId, request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");

        var totalTendered = request.Tenders.Sum(t => t.TenderedAmount ?? t.Amount);
        var change = Math.Max(0m, totalTendered - order.GrandTotal);
        var receiptNumber = await _paymentStore.GenerateNextReceiptNumberAsync(tenantId, outletId, cancellationToken);

        var cashierId = _currentUser.UserId ?? Guid.Parse("33333333-3333-3333-3333-333333333331");
        var cashierName = _currentUser.Email ?? "Cashier";

        var transaction = new SalesTransaction(
            Guid.NewGuid(),
            tenantId,
            outletId,
            order.Id,
            order.OrderNumber,
            receiptNumber,
            cashierId,
            cashierName,
            order.Subtotal,
            order.DiscountTotal,
            order.TaxTotal,
            order.GrandTotal,
            totalTendered,
            change);

        foreach (var t in request.Tenders)
        {
            Enum.TryParse<PaymentMethod>(t.PaymentMethod, true, out var m);
            var p = new Payment(
                Guid.NewGuid(),
                tenantId,
                transaction.Id,
                m == 0 ? PaymentMethod.Cash : m,
                t.Amount,
                t.TenderedAmount,
                null,
                t.ReferenceCode);
            transaction.AddPayment(p);
        }

        order.MarkPaid();
        await _orderStore.SaveOrderAsync(order, cancellationToken);
        await _paymentStore.SaveTransactionAsync(transaction, cancellationToken);

        return MapToDto(transaction);
    }

    public async Task<RefundDto> ProcessRefundAsync(ProcessRefundRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _) = RequireContext();
        var txn = await _paymentStore.GetTransactionByIdAsync(tenantId, request.TransactionId, cancellationToken)
            ?? throw new KeyNotFoundException($"Transaction with ID {request.TransactionId} not found.");

        // Manager PIN check demo fallback
        if (request.ManagerPin != "1234")
        {
            throw new UnauthorizedAccessException("Invalid manager PIN for refund authorization.");
        }

        var approverId = _currentUser.UserId ?? Guid.Parse("22222222-2222-2222-2222-222222222222");
        var approverName = "Siti Manager";

        var refund = new Refund(
            Guid.NewGuid(),
            tenantId,
            txn.Id,
            request.Amount,
            request.Reason,
            approverId,
            approverName);

        txn.AddRefund(refund);
        await _paymentStore.SaveTransactionAsync(txn, cancellationToken);

        await _auditService.RecordAsync(new AuditEvent(
            Guid.NewGuid(),
            tenantId,
            approverName,
            "Manager",
            "ProcessRefund",
            $"Approved refund of RM {request.Amount:F2} for receipt {txn.ReceiptNumber}. Reason: {request.Reason}"));

        return new RefundDto(
            refund.Id,
            refund.TransactionId,
            refund.Amount,
            refund.Reason,
            refund.ApprovedByUserId,
            refund.ApprovedByUserName,
            refund.CreatedAtUtc);
    }

    public async Task<SalesTransactionDto?> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var (tenantId, _) = RequireContext();
        var txn = await _paymentStore.GetTransactionByIdAsync(tenantId, transactionId, cancellationToken);
        return txn != null ? MapToDto(txn) : null;
    }

    private static SalesTransactionDto MapToDto(SalesTransaction t)
    {
        var payments = t.Payments.Select(p => new PaymentDto(
            p.Id,
            p.TransactionId,
            p.PaymentMethod.ToString(),
            p.Amount,
            p.TenderedAmount,
            p.ChangeAmount,
            p.ReferenceCode,
            p.Status.ToString(),
            p.CreatedAtUtc)).ToList();

        var refunds = t.Refunds.Select(r => new RefundDto(
            r.Id,
            r.TransactionId,
            r.Amount,
            r.Reason,
            r.ApprovedByUserId,
            r.ApprovedByUserName,
            r.CreatedAtUtc)).ToList();

        return new SalesTransactionDto(
            t.Id,
            t.TenantId,
            t.OutletId,
            t.OrderId,
            t.OrderNumber,
            t.ReceiptNumber,
            t.CashierId,
            t.CashierName,
            t.Subtotal,
            t.DiscountTotal,
            t.TaxTotal,
            t.GrandTotal,
            t.PaidAmount,
            t.ChangeAmount,
            payments,
            refunds,
            t.CreatedAtUtc);
    }
}
