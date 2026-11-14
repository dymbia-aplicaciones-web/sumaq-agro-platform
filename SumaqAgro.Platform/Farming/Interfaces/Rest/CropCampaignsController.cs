using SumaqAgro.Platform.Farming.Domain.Model.Queries;
using SumaqAgro.Platform.Farming.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Farming.Application.CommandServices;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Farming.Application.QueryServices;
using SumaqAgro.Platform.Farming.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Interfaces.Rest;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Farming.Interfaces.Rest;

[ApiController]
[Route("api/v1/crop-campaigns")]
public sealed class CropCampaignsController(IFarmingCommandService commands, IFarmingQueryService repository,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpPost]
    [ProducesResponseType<CropCampaignResource>(201)]
    public async Task<IActionResult> Start(StartCropCampaignResource resource, CancellationToken ct) =>
        Respond(CropCampaignAssembler.ToResource(await commands.StartAsync(CropCampaignAssembler.ToCommand(resource), ct)), 201);

    [HttpGet("{cropCampaignId:long}")]
    [ProducesResponseType<CropCampaignResource>(200)]
    public async Task<IActionResult> Get(long cropCampaignId, CancellationToken ct) =>
        Found((await repository.FindCampaignAsync(new GetCropCampaignByIdQuery(cropCampaignId), ct)) is { } campaign
            ? CropCampaignAssembler.ToResource(campaign) : null, "CropCampaign");

    [HttpPut("{cropCampaignId:long}/crop-type")]
    [ProducesResponseType<CropCampaignResource>(200)]
    public async Task<IActionResult> SelectCrop(long cropCampaignId, SelectCropTypeResource resource, CancellationToken ct) =>
        Respond(CropCampaignAssembler.ToResource(await commands.SelectCropTypeAsync(new(cropCampaignId, resource.CropType.Trim().ToUpperInvariant()), ct)));

    [HttpPut("{cropCampaignId:long}/seed-variety")]
    [ProducesResponseType<CropCampaignResource>(200)]
    public async Task<IActionResult> SpecifySeed(long cropCampaignId, SpecifySeedVarietyResource resource, CancellationToken ct) =>
        Respond(CropCampaignAssembler.ToResource(await commands.SpecifySeedVarietyAsync(new(cropCampaignId, resource.SeedVariety.Trim(), resource.CustomSeedVariety), ct)));

    [HttpPut("{cropCampaignId:long}/sowing-date")]
    [ProducesResponseType<CropCampaignResource>(200)]
    public async Task<IActionResult> RecordSowing(long cropCampaignId, RecordSowingDateResource resource, CancellationToken ct) =>
        Respond(CropCampaignAssembler.ToResource(await commands.RecordSowingDateAsync(new(cropCampaignId, resource.SowingDate!.Value), ct)));

    [HttpPost("{cropCampaignId:long}/closures")]
    public async Task<IActionResult> Close(long cropCampaignId, CancellationToken ct)
    {
        var result = await commands.CloseCropCampaignAsync(new(cropCampaignId), ct);
        return Respond(result.Match(
            _ => Result<ClosureResponse>.Success(new("Crop campaign closed successfully")),
            Result<ClosureResponse>.Failure));
    }
}
