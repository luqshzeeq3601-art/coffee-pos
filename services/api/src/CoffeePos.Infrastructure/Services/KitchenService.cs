using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Kitchen;

namespace CoffeePos.Infrastructure.Services;

public sealed class KitchenService : IKitchenService
{
    private readonly IKitchenStore _kitchenStore;
    private readonly ITenantContext _tenantContext;

    public KitchenService(
        IKitchenStore kitchenStore,
        ITenantContext tenantContext)
    {
        _kitchenStore = kitchenStore;
        _tenantContext = tenantContext;
    }

    private (Guid TenantId, Guid OutletId) RequireContext()
    {
        var tenantId = _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required.");
        var outletId = _tenantContext.OutletId ?? Guid.Parse("22222222-2222-2222-2222-222222222221");
        return (tenantId, outletId);
    }

    public async Task<IReadOnlyList<KitchenChitDto>> GetChitsAsync(string? station = null, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var (tenantId, outletId) = RequireContext();
        PrepStation? prepStation = null;
        if (!string.IsNullOrEmpty(station) && Enum.TryParse<PrepStation>(station, true, out var parsedStation))
        {
            prepStation = parsedStation;
        }

        var chits = await _kitchenStore.GetChitsAsync(tenantId, outletId, prepStation, activeOnly, cancellationToken);
        return chits.Select(MapToDto).ToList();
    }

    public async Task<KitchenChitDto?> GetChitByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var (tenantId, _) = RequireContext();
        var chit = await _kitchenStore.GetChitByIdAsync(tenantId, id, cancellationToken);
        return chit != null ? MapToDto(chit) : null;
    }

    public async Task<KitchenChitDto> BumpChitAsync(Guid id, BumpChitRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _) = RequireContext();
        var chit = await _kitchenStore.GetChitByIdAsync(tenantId, id, cancellationToken)
            ?? throw new KeyNotFoundException($"Kitchen chit with ID {id} not found.");

        ChitStatus? targetStatus = null;
        if (!string.IsNullOrEmpty(request.NextStatus) && Enum.TryParse<ChitStatus>(request.NextStatus, true, out var parsed))
        {
            targetStatus = parsed;
        }

        chit.Bump(targetStatus);
        await _kitchenStore.SaveChitAsync(chit, cancellationToken);
        return MapToDto(chit);
    }

    public async Task<KitchenChitDto> RecallChitAsync(Guid id, RecallChitRequest request, CancellationToken cancellationToken = default)
    {
        var (tenantId, _) = RequireContext();
        var chit = await _kitchenStore.GetChitByIdAsync(tenantId, id, cancellationToken)
            ?? throw new KeyNotFoundException($"Kitchen chit with ID {id} not found.");

        chit.Recall();
        await _kitchenStore.SaveChitAsync(chit, cancellationToken);
        return MapToDto(chit);
    }

    private static KitchenChitDto MapToDto(KitchenChit c)
    {
        return new KitchenChitDto(
            c.Id,
            c.TenantId,
            c.OutletId,
            c.OrderId,
            c.OrderNumber,
            c.DiningOption.ToString(),
            c.CustomerName,
            c.TableNumber,
            c.Station.ToString(),
            c.Status.ToString(),
            c.ElapsedSeconds,
            c.CreatedAtUtc,
            c.StartedAtUtc,
            c.CompletedAtUtc,
            c.Items.Select(i => new KitchenChitItemDto(
                i.Id,
                i.ProductId,
                i.ProductName,
                i.VariantName,
                i.ModifiersSummary,
                i.Notes,
                i.Quantity,
                i.IsPrepared)).ToList());
    }
}
