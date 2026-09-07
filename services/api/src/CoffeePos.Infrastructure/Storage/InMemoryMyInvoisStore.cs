using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.MyInvois;

namespace CoffeePos.Infrastructure.Storage;

public sealed class InMemoryMyInvoisStore : IMyInvoisStore
{
    private static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly List<MyInvoisDocument> _documents = new();

    public InMemoryMyInvoisStore()
    {
        SeedDocuments();
    }

    private void SeedDocuments()
    {
        var buyer1 = new BuyerDetails(
            "C2584563200",
            BuyerIdType.BRN,
            "202301045678",
            "Petronas Digital Sdn Bhd",
            "+60323315000",
            "einvoice@petronas.com.my",
            "Tower 1, PETRONAS Twin Towers, KLCC");

        var doc1 = new MyInvoisDocument(
            Guid.Parse("77777777-7777-7777-7777-777777777001"),
            DemoTenantId,
            Guid.Parse("55555555-5555-5555-5555-555555555001"),
            "INV-2026-00101",
            buyer1,
            240.00m,
            14.40m,
            254.40m);
        doc1.MarkSubmitted(
            "LHDN-UUID-20260819-A1B2C3D4",
            "LHDN-LONG-0123456789ABCDEF0123456789ABCDEF",
            "https://myinvois.hasil.gov.my/verify/LHDN-UUID-20260819-A1B2C3D4");
        doc1.MarkValidated();

        var buyer2 = new BuyerDetails(
            "IG3456789010",
            BuyerIdType.NRIC,
            "890512145678",
            "Muhammad Danial",
            "+60172233445",
            "danial.m@gmail.com",
            "Bangsar South, Kuala Lumpur");

        var doc2 = new MyInvoisDocument(
            Guid.Parse("77777777-7777-7777-7777-777777777002"),
            DemoTenantId,
            Guid.Parse("55555555-5555-5555-5555-555555555002"),
            "INV-2026-00102",
            buyer2,
            55.00m,
            3.30m,
            58.30m);
        doc2.MarkSubmitted(
            "LHDN-UUID-20260819-E5F6G7H8",
            "LHDN-LONG-9876543210FEDCBA9876543210FEDCBA",
            "https://myinvois.hasil.gov.my/verify/LHDN-UUID-20260819-E5F6G7H8");

        _documents.AddRange(new[] { doc1, doc2 });
    }

    public Task<IReadOnlyList<MyInvoisDocument>> GetRecentDocumentsAsync(Guid tenantId, int limit = 20, CancellationToken cancellationToken = default)
    {
        var list = _documents
            .Where(d => d.TenantId == tenantId)
            .OrderByDescending(d => d.CreatedAtUtc)
            .Take(limit)
            .ToList();
        return Task.FromResult<IReadOnlyList<MyInvoisDocument>>(list);
    }

    public Task<MyInvoisDocument?> GetDocumentByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        var doc = _documents.FirstOrDefault(d => d.TenantId == tenantId && d.Id == id);
        return Task.FromResult(doc);
    }

    public Task<MyInvoisDocument?> GetDocumentByTransactionIdAsync(Guid tenantId, Guid salesTransactionId, CancellationToken cancellationToken = default)
    {
        var doc = _documents.FirstOrDefault(d => d.TenantId == tenantId && d.SalesTransactionId == salesTransactionId);
        return Task.FromResult(doc);
    }

    public Task SaveDocumentAsync(MyInvoisDocument document, CancellationToken cancellationToken = default)
    {
        var idx = _documents.FindIndex(d => d.Id == document.Id && d.TenantId == document.TenantId);
        if (idx >= 0)
        {
            _documents[idx] = document;
        }
        else
        {
            _documents.Add(document);
        }
        return Task.CompletedTask;
    }
}
