using SumaqAgro.Platform.Farming.Domain.Model.Commands;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Farming.Application.CommandServices;

public interface IFarmingCommandService
{
    Task<Result<FieldPlot>> RegisterAsync(RegisterFieldPlotCommand command, CancellationToken ct);

    Task<Result<FieldPlot>> DelineateAsync(DelineateFieldPlotCommand command, CancellationToken ct);

    Task<Result<CropCampaign>> StartAsync(StartCropCampaignCommand command, CancellationToken ct);

    Task<Result<CropCampaign>> SelectCropTypeAsync(SelectCropTypeCommand command, CancellationToken ct);

    Task<Result<CropCampaign>> SpecifySeedVarietyAsync(SpecifySeedVarietyCommand command, CancellationToken ct);

    Task<Result<CropCampaign>> RecordSowingDateAsync(RecordSowingDateCommand command, CancellationToken ct);

    Task<Result<CropCampaign>> CloseCropCampaignAsync(CloseCropCampaignCommand command, CancellationToken ct);
}
