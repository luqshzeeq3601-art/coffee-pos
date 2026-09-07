namespace CoffeePos.Application.DTOs;

public record ReceiptLineItemDto(
    string Name,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal,
    IReadOnlyList<string>? Modifiers);

public record ReceiptTaxSummaryDto(
    string TaxName,
    decimal RatePercent,
    decimal TaxableAmount,
    decimal TaxAmount);

public record ReceiptPaymentDto(
    string Method,
    decimal Amount,
    string? ReferenceCode);

public record ReceiptDto(
    string ReceiptNumber,
    string OrderNumber,
    string MerchantName,
    string CompanyRegistrationNumber,
    string SstRegistrationNumber,
    string OutletName,
    string OutletAddress,
    string OutletPhone,
    string CashierName,
    string TerminalName,
    DateTime TransactionTimeUtc,
    string DiningOption,
    string? TableOrCustomer,
    IReadOnlyList<ReceiptLineItemDto> Items,
    decimal Subtotal,
    decimal DiscountsTotal,
    IReadOnlyList<ReceiptTaxSummaryDto> TaxSummary,
    decimal GrandTotal,
    IReadOnlyList<ReceiptPaymentDto> Payments,
    decimal TotalPaid,
    decimal ChangeGiven,
    string? QrVerificationUrl,
    string? FooterNotes);

public record ReceiptPrintPayloadDto(
    string ReceiptNumber,
    string Base64EscPosPayload,
    string PlainTextRepresentation,
    int ByteLength,
    bool ContainsDrawerKick);
