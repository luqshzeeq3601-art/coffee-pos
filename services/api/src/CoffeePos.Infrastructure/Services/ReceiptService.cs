using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;

namespace CoffeePos.Infrastructure.Services;

public sealed class ReceiptService : IReceiptService
{
    private readonly IPaymentStore _paymentStore;
    private readonly IOrderStore _orderStore;
    private readonly IReceiptFormatter _receiptFormatter;
    private readonly ITenantContext _tenantContext;

    public ReceiptService(
        IPaymentStore paymentStore,
        IOrderStore orderStore,
        IReceiptFormatter receiptFormatter,
        ITenantContext tenantContext)
    {
        _paymentStore = paymentStore;
        _orderStore = orderStore;
        _receiptFormatter = receiptFormatter;
        _tenantContext = tenantContext;
    }

    private Guid RequireTenantId()
    {
        return _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
    }

    public async Task<ReceiptDto?> GetReceiptAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var txn = await _paymentStore.GetTransactionByIdAsync(tenantId, transactionId, cancellationToken);
        if (txn == null) return null;

        var order = await _orderStore.GetOrderByIdAsync(tenantId, txn.OrderId, cancellationToken);
        if (order == null) return null;

        return _receiptFormatter.BuildReceiptDto(txn, order);
    }

    public async Task<ReceiptPrintPayloadDto?> GetReceiptEscPosAsync(Guid transactionId, bool kickDrawer = false, CancellationToken cancellationToken = default)
    {
        var receipt = await GetReceiptAsync(transactionId, cancellationToken);
        if (receipt == null) return null;

        return _receiptFormatter.GenerateEscPosPayload(receipt, kickDrawer);
    }
}
