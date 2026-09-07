using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Payments;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryPaymentStore : IPaymentStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoOutletId = Guid.Parse("22222222-2222-2222-2222-222222222221");

    private readonly List<SalesTransaction> _transactions = new();
    private int _receiptSequence = 1000;

    public InMemoryPaymentStore()
    {
        SeedDemoSales();
    }

    private void SeedDemoSales()
    {
        var txn1 = new SalesTransaction(
            Guid.Parse("88888888-8888-8888-8888-888888888801"),
            DemoTenantId,
            DemoOutletId,
            Guid.Parse("55555555-5555-5555-5555-555555555501"),
            "#101",
            "RCP-20260819-1001",
            Guid.Parse("33333333-3333-3333-3333-333333333331"),
            "Ahmad Cashier",
            24.50m,
            0.00m,
            1.47m,
            25.97m,
            30.00m,
            4.03m);

        txn1.AddPayment(new Payment(
            Guid.NewGuid(),
            DemoTenantId,
            txn1.Id,
            PaymentMethod.Cash,
            25.97m,
            30.00m,
            4.03m));

        _transactions.Add(txn1);
    }

    public Task<SalesTransaction?> GetTransactionByIdAsync(Guid tenantId, Guid transactionId, CancellationToken cancellationToken = default)
    {
        var txn = _transactions.FirstOrDefault(t => t.TenantId == tenantId && t.Id == transactionId);
        return Task.FromResult(txn);
    }

    public Task<SalesTransaction?> GetTransactionByOrderIdAsync(Guid tenantId, Guid orderId, CancellationToken cancellationToken = default)
    {
        var txn = _transactions.FirstOrDefault(t => t.TenantId == tenantId && t.OrderId == orderId);
        return Task.FromResult(txn);
    }

    public Task<string> GenerateNextReceiptNumberAsync(Guid tenantId, Guid outletId, CancellationToken cancellationToken = default)
    {
        _receiptSequence++;
        var dateStr = DateTime.UtcNow.ToString("yyyyMMdd");
        return Task.FromResult($"RCP-{dateStr}-{_receiptSequence}");
    }

    public Task SaveTransactionAsync(SalesTransaction transaction, CancellationToken cancellationToken = default)
    {
        var idx = _transactions.FindIndex(t => t.Id == transaction.Id && t.TenantId == transaction.TenantId);
        if (idx >= 0)
        {
            _transactions[idx] = transaction;
        }
        else
        {
            _transactions.Add(transaction);
        }
        return Task.CompletedTask;
    }
}
