using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Application.Internal.OutboundServices;

public interface IPolygonMonitoring
{
    Task<string?> ReplaceAsync(string? previousId, string name, PlotBoundary boundary, CancellationToken cancellationToken);
}
