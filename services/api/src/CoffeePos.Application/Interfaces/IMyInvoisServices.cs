using CoffeePos.Application.DTOs;
using CoffeePos.Domain.MyInvois;

namespace CoffeePos.Application.Interfaces;

public interface IMyInvoisStore
{
    Task<IReadOnlyList<MyInvoisDocument>> GetRecentDocumentsAsync(Guid tenantId, int limit = 20, CancellationToken cancellationToken = default);
    Task<MyInvoisDocument?> GetDocumentByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default);
    Task<MyInvoisDocument?> GetDocumentByTransactionIdAsync(Guid tenantId, Guid salesTransactionId, CancellationToken cancellationToken = default);
    Task SaveDocumentAsync(MyInvoisDocument document, CancellationToken cancellationToken = default);
}

public interface IMyInvoisService
{
    Task<IReadOnlyList<MyInvoisDocumentDto>> GetRecentDocumentsAsync(int limit = 20, CancellationToken cancellationToken = default);
    Task<MyInvoisDocumentDto?> GetDocumentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MyInvoisDocumentDto> SubmitInvoiceAsync(SubmitInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<MyInvoisDocumentDto> CancelInvoiceAsync(Guid id, CancelInvoiceRequest request, CancellationToken cancellationToken = default);
}
