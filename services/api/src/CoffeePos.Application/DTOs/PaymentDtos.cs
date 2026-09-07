namespace CoffeePos.Application.DTOs;

public record PaymentDto(
    Guid Id,
    Guid TransactionId,
    string PaymentMethod,
    decimal Amount,
    decimal? TenderedAmount,
    decimal? ChangeAmount,
    string? ReferenceCode,
    string Status,
    DateTime CreatedAtUtc);

public record RefundDto(
    Guid Id,
    Guid TransactionId,
    decimal Amount,
    string Reason,
    Guid ApprovedByUserId,
    string ApprovedByUserName,
    DateTime CreatedAtUtc);

public record SalesTransactionDto(
    Guid Id,
    Guid TenantId,
    Guid OutletId,
    Guid OrderId,
    string OrderNumber,
    string ReceiptNumber,
    Guid CashierId,
    string CashierName,
    decimal Subtotal,
    decimal DiscountTotal,
    decimal TaxTotal,
    decimal GrandTotal,
    decimal PaidAmount,
    decimal ChangeAmount,
    IReadOnlyList<PaymentDto> Payments,
    IReadOnlyList<RefundDto> Refunds,
    DateTime CreatedAtUtc);

public record ProcessPaymentRequest(
    Guid OrderId,
    string IdempotencyKey,
    string PaymentMethod,
    decimal Amount,
    decimal? TenderedAmount,
    string? ReferenceCode);

public record SplitTenderItemDto(
    string PaymentMethod,
    decimal Amount,
    decimal? TenderedAmount,
    string? ReferenceCode);

public record ProcessSplitPaymentRequest(
    Guid OrderId,
    string IdempotencyKey,
    IReadOnlyList<SplitTenderItemDto> Tenders);

public record ProcessRefundRequest(
    Guid TransactionId,
    decimal Amount,
    string Reason,
    string ManagerPin);
