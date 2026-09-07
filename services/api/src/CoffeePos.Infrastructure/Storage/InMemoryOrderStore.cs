using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Orders;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryOrderStore : IOrderStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoOutletId = Guid.Parse("22222222-2222-2222-2222-222222222221");

    private readonly List<Order> _orders = new();
    private int _orderSequence = 100;

    public InMemoryOrderStore()
    {
        SeedOpenTickets();
    }

    private void SeedOpenTickets()
    {
        // Open Ticket 1: Table 4 (Sarah)
        var order1 = new Order(
            Guid.Parse("55555555-5555-5555-5555-555555555501"),
            DemoTenantId,
            DemoOutletId,
            "#104",
            DiningOption.DineIn,
            "Table 4 • Sarah",
            notes: "Serve oat flat white extra hot");

        var item1 = new OrderLineItem(
            Guid.NewGuid(),
            DemoTenantId,
            order1.Id,
            Guid.Parse("44444444-4444-4444-4444-444444444401"),
            "Oat Flat White",
            14.50m,
            2);
        item1.AddModifier(new OrderLineModifier(Guid.NewGuid(), DemoTenantId, item1.Id, Guid.NewGuid(), "Double Shot", 3.00m));
        item1.AddModifier(new OrderLineModifier(Guid.NewGuid(), DemoTenantId, item1.Id, Guid.NewGuid(), "Less Sweet (50%)", 0.00m));
        order1.AddItem(item1);

        var item2 = new OrderLineItem(
            Guid.NewGuid(),
            DemoTenantId,
            order1.Id,
            Guid.Parse("44444444-4444-4444-4444-444444444403"),
            "Almond Croissant",
            12.00m,
            1,
            notes: "Warm up");
        order1.AddItem(item2);
        order1.HoldTicket("Table 4 • Sarah");
        _orders.Add(order1);

        // Open Ticket 2: Takeaway (John)
        var order2 = new Order(
            Guid.Parse("55555555-5555-5555-5555-555555555502"),
            DemoTenantId,
            DemoOutletId,
            "#105",
            DiningOption.Takeaway,
            "Takeaway • John");

        var item3 = new OrderLineItem(
            Guid.NewGuid(),
            DemoTenantId,
            order2.Id,
            Guid.Parse("44444444-4444-4444-4444-444444444402"),
            "Pour Over (Ethiopia Guji)",
            18.00m,
            1);
        order2.AddItem(item3);
        order2.HoldTicket("Takeaway • John");
        _orders.Add(order2);
    }

    public Task<Order?> GetOrderByIdAsync(Guid tenantId, Guid orderId, CancellationToken cancellationToken = default)
    {
        var ord = _orders.FirstOrDefault(o => o.TenantId == tenantId && o.Id == orderId);
        return Task.FromResult(ord);
    }

    public Task<IReadOnlyList<Order>> GetOpenOrdersAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default)
    {
        var list = _orders.Where(o => o.TenantId == tenantId && o.OutletId == outletId && (o.Status == OrderStatus.Draft || o.Status == OrderStatus.Open)).ToList();
        return Task.FromResult<IReadOnlyList<Order>>(list);
    }

    public Task<string> GenerateNextOrderNumberAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default)
    {
        _orderSequence++;
        return Task.FromResult($"#{_orderSequence}");
    }

    public Task SaveOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        var idx = _orders.FindIndex(o => o.Id == order.Id && o.TenantId == order.TenantId);
        if (idx >= 0)
        {
            _orders[idx] = order;
        }
        else
        {
            _orders.Add(order);
        }
        return Task.CompletedTask;
    }
}
