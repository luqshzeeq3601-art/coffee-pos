using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.MyInvois;

namespace CoffeePos.Infrastructure.Services;

public sealed class MyInvoisService : IMyInvoisService
{
    private readonly IMyInvoisStore _myInvoisStore;
    private readonly ITenantContext _tenantContext;

    public MyInvoisService(
        IMyInvoisStore myInvoisStore,
        ITenantContext tenantContext)
    {
        _myInvoisStore = myInvoisStore;
        _tenantContext = tenantContext;
    }

    private Guid RequireTenant()
    {
        return _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
    }

    public async Task<IReadOnlyList<MyInvoisDocumentDto>> GetRecentDocumentsAsync(int limit = 20, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenant();
        var docs = await _myInvoisStore.GetRecentDocumentsAsync(tenantId, limit, cancellationToken);
        return docs.Select(MapToDto).ToList();
    }

    public async Task<MyInvoisDocumentDto?> GetDocumentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenant();
        var doc = await _myInvoisStore.GetDocumentByIdAsync(tenantId, id, cancellationToken);
        return doc != null ? MapToDto(doc) : null;
    }

    public async Task<MyInvoisDocumentDto> SubmitInvoiceAsync(SubmitInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenant();

        if (!Enum.TryParse<BuyerIdType>(request.Buyer.IdType, true, out var idType))
        {
            idType = BuyerIdType.NRIC;
        }

        var buyer = new BuyerDetails(
            request.Buyer.Tin,
            idType,
            request.Buyer.IdValue,
            request.Buyer.Name,
            request.Buyer.PhoneNumber,
            request.Buyer.Email,
            request.Buyer.Address);

        var doc = new MyInvoisDocument(
            Guid.NewGuid(),
            tenantId,
            request.SalesTransactionId,
            request.InvoiceNumber,
            buyer,
            request.TotalExcludingTax,
            request.TotalTaxAmount,
            request.TotalPayable);

        // Simulate LHDN MyInvois Sandbox API Submission & Validation
        var simulatedUuid = $"LHDN-UUID-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        var simulatedLongId = $"LHDN-LONG-{Guid.NewGuid():N}".ToUpperInvariant();
        var qrUrl = $"https://myinvois.hasil.gov.my/verify/{simulatedUuid}";

        doc.MarkSubmitted(simulatedUuid, simulatedLongId, qrUrl);
        doc.MarkValidated();

        await _myInvoisStore.SaveDocumentAsync(doc, cancellationToken);
        return MapToDto(doc);
    }

    public async Task<MyInvoisDocumentDto> CancelInvoiceAsync(Guid id, CancelInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenant();
        var doc = await _myInvoisStore.GetDocumentByIdAsync(tenantId, id, cancellationToken)
            ?? throw new KeyNotFoundException($"MyInvois document with ID {id} not found.");

        doc.Cancel(request.Reason);
        await _myInvoisStore.SaveDocumentAsync(doc, cancellationToken);
        return MapToDto(doc);
    }

    private static MyInvoisDocumentDto MapToDto(MyInvoisDocument d)
    {
        return new MyInvoisDocumentDto(
            d.Id,
            d.TenantId,
            d.SalesTransactionId,
            d.InvoiceNumber,
            d.Uuid,
            d.LongId,
            d.Status.ToString(),
            new BuyerDetailsDto(
                d.Buyer.Tin,
                d.Buyer.IdType.ToString(),
                d.Buyer.IdValue,
                d.Buyer.Name,
                d.Buyer.PhoneNumber,
                d.Buyer.Email,
                d.Buyer.Address),
            d.TotalExcludingTax,
            d.TotalTaxAmount,
            d.TotalPayable,
            d.QrCodeUrl,
            d.ValidationErrorsJson,
            d.SubmittedAtUtc,
            d.ValidatedAtUtc,
            d.CreatedAtUtc);
    }
}
