namespace CoffeePos.Application.DTOs;

public record HourlySalesPointDto(
    int Hour,
    string Label,
    int OrderCount,
    decimal GrossSales,
    decimal NetSales);

public record ProductMixItemDto(
    Guid ProductId,
    string ProductName,
    string Category,
    int UnitsSold,
    decimal TotalRevenue,
    decimal PercentOfSales);

public record PaymentBreakdownDto(
    string Method,
    int TransactionCount,
    decimal TotalAmount,
    decimal Percentage);

public record SalesSummaryReportDto(
    Guid TenantId,
    Guid OutletId,
    string Date,
    int TotalOrders,
    decimal GrossSales,
    decimal DiscountsTotal,
    decimal NetSales,
    decimal TaxTotal,
    decimal CashSales,
    decimal CardSales,
    decimal QrSales,
    decimal AverageOrderValue,
    IReadOnlyList<HourlySalesPointDto> HourlyVelocity,
    IReadOnlyList<ProductMixItemDto> ProductMix,
    IReadOnlyList<PaymentBreakdownDto> PaymentBreakdown);
