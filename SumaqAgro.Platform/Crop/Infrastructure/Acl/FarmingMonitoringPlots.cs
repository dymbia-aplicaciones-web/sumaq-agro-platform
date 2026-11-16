using SumaqAgro.Platform.Crop.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Farming.Interfaces.Acl;

namespace SumaqAgro.Platform.Crop.Infrastructure.Acl;

public sealed class FarmingMonitoringPlots(IFieldPlotFacade farming) : IMonitoringPlots
{
    public async Task<MonitoringPlot?> FindOwnedAsync(long plotId, long ownerUserId, CancellationToken ct)
    {
        var plot = await farming.FindMonitoringAsync(plotId, ownerUserId, ct);
        return plot is null ? null : new(plot.PolygonId);
    }
}
