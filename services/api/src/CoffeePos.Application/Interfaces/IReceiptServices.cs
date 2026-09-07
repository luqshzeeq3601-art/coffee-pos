using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Orders;
using CoffeePos.Domain.Payments;

namespace CoffeePos.Application.Interfaces;

public interface IReceiptFormatter
{
    ReceiptDto BuildReceiptDto(SalesTransaction transaction, Order order);
    ReceiptPrintPayloadDto GenerateEscPosPayload(ReceiptDto receipt, bool kickDrawer = false);
    string GeneratePlainTextReceipt(ReceiptDto receipt);
}

public interface IReceiptService
{
    Task<ReceiptDto?> GetReceiptAsync(Guid transactionId, CancellationToken cancellationToken = default);
    Task<ReceiptPrintPayloadDto?> GetReceiptEscPosAsync(Guid transactionId, bool kickDrawer = false, CancellationToken cancellationToken = default);
}
