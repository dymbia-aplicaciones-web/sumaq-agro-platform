using SumaqAgro.Platform.Farming.Domain.Model.Commands;
using SumaqAgro.Platform.Farming.Domain.Model.Queries;
using SumaqAgro.Platform.Farming.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SumaqAgro.Platform.Farming.Application.CommandServices;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Farming.Application.QueryServices;
using SumaqAgro.Platform.Farming.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Interfaces.Rest;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Farming.Interfaces.Rest;

[ApiController]
[Route("api/v1/field-plots")]
public sealed class FieldPlotsController(IFarmingCommandService commands, IFarmingQueryService repository,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpPost]
    [ProducesResponseType<FieldPlotResource>(201)]
    public async Task<IActionResult> Register(RegisterFieldPlotResource resource, CancellationToken ct) =>
        Respond((await commands.RegisterAsync(FieldPlotAssembler.ToCommand(resource), ct)).Match(
                plot => Shared.Application.Model.Result<FieldPlotResource>.Success(FieldPlotAssembler.ToResource(plot)),
                Shared.Application.Model.Result<FieldPlotResource>.Failure), 201);

    [HttpGet("{fieldPlotId:long}")]
    [ProducesResponseType<FieldPlotResource>(200)]
    public async Task<IActionResult> Get(long fieldPlotId, CancellationToken ct)
    {
        var plot = await repository.FindPlotAsync(new GetFieldPlotByIdQuery(fieldPlotId), ct);
        return Found(plot is null ? null : FieldPlotAssembler.ToResource(plot), "FieldPlot");
    }

    [HttpGet]
    public async Task<FieldPlotResource[]> List([FromQuery, BindRequired] long ownerUserId, CancellationToken ct) =>
        (await repository.ListPlotsAsync(new GetFieldPlotsByOwnerQuery(ownerUserId), ct)).Select(FieldPlotAssembler.ToResource).ToArray();

    [HttpPut("{fieldPlotId:long}/boundary")]
    [ProducesResponseType<FieldPlotResource>(200)]
    public async Task<IActionResult> Delineate(long fieldPlotId, DelineatePlotBoundaryResource resource, CancellationToken ct)
    {
        if (resource.Vertices.Any(point => point?.Latitude is null || point.Longitude is null))
            return BadRequest();

        var vertices = resource.Vertices.Select(point =>
            new GeoCoordinate(point!.Latitude!.Value, point.Longitude!.Value)).ToArray();
        return Respond((await commands.DelineateAsync(new DelineateFieldPlotCommand(fieldPlotId, vertices), ct)).Match(
            plot => Shared.Application.Model.Result<FieldPlotResource>.Success(FieldPlotAssembler.ToResource(plot)),
            Shared.Application.Model.Result<FieldPlotResource>.Failure));
    }

    [HttpGet("{fieldPlotId:long}/crop-campaigns")]
    public async Task<CropCampaignResource[]> ListCampaigns(long fieldPlotId, CancellationToken ct) =>
        (await repository.ListCampaignsAsync(new GetCropCampaignsByPlotQuery(fieldPlotId), ct))
            .Select(CropCampaignAssembler.ToResource).ToArray();
}
