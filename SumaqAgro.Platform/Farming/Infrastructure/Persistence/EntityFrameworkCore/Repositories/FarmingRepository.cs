using SumaqAgro.Platform.Farming.Domain.Repositories;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Farming.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class FarmingRepository(AppDbContext context) : IFarmingRepository
{
    public Task<FieldPlot?> FindPlotAsync(long id, CancellationToken ct) =>
        context.Set<FieldPlot>().SingleOrDefaultAsync(plot => plot.Id == id, ct);

    public Task<List<FieldPlot>> ListPlotsAsync(long owner, CancellationToken ct) =>
        context.Set<FieldPlot>().AsNoTracking().Where(plot => plot.OwnerUserId == owner).OrderBy(plot => plot.Id).ToListAsync(ct);

    public Task<CropCampaign?> FindCampaignAsync(long id, CancellationToken ct) =>
        context.Set<CropCampaign>().SingleOrDefaultAsync(campaign => campaign.Id == id, ct);

    public Task<List<CropCampaign>> ListCampaignsAsync(long plotId, CancellationToken ct) =>
        context.Set<CropCampaign>().AsNoTracking().Where(campaign => campaign.FieldPlotId == plotId)
            .OrderBy(campaign => campaign.Id).ToListAsync(ct);

    public async Task<IPlotRegistrationTransaction> BeginPlotRegistrationAsync(CancellationToken ct) =>
        new PlotRegistrationTransaction(await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct));

    private sealed class PlotRegistrationTransaction(IDbContextTransaction transaction) : IPlotRegistrationTransaction
    {
        public Task CommitAsync(CancellationToken ct) => transaction.CommitAsync(ct);
        public ValueTask DisposeAsync() => transaction.DisposeAsync();
    }

    public void Add(FieldPlot plot) => context.Add(plot);
    public void Add(CropCampaign campaign) => context.Add(campaign);
}
