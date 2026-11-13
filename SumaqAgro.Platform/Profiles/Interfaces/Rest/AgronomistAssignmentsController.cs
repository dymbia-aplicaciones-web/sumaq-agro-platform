using SumaqAgro.Platform.Profiles.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Profiles.Domain.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Profiles.Application.CommandServices;
using SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SumaqAgro.Platform.Profiles.Application.QueryServices;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/agronomist-assignments")]
public sealed class AgronomistAssignmentsController(IProfilesCommandService commands, IProfilesQueryService queries)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<AgronomistAssignmentResource>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Assign(AssignAgronomistResource resource, CancellationToken cancellationToken)
    {
        var assignment = await commands.AssignAsync(ProfilesResourceAssembler.ToCommand(resource), cancellationToken);
        return assignment.IsFailure ? BadRequest() : new JsonResult(ProfilesResourceAssembler.ToResource(assignment.Value!)) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpGet("plot/{plotId:long}")]
    [ProducesResponseType<AgronomistAssignmentResource>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPlot(long plotId, CancellationToken cancellationToken)
    {
        var assignment = await queries.FindActiveAssignmentAsync(new GetActiveAssignmentByPlotQuery(plotId), cancellationToken);
        return assignment is null ? NotFound() : new JsonResult(ProfilesResourceAssembler.ToResource(assignment));
    }

    [HttpGet]
    [ProducesResponseType<AgronomistAssignmentResource[]>(StatusCodes.Status200OK, "application/json")]
    public async Task<IActionResult> GetByCooperative([FromQuery, BindRequired] long cooperativeId,
        CancellationToken cancellationToken) =>
        new JsonResult((await queries.ListAssignmentsAsync(new GetAssignmentsByCooperativeQuery(cooperativeId), cancellationToken)).Select(ProfilesResourceAssembler.ToResource).ToArray());
}
