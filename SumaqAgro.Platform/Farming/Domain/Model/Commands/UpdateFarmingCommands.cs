using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Domain.Model.Commands;

public sealed record RecordSoilBaselineCommand(long Id, SoilBaseline Baseline);
public sealed record UpdateCropCampaignCommand(long Id, string? Crop, string? Seed, bool? Custom, DateOnly? SowingDate);
