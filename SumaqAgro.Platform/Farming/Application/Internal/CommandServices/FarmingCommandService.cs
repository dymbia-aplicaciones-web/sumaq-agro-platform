using SumaqAgro.Platform.Farming.Domain.Model.Errors;
using SumaqAgro.Platform.Farming.Domain.Repositories;
using SumaqAgro.Platform.Farming.Domain.Model.Commands;
using SumaqAgro.Platform.Farming.Application.CommandServices;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Farming.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Farming.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using SumaqAgro.Platform.Shared.Domain.Repositories;

namespace SumaqAgro.Platform.Farming.Application.Internal.CommandServices;

public sealed class FarmingCommandService(IFarmingRepository repository, IUnitOfWork unitOfWork, IPolygonMonitoring monitoring) : IFarmingCommandService
{
    public async Task<Result<FieldPlot>> RegisterAsync(RegisterFieldPlotCommand command, CancellationToken ct)
    {
        await using var transaction = await repository.BeginPlotRegistrationAsync(ct);
        var plots = await repository.ListPlotsAsync(command.OwnerUserId, ct);
        if (plots.Any(plot => string.Equals(plot.Name, command.Name, StringComparison.OrdinalIgnoreCase)))
            return Result<FieldPlot>.Failure(FarmingErrors.DuplicatePlot);
        if (plots.Count >= 4)
            return Result<FieldPlot>.Failure(FarmingErrors.PlotQuotaExceeded);

        var plot = new FieldPlot(command.OwnerUserId, command.Name, command.Region, command.AreaHectares);
        repository.Add(plot);
        try
        {
            await unitOfWork.CompleteAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (DbUpdateException exception) when (exception.InnerException is MySqlException { Number: 1062 or 1213 })
        {
            return Result<FieldPlot>.Failure(FarmingErrors.ConcurrentRegistration);
        }

        return Result<FieldPlot>.Success(plot);
    }

    public async Task<Result<FieldPlot>> DelineateAsync(DelineateFieldPlotCommand command, CancellationToken ct)
    {
        var plot = await repository.FindPlotAsync(command.Id, ct);
        if (plot is null) return Result<FieldPlot>.Failure(FarmingErrors.PlotNotFound);

        PlotBoundary boundary;
        try { boundary = PlotBoundary.Create(command.Vertices); }
        catch (ArgumentException exception)
        {
            return Result<FieldPlot>.Failure(FarmingErrors.InvalidBoundary(exception.Message));
        }

        var previousId = plot.AgroMonitoringPolygonId;
        plot.Delineate(boundary);
        await unitOfWork.CompleteAsync(ct);

        var polygonId = await monitoring.ReplaceAsync(previousId, plot.Name, boundary, ct);
        if (!string.IsNullOrWhiteSpace(polygonId))
        {
            plot.LinkPolygon(polygonId);
            await unitOfWork.CompleteAsync(ct);
        }
        return Result<FieldPlot>.Success(plot);
    }

    public async Task<Result<CropCampaign>> StartAsync(StartCropCampaignCommand command, CancellationToken ct)
    {
        if (await repository.FindPlotAsync(command.PlotId, ct) is null)
            return Result<CropCampaign>.Failure(FarmingErrors.PlotNotFound);

        var campaign = new CropCampaign(command.PlotId, command.Season.Value, command.Crop, command.Seed, command.Custom, command.SowingDate);
        repository.Add(campaign);
        try { await unitOfWork.CompleteAsync(ct); }
        catch (DbUpdateException exception) when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return Result<CropCampaign>.Failure(FarmingErrors.ActiveCampaign);
        }
        return Result<CropCampaign>.Success(campaign);
    }

    public Task<Result<CropCampaign>> SelectCropTypeAsync(SelectCropTypeCommand command, CancellationToken ct) =>
        ChangeAsync(command.Id, campaign => campaign.SelectCrop(command.Crop), ct);

    public Task<Result<CropCampaign>> SpecifySeedVarietyAsync(SpecifySeedVarietyCommand command, CancellationToken ct) =>
        ChangeAsync(command.Id, campaign => campaign.SpecifySeed(command.Seed, command.Custom), ct);

    public Task<Result<CropCampaign>> RecordSowingDateAsync(RecordSowingDateCommand command, CancellationToken ct) =>
        ChangeAsync(command.Id, campaign => campaign.RecordSowing(command.Date), ct);

    public Task<Result<CropCampaign>> CloseCropCampaignAsync(CloseCropCampaignCommand command, CancellationToken ct) =>
        ChangeAsync(command.Id, campaign => campaign.Close(), ct);

    private async Task<Result<CropCampaign>> ChangeAsync(long id, Action<CropCampaign> change, CancellationToken ct)
    {
        var campaign = await repository.FindCampaignAsync(id, ct);
        if (campaign is null)
            return Result<CropCampaign>.Failure(FarmingErrors.CampaignNotFound);
        if (campaign.Status == "FINISHED")
            return Result<CropCampaign>.Failure(FarmingErrors.CampaignFinished);

        change(campaign);
        try { await unitOfWork.CompleteAsync(ct); }
        catch (DbUpdateConcurrencyException)
        {
            return Result<CropCampaign>.Failure(FarmingErrors.CampaignChanged);
        }
        return Result<CropCampaign>.Success(campaign);
    }
}
