namespace SumaqAgro.Platform.Farming.Interfaces.Rest.Resources;

public sealed record CropCampaignResource(long Id, long FieldPlotId, string Season, string CropType,
    string SeedVariety, bool CustomSeedVariety, DateOnly? SowingDate, string Status);
