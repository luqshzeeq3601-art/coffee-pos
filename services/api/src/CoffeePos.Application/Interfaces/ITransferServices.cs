using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Inventory;

namespace CoffeePos.Application.Interfaces;

public interface ITransferStore
{
    Task<IReadOnlyList<StockTransfer>> GetTransfersAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<StockTransfer?> GetTransferByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default);
    Task SaveTransferAsync(StockTransfer transfer, CancellationToken cancellationToken = default);
}

public interface ITransferService
{
    Task<IReadOnlyList<StockTransferDto>> GetTransfersAsync(CancellationToken cancellationToken = default);
    Task<StockTransferDto?> GetTransferByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StockTransferDto> CreateTransferAsync(CreateStockTransferRequest request, CancellationToken cancellationToken = default);
    Task<StockTransferDto> DispatchTransferAsync(Guid id, DispatchStockTransferRequest request, CancellationToken cancellationToken = default);
    Task<StockTransferDto> ReceiveTransferAsync(Guid id, ReceiveStockTransferRequest request, CancellationToken cancellationToken = default);
}
