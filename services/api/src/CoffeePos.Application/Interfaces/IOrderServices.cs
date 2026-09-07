using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Orders;

namespace CoffeePos.Application.Interfaces;

public interface IOrderStore
{
    Task<Order?> GetOrderByIdAsync(Guid tenantId, Guid orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetOpenOrdersAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default);
    Task<string> GenerateNextOrderNumberAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default);
    Task SaveOrderAsync(Order order, CancellationToken cancellationToken = default);
}

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<OrderDto> HoldTicketAsync(Guid orderId, HoldTicketRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderDto>> GetOpenOrdersAsync(CancellationToken cancellationToken = default);
    Task<OrderDto?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<bool> VoidOrderAsync(Guid orderId, VoidOrderRequest request, CancellationToken cancellationToken = default);
}
