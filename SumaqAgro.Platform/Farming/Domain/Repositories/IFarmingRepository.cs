using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Farming.Domain.Repositories;

public interface IFarmingRepository
{
    Task<FieldPlot?> FindPlotAsync(long id, CancellationToken ct);
    Task<List<FieldPlot>> ListPlotsAsync(long owner, CancellationToken ct);
    Task<CropCampaign?> FindCampaignAsync(long id, CancellationToken ct);
    Task<List<CropCampaign>> ListCampaignsAsync(long plotId, CancellationToken ct);
    Task<IPlotRegistrationTransaction> BeginPlotRegistrationAsync(CancellationToken ct);
    void Add(FieldPlot plot);
    void Add(CropCampaign campaign);
}

public interface IPlotRegistrationTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken ct);
}
