using SumaqAgro.Platform.Farming.Application.QueryServices;
using SumaqAgro.Platform.Farming.Interfaces.Acl;

namespace SumaqAgro.Platform.Farming.Application.Acl;

public sealed class FieldPlotFacade(IFarmingQueryService plots) : IFieldPlotFacade
{
    public async Task<FieldPlotLocation?> FindLocationAsync(long plotId, CancellationToken ct)
    {
        var plot = await plots.FindPlotAsync(new(plotId), ct);
        return plot is null ? null : new(plot.Name, plot.Region);
    }

    public async Task<FieldPlotMonitoring?> FindMonitoringAsync(long plotId, long ownerUserId, CancellationToken ct)
    {
        var plot = await plots.FindPlotAsync(new(plotId), ct);
        return plot is null || plot.OwnerUserId != ownerUserId ? null : new(plot.AgroMonitoringPolygonId);
    }
}
