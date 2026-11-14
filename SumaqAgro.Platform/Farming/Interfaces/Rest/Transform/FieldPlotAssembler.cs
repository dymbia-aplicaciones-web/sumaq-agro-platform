using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Farming.Domain.Model.Commands;
using SumaqAgro.Platform.Farming.Interfaces.Rest.Resources;

namespace SumaqAgro.Platform.Farming.Interfaces.Rest.Transform;

internal static class FieldPlotAssembler
{
    public static RegisterFieldPlotCommand ToCommand(RegisterFieldPlotResource resource) =>
        new(resource.OwnerUserId, resource.Name, resource.Region, resource.AreaHectares!.Value);

    public static FieldPlotResource ToResource(FieldPlot plot) =>
        new(plot.Id, plot.OwnerUserId, plot.Name, plot.Region, plot.Status, plot.AreaHectares,
            plot.AgroMonitoringPolygonId, plot.Boundary ?? []);
}
