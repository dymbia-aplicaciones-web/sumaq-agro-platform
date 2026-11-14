using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Domain.Model.Commands;

public sealed record StartCropCampaignCommand(long PlotId, CropSeason Season, string Crop, string Seed, bool Custom, DateOnly? SowingDate);
