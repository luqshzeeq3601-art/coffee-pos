namespace CoffeePos.Application.DTOs;

public record OrderModifierDto(
    Guid Id,
    Guid ModifierId,
    string Name,
    decimal PriceDelta);

public record OrderLineItemDto(
    Guid Id,
    Guid ProductId,
    Guid? VariantId,
    string Name,
    decimal UnitPrice,
    int Quantity,
    IReadOnlyList<OrderModifierDto> Modifiers,
    string? Notes,
    decimal LineTotal);

public record OrderDto(
    Guid Id,
    Guid TenantId,
    Guid OutletId,
    string OrderNumber,
    string Status,
    string DiningOption,
    string? TableOrCustomer,
    Guid? CashierId,
    string? CashierName,
    IReadOnlyList<OrderLineItemDto> Items,
    decimal Subtotal,
    decimal DiscountTotal,
    decimal TaxTotal,
    decimal GrandTotal,
    string? Notes,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public record CreateOrderLineItemRequest(
    Guid ProductId,
    Guid? VariantId,
    string Name,
    decimal UnitPrice,
    int Quantity,
    IReadOnlyList<OrderModifierDto>? Modifiers,
    string? Notes);

public record CreateOrderRequest(
    string DiningOption,
    string? TableOrCustomer,
    string? Notes,
    IReadOnlyList<CreateOrderLineItemRequest> Items);

public record HoldTicketRequest(
    string TableOrCustomer,
    string? Notes);

public record VoidOrderRequest(
    string Reason,
    string? ManagerPin);
