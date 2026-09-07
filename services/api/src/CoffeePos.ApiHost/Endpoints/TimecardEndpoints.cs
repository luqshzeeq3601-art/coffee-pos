using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class TimecardEndpoints
{
    public static IEndpointRouteBuilder MapTimecardEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/timecards")
            .WithTags("Staff Timecards & Timesheets");

        group.MapGet("/", async (ITimecardService timecardService, CancellationToken ct) =>
        {
            var list = await timecardService.GetTimecardsAsync(ct);
            return Results.Ok(list);
        })
        .WithName("GetStaffTimecards")
        .Produces<IReadOnlyList<StaffTimecardDto>>(StatusCodes.Status200OK);

        group.MapPost("/clock-in", async (ClockInRequest request, ITimecardService timecardService, CancellationToken ct) =>
        {
            var timecard = await timecardService.ClockInAsync(request, ct);
            return Results.Created($"/api/v1/timecards/{timecard.Id}", timecard);
        })
        .WithName("StaffClockIn")
        .Produces<StaffTimecardDto>(StatusCodes.Status201Created);

        group.MapPost("/clock-out", async (ClockOutRequest request, ITimecardService timecardService, CancellationToken ct) =>
        {
            var timecard = await timecardService.ClockOutAsync(request, ct);
            return Results.Ok(timecard);
        })
        .WithName("StaffClockOut")
        .Produces<StaffTimecardDto>(StatusCodes.Status200OK);

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveTimecardRequest request, ITimecardService timecardService, CancellationToken ct) =>
        {
            var approved = await timecardService.ApproveTimecardAsync(id, request, ct);
            return Results.Ok(approved);
        })
        .WithName("ApproveTimecard")
        .Produces<StaffTimecardDto>(StatusCodes.Status200OK);

        return routes;
    }
}
