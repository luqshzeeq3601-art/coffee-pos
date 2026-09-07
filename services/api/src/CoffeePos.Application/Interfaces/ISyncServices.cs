using CoffeePos.Application.DTOs;

namespace CoffeePos.Application.Interfaces;

public interface ISyncService
{
    Task<WatermarkSyncResponseDto> ProcessWatermarkSyncAsync(WatermarkSyncRequestDto request, CancellationToken cancellationToken = default);
}
