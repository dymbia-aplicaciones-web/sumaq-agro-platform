using SumaqAgro.Platform.Farming.Domain.Model.Queries;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Application.QueryServices;

public interface IFarmingQueryService
{
    Task<FieldPlot?> FindPlotAsync(GetFieldPlotByIdQuery query, CancellationToken ct);

    Task<List<FieldPlot>> ListPlotsAsync(GetFieldPlotsByOwnerQuery query, CancellationToken ct);

    Task<CropCampaign?> FindCampaignAsync(GetCropCampaignByIdQuery query, CancellationToken ct);

    Task<List<CropCampaign>> ListCampaignsAsync(GetCropCampaignsByPlotQuery query, CancellationToken ct);
}
