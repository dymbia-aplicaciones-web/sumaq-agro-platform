using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.Commands;
using SumaqAgro.Platform.Farming.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Application.Model;

namespace SumaqAgro.Platform.Farming.Interfaces.Rest.Transform;

internal static class CropCampaignAssembler
{
    public static StartCropCampaignCommand ToCommand(StartCropCampaignResource resource) =>
        new(resource.FieldPlotId, new CropSeason(resource.Season), resource.CropType.Trim().ToUpperInvariant(),
            resource.SeedVariety.Trim(), resource.CustomSeedVariety, resource.SowingDate);

    public static CropCampaignResource ToResource(CropCampaign campaign) =>
        new(campaign.Id, campaign.FieldPlotId, campaign.Season, campaign.CropType,
            campaign.SeedVariety, campaign.CustomSeedVariety, campaign.SowingDate, campaign.Status);

    public static Result<CropCampaignResource> ToResource(Result<CropCampaign> result) =>
        result.Match(campaign => Result<CropCampaignResource>.Success(ToResource(campaign)),
            Result<CropCampaignResource>.Failure);
}
