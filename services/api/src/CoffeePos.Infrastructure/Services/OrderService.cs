using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Orders;

namespace CoffeePos.Infrastructure.Services;

public sealed class OrderService : IOrderService
{
    private readonly IOrderStore _orderStore;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _auditService;

    public OrderService(
        IOrderStore orderStore,
        ITenantContext tenantContext,
        ICurrentUser currentUser,
        IAuditService auditService)
    {
        _orderStore = orderStore;
        _tenantContext = tenantContext;
        _currentUser = currentUser;
        _auditService = auditService;
    }

    private (Guid TenantId, Guid OutletId) RequireContext()
    {
        var tenantId = _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
        var outletId = _tenantContext.OutletId ?? Guid.Parse("22222222-2222-2222-2222-222222222221"); // Fallback to main demo outlet
        return (tenantId, outletId);
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId) = RequireContext();
        var orderNumber = await _orderStore.GenerateNextOrderNumberAsync(tenantId, outletId, cancellationToken);

        Enum.TryParse<DiningOption>(request.DiningOption, true, out var diningOption);

        var order = new Order(
            Guid.NewGuid(),
            tenantId,
            outletId,
            orderNumber,
            diningOption == 0 ? DiningOption.DineIn : diningOption,
            request.TableOrCustomer,
            _currentUser.UserId,
            _currentUser.Email,
            6.00m, // 6% SST
            request.Notes);

        foreach (var itemReq in request.Items)
        {
            var lineItem = new OrderLineItem(
                Guid.NewGuid(),
                tenantId,
                order.Id,
                itemReq.ProductId,
                itemReq.Name,
                itemReq.UnitPrice,
                itemReq.Quantity,
                itemReq.VariantId,
                itemReq.Notes);

            if (itemReq.Modifiers != null)
            {
                foreach (var modReq in itemReq.Modifiers)
                {
                    lineItem.AddModifier(new OrderLineModifier(
                        Guid.NewGuid(),
                        tenantId,
                        lineItem.Id,
                        modReq.ModifierId,
                        modReq.Name,
                        modReq.PriceDelta));
                }
            }

            order.AddItem(lineItem);
        }

        await _orderStore.SaveOrderAsync(order, cancellationToken);
        return MapToDto(order);
    }

    public async Task<OrderDto> HoldTicketAsync(Guid orderId, HoldTicketRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _) = RequireContext();
        var order = await _orderStore.GetOrderByIdAsync(tenantId, orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");

        order.HoldTicket(request.TableOrCustomer, request.Notes);
        await _orderStore.SaveOrderAsync(order, cancellationToken);

        await _auditService.RecordAsync(new Domain.Audit.AuditEvent(
            Guid.NewGuid(),
            tenantId,
            _currentUser.UserId?.ToString() ?? "Cashier",
            "User",
            "HoldTicket",
            $"Held open ticket {order.OrderNumber} for {request.TableOrCustomer}"));

        return MapToDto(order);
    }

    public async Task<IReadOnlyList<OrderDto>> GetOpenOrdersAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId) = RequireContext();
        var orders = await _orderStore.GetOpenOrdersAsync(tenantId, outletId, cancellationToken);
        return orders.Select(MapToDto).ToList();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var (tenantId, _) = RequireContext();
        var order = await _orderStore.GetOrderByIdAsync(tenantId, orderId, cancellationToken);
        return order != null ? MapToDto(order) : null;
    }

    public async Task<bool> VoidOrderAsync(Guid orderId, VoidOrderRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _) = RequireContext();
        var order = await _orderStore.GetOrderByIdAsync(tenantId, orderId, cancellationToken);
        if (order == null) return false;

        order.Cancel(request.Reason);
        await _orderStore.SaveOrderAsync(order, cancellationToken);

        await _auditService.RecordAsync(new Domain.Audit.AuditEvent(
            Guid.NewGuid(),
            tenantId,
            _currentUser.UserId?.ToString() ?? "Cashier",
            "User",
            "VoidOrder",
            $"Voided ticket {order.OrderNumber}. Reason: {request.Reason}"));

        return true;
    }

    private static OrderDto MapToDto(Order o)
    {
        var items = o.Items.Select(i => new OrderLineItemDto(
            i.Id,
            i.ProductId,
            i.VariantId,
            i.Name,
            i.UnitPrice,
            i.Quantity,
            i.Modifiers.Select(m => new OrderModifierDto(m.Id, m.ModifierId, m.Name, m.PriceDelta)).ToList(),
            i.Notes,
            i.LineTotal)).ToList();

        return new OrderDto(
            o.Id,
            o.TenantId,
            o.OutletId,
            o.OrderNumber,
            o.Status.ToString(),
            o.DiningOption.ToString(),
            o.TableOrCustomer,
            o.CashierId,
            o.CashierName,
            items,
            o.Subtotal,
            o.DiscountTotal,
            o.TaxTotal,
            o.GrandTotal,
            o.Notes,
            o.CreatedAtUtc,
            o.UpdatedAtUtc);
    }
}
