using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/orders")
            .WithTags("Orders & Open Tickets");

        group.MapPost("/", async (CreateOrderRequest request, IOrderService orderService, CancellationToken ct) =>
        {
            var order = await orderService.CreateOrderAsync(request, ct);
            return Results.Created($"/api/v1/orders/{order.Id}", order);
        })
        .WithName("CreateOrder")
        .Produces<OrderDto>(StatusCodes.Status201Created);

        group.MapGet("/open", async (IOrderService orderService, CancellationToken ct) =>
        {
            var openOrders = await orderService.GetOpenOrdersAsync(ct);
            return Results.Ok(openOrders);
        })
        .WithName("GetOpenOrders")
        .Produces<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", async (Guid id, IOrderService orderService, CancellationToken ct) =>
        {
            var order = await orderService.GetOrderByIdAsync(id, ct);
            return order != null ? Results.Ok(order) : Results.NotFound();
        })
        .WithName("GetOrderById")
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}/hold", async (Guid id, HoldTicketRequest request, IOrderService orderService, CancellationToken ct) =>
        {
            var order = await orderService.HoldTicketAsync(id, request, ct);
            return Results.Ok(order);
        })
        .WithName("HoldTicket")
        .Produces<OrderDto>(StatusCodes.Status200OK);

        group.MapDelete("/{id:guid}", async (Guid id, VoidOrderRequest request, IOrderService orderService, CancellationToken ct) =>
        {
            var success = await orderService.VoidOrderAsync(id, request, ct);
            return success ? Results.NoContent() : Results.NotFound();
        })
        .WithName("VoidOrder")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return routes;
    }
}
