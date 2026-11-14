namespace SumaqAgro.Platform.Farming.Domain.Model.Aggregates;

public sealed class CropCampaign(long fieldPlotId, string season, string cropType, string seedVariety,
    bool customSeedVariety, DateOnly? sowingDate)
{
    public long Id { get; private set; }
    public long FieldPlotId { get; private set; } = fieldPlotId;
    public string Season { get; private set; } = season;
    public string CropType { get; private set; } = cropType;
    public string SeedVariety { get; private set; } = seedVariety;
    public bool CustomSeedVariety { get; private set; } = customSeedVariety;
    public DateOnly? SowingDate { get; private set; } = sowingDate;
    public string Status { get; private set; } = "IN_PROGRESS";

    public void SelectCrop(string crop) => CropType = crop;
    public void RecordSowing(DateOnly date) => SowingDate = date;
    public void Close() => Status = "FINISHED";

    public void SpecifySeed(string variety, bool custom)
    {
        SeedVariety = variety;
        CustomSeedVariety = custom;
    }
}
