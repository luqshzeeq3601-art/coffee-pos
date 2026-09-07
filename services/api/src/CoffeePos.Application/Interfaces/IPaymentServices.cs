using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Payments;

namespace CoffeePos.Application.Interfaces;

public interface IPaymentStore
{
    Task<SalesTransaction?> GetTransactionByIdAsync(Guid tenantId, Guid transactionId, CancellationToken cancellationToken = default);
    Task<SalesTransaction?> GetTransactionByOrderIdAsync(Guid tenantId, Guid orderId, CancellationToken cancellationToken = default);
    Task<string> GenerateNextReceiptNumberAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default);
    Task SaveTransactionAsync(SalesTransaction transaction, CancellationToken cancellationToken = default);
}

public interface IPaymentService
{
    Task<SalesTransactionDto> ProcessPaymentAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default);
    Task<SalesTransactionDto> ProcessSplitPaymentAsync(ProcessSplitPaymentRequest request, CancellationToken cancellationToken = default);
    Task<RefundDto> ProcessRefundAsync(ProcessRefundRequest request, CancellationToken cancellationToken = default);
    Task<SalesTransactionDto?> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
}
