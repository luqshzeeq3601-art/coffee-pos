using CoffeePos.Application.DTOs;

namespace CoffeePos.Application.Interfaces;

public interface IReportService
{
    Task<SalesSummaryReportDto> GetDailySalesSummaryAsync(DateTime? date = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductMixItemDto>> GetProductMixAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<HourlySalesPointDto>> GetHourlyVelocityAsync(DateTime? date = null, CancellationToken cancellationToken = default);
}
