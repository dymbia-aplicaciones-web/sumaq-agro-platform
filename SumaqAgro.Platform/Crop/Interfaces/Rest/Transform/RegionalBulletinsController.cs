using SumaqAgro.Platform.Crop.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Crop.Domain.Model.Querys;
using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SumaqAgro.Platform.Crop.Application.CommandServices;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Application.QueryServices;
using SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Interfaces.Rest;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Crop.Interfaces.Rest;

[ApiController]
[Route("api/v1/regional-bulletins")]
public sealed class RegionalBulletinsController(ICropCommandService commands, ICropQueryService repository,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpPost]
    [ProducesResponseType<RegionalBulletinResource>(201)]
    public async Task<IActionResult> Publish(PublishRegionalBulletinResource resource, CancellationToken ct) =>
        Respond(CropResourceAssembler.ToResource(await commands.PublishAsync(CropResourceAssembler.ToCommand(resource), ct), CropResourceAssembler.ToResource), 201);

    [HttpGet]
    public async Task<RegionalBulletinResource[]> List([FromQuery, BindRequired, Required] string region, CancellationToken ct) =>
        (await repository.ListBulletinsAsync(new ListBulletinsQuery(region.Trim()), ct)).Select(CropResourceAssembler.ToResource).ToArray();
}
