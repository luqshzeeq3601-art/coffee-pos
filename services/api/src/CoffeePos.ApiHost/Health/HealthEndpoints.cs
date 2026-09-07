using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeePos.ApiHost.Health;

public static class HealthEndpoints
{
    public static WebApplication MapHealthEndpoints(this WebApplication app)
    {
        app.MapGet("/health/live", () => Results.Json(new { status = "ok" }));

        app.MapGet("/health/ready", async (HttpContext context, CancellationToken cancellationToken) =>
        {
            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
            var report = await CheckReadinessAsync(configuration, cancellationToken);
            var payload = new
            {
                status = report.IsReady ? "ready" : "not_ready",
                dependencies = report.Dependencies
            };

            return Results.Json(
                payload,
                statusCode: report.IsReady
                    ? StatusCodes.Status200OK
                    : StatusCodes.Status503ServiceUnavailable);
        });

        return app;
    }

    private static async Task<ReadinessReport> CheckReadinessAsync(
        IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        var dependencies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["postgres"] = await CheckDependencyAsync(configuration, "Postgres", cancellationToken),
            ["redis"] = await CheckDependencyAsync(configuration, "Redis", cancellationToken)
        };

        return new ReadinessReport(
            dependencies.Values.All(static state => state == "up"),
            dependencies);
    }

    private static async Task<string> CheckDependencyAsync(
        IConfiguration configuration,
        string logicalName,
        CancellationToken cancellationToken)
    {
        if (!TryGetEndpoint(configuration, logicalName, out var host, out var port, out var configurationState))
        {
            return configurationState;
        }

        try
        {
            using var client = new TcpClient();
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromSeconds(1));
            await client.ConnectAsync(host!, port, timeout.Token);
            return "up";
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return "down";
        }
        catch (SocketException)
        {
            return "down";
        }
        catch (ArgumentException)
        {
            return "down";
        }
    }

    private static bool TryGetEndpoint(
        IConfiguration configuration,
        string logicalName,
        out string? host,
        out int port,
        out string state)
    {
        var uppercaseName = logicalName.ToUpperInvariant();
        host = configuration[$"{logicalName}:Host"]
            ?? configuration[$"{logicalName}__Host"]
            ?? configuration[$"{uppercaseName}_HOST"];
        var rawPort = configuration[$"{logicalName}:Port"]
            ?? configuration[$"{logicalName}__Port"]
            ?? configuration[$"{uppercaseName}_PORT"];

        if (string.IsNullOrWhiteSpace(host) || !int.TryParse(rawPort, out port) || port is < 1 or > 65535)
        {
            port = 0;
            state = "not_configured";
            return false;
        }

        state = "configured";
        return true;
    }

    private sealed record ReadinessReport(
        bool IsReady,
        IReadOnlyDictionary<string, string> Dependencies);
}
