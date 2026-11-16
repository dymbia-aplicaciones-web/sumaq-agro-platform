using SumaqAgro.Platform.Crop.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Crop.Domain.Model.Querys;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using SumaqAgro.Platform.Crop.Application.CommandServices;
using SumaqAgro.Platform.Crop.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Application.QueryServices;

namespace SumaqAgro.Platform.Crop.Interfaces.Rest;

[ApiController]
[Route("api/v1/satellite-observations")]
public sealed class SatelliteObservationsController(ICropQueryService repository, ISatelliteObservationCommandService commands, ICropAccounts users, ProblemDetailsFactory problems) : ControllerBase
{
    [HttpPost("import")]
    public async Task<IActionResult> Import(ImportSatelliteObservationResource resource, CancellationToken ct)
    {
        var email = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        var userId = await users.FindIdAsync(email, ct);
        if (userId is null) return Unauthorized();

        var result = await commands.ImportAsync(new(resource.PlotId, userId.Value, resource.Start!.Value, resource.End!.Value), ct);
        return result.Match<IActionResult>(observation => Ok(CropResourceAssembler.ToResource(observation)),
            error => problems.CreateProblemDetails(HttpContext, error));
    }

    [HttpGet]
    public async Task<SatelliteObservationResource[]> List([FromQuery, BindRequired, Range(1, long.MaxValue)] long plotId, CancellationToken ct) =>
        (await repository.ListObservationsAsync(new ListObservationsQuery(plotId), ct)).Select(CropResourceAssembler.ToResource).ToArray();
}
