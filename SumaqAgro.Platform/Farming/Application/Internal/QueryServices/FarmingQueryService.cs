using SumaqAgro.Platform.Farming.Domain.Model.Queries;
using SumaqAgro.Platform.Farming.Domain.Repositories;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Farming.Application.QueryServices;
using SumaqAgro.Platform.Farming.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SumaqAgro.Platform.Farming.Application.Internal.QueryServices;

public sealed class FarmingQueryService(IFarmingRepository repository) : IFarmingQueryService
{
    public Task<FieldPlot?> FindPlotAsync(GetFieldPlotByIdQuery query, CancellationToken ct) =>
        repository.FindPlotAsync(query.Id, ct);

    public Task<List<FieldPlot>> ListPlotsAsync(GetFieldPlotsByOwnerQuery query, CancellationToken ct) =>
        repository.ListPlotsAsync(query.Owner, ct);

    public Task<CropCampaign?> FindCampaignAsync(GetCropCampaignByIdQuery query, CancellationToken ct) =>
        repository.FindCampaignAsync(query.Id, ct);

    public Task<List<CropCampaign>> ListCampaignsAsync(GetCropCampaignsByPlotQuery query, CancellationToken ct) =>
        repository.ListCampaignsAsync(query.PlotId, ct);
}
