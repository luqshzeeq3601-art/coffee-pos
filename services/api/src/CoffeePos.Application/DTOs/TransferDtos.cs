namespace CoffeePos.Application.DTOs;

public record StockTransferItemDto(
    Guid Id,
    Guid StockItemId,
    string StockItemName,
    string Sku,
    decimal Quantity,
    string Unit);

public record StockTransferDto(
    Guid Id,
    Guid TenantId,
    string TransferNumber,
    Guid SourceOutletId,
    string SourceOutletName,
    Guid DestinationOutletId,
    string DestinationOutletName,
    string Status,
    IReadOnlyList<StockTransferItemDto> Items,
    string? DispatchedByName,
    DateTime? DispatchedAtUtc,
    string? ReceivedByName,
    DateTime? ReceivedAtUtc,
    string? Notes,
    DateTime CreatedAtUtc);

public record TransferItemInput(
    Guid StockItemId,
    decimal Quantity);

public record CreateStockTransferRequest(
    Guid SourceOutletId,
    Guid DestinationOutletId,
    IReadOnlyList<TransferItemInput> Items,
    string? Notes);

public record DispatchStockTransferRequest(
    string? Notes);

public record ReceiveStockTransferRequest(
    string? Notes);
