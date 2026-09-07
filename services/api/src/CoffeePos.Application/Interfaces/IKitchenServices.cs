using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Kitchen;

namespace CoffeePos.Application.Interfaces;

public interface IKitchenStore
{
    Task<IReadOnlyList<KitchenChit>> GetChitsAsync(Guid tenantId, Guid outletId, PrepStation? station = null, bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<KitchenChit?> GetChitByIdAsync(Guid tenantId, Guid chitId, CancellationToken cancellationToken = default);
    Task SaveChitAsync(KitchenChit chit, CancellationToken cancellationToken = default);
}

public interface IKitchenService
{
    Task<IReadOnlyList<KitchenChitDto>> GetChitsAsync(string? station = null, bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<KitchenChitDto?> GetChitByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<KitchenChitDto> BumpChitAsync(Guid id, BumpChitRequest request, CancellationToken cancellationToken = default);
    Task<KitchenChitDto> RecallChitAsync(Guid id, RecallChitRequest request, CancellationToken cancellationToken = default);
}
