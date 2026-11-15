namespace SumaqAgro.Platform.Crop.Application.Internal.OutboundServices;

public interface IMonitoringPlots
{
    Task<MonitoringPlot?> FindOwnedAsync(long plotId, long ownerUserId, CancellationToken ct);
}

public sealed record MonitoringPlot(string? PolygonId);
