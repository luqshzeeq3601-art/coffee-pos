using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class ReportEndpoints
{
    public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/reports")
            .WithTags("Sales Analytics & Reports");

        group.MapGet("/daily-sales", async (DateTime? date, IReportService reportService, CancellationToken ct) =>
        {
            var report = await reportService.GetDailySalesSummaryAsync(date, ct);
            return Results.Ok(report);
        })
        .WithName("GetDailySalesSummary")
        .Produces<SalesSummaryReportDto>(StatusCodes.Status200OK);

        group.MapGet("/product-mix", async (DateTime? startDate, DateTime? endDate, IReportService reportService, CancellationToken ct) =>
        {
            var mix = await reportService.GetProductMixAsync(startDate, endDate, ct);
            return Results.Ok(mix);
        })
        .WithName("GetProductMix")
        .Produces<IReadOnlyList<ProductMixItemDto>>(StatusCodes.Status200OK);

        group.MapGet("/hourly-velocity", async (DateTime? date, IReportService reportService, CancellationToken ct) =>
        {
            var velocity = await reportService.GetHourlyVelocityAsync(date, ct);
            return Results.Ok(velocity);
        })
        .WithName("GetHourlyVelocity")
        .Produces<IReadOnlyList<HourlySalesPointDto>>(StatusCodes.Status200OK);

        return routes;
    }
}
