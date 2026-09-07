namespace CoffeePos.Application.DTOs;

public record BuyerDetailsDto(
    string Tin,
    string IdType,
    string IdValue,
    string Name,
    string? PhoneNumber,
    string? Email,
    string? Address);

public record MyInvoisDocumentDto(
    Guid Id,
    Guid TenantId,
    Guid SalesTransactionId,
    string InvoiceNumber,
    string? Uuid,
    string? LongId,
    string Status,
    BuyerDetailsDto Buyer,
    decimal TotalExcludingTax,
    decimal TotalTaxAmount,
    decimal TotalPayable,
    string? QrCodeUrl,
    string? ValidationErrors,
    DateTime? SubmittedAtUtc,
    DateTime? ValidatedAtUtc,
    DateTime CreatedAtUtc);

public record SubmitInvoiceRequest(
    Guid SalesTransactionId,
    string InvoiceNumber,
    BuyerDetailsDto Buyer,
    decimal TotalExcludingTax,
    decimal TotalTaxAmount,
    decimal TotalPayable);

public record CancelInvoiceRequest(
    string Reason);
