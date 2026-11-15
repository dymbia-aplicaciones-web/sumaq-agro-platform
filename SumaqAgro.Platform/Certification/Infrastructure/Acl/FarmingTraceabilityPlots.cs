using SumaqAgro.Platform.Certification.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Farming.Interfaces.Acl;

namespace SumaqAgro.Platform.Certification.Infrastructure.Acl;

public sealed class FarmingTraceabilityPlots(IFieldPlotFacade farming) : ITraceabilityPlots
{
    public async Task<TraceabilityPlot?> FindAsync(long plotId, CancellationToken ct)
    {
        var plot = await farming.FindLocationAsync(plotId, ct);
        return plot is null ? null : new(plot.Name, plot.Region);
    }
}
