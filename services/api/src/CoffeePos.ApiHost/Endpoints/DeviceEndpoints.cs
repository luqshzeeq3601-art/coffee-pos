using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CoffeePos.ApiHost.Endpoints;

public static class DeviceEndpoints
{
    public static IEndpointRouteBuilder MapDeviceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/devices");

        group.MapPost("/enroll", async (
            DeviceEnrollmentRequestDto request,
            IDeviceService deviceService,
            CancellationToken cancellationToken) =>
        {
            var response = await deviceService.EnrollDeviceAsync(request, cancellationToken);
            return Results.Ok(response);
        });

        group.MapGet("/{deviceId:guid}", async (
            Guid deviceId,
            IDeviceService deviceService,
            CancellationToken cancellationToken) =>
        {
            var device = await deviceService.GetDeviceAsync(deviceId, cancellationToken);
            return device != null ? Results.Ok(device) : Results.NotFound();
        });

        return endpoints;
    }
}
