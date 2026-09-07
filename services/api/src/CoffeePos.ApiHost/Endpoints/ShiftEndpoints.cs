using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class ShiftEndpoints
{
    public static IEndpointRouteBuilder MapShiftEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/shifts")
            .WithTags("Shifts & Cash Control");

        group.MapGet("/active", async (IShiftService shiftService, CancellationToken ct) =>
        {
            var shift = await shiftService.GetActiveShiftAsync(ct);
            return shift != null ? Results.Ok(shift) : Results.NotFound();
        })
        .WithName("GetActiveShift")
        .Produces<ShiftDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/open", async (OpenShiftRequest request, IShiftService shiftService, CancellationToken ct) =>
        {
            var shift = await shiftService.OpenShiftAsync(request, ct);
            return Results.Created($"/api/v1/shifts/{shift.Id}", shift);
        })
        .WithName("OpenShift")
        .Produces<ShiftDto>(StatusCodes.Status201Created);

        group.MapPost("/{id:guid}/movements", async (Guid id, CashMovementRequest request, IShiftService shiftService, CancellationToken ct) =>
        {
            var shift = await shiftService.RecordCashMovementAsync(id, request, ct);
            return Results.Ok(shift);
        })
        .WithName("RecordCashMovement")
        .Produces<ShiftDto>(StatusCodes.Status200OK);

        group.MapPost("/{id:guid}/close", async (Guid id, CloseShiftRequest request, IShiftService shiftService, CancellationToken ct) =>
        {
            var report = await shiftService.CloseShiftAsync(id, request, ct);
            return Results.Ok(report);
        })
        .WithName("CloseShift")
        .Produces<ShiftSummaryReportDto>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}/report", async (Guid id, IShiftService shiftService, CancellationToken ct) =>
        {
            var report = await shiftService.GetShiftReportAsync(id, ct);
            return Results.Ok(report);
        })
        .WithName("GetShiftReport")
        .Produces<ShiftSummaryReportDto>(StatusCodes.Status200OK);

        return routes;
    }
}
