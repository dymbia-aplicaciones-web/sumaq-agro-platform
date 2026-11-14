using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Farming.Domain.Model.Errors;

public static class FarmingErrors
{
    public static readonly Error DuplicatePlot = Error.Conflict("FieldPlot", "A plot with this name already exists.");
    public static readonly Error PlotQuotaExceeded = new Error("BUSINESS_RULE_VIOLATION", "The producer reached the plot quota.");
    public static readonly Error ConcurrentRegistration = Error.Conflict("FieldPlot", "A concurrent registration conflicted with this request.");
    public static readonly Error PlotNotFound = Error.NotFound("FieldPlot", "Field plot not found.");
    public static readonly Error ActiveCampaign = Error.Conflict("CropCampaign", "The plot already has a campaign in progress.");
    public static readonly Error CampaignNotFound = Error.NotFound("CropCampaign", "Crop campaign not found.");
    public static readonly Error CampaignFinished = Error.Conflict("CropCampaign", "The crop campaign is already finished.");
    public static readonly Error CampaignChanged = Error.Conflict("CropCampaign", "The crop campaign changed concurrently.");

    public static Error InvalidBoundary(string detail) => Error.Validation("PlotBoundary", detail);
}
