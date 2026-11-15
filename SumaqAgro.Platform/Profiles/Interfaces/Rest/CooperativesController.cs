using SumaqAgro.Platform.Profiles.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Profiles.Application.QueryServices;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;
using SumaqAgro.Platform.Profiles.Application.CommandServices;
using SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/cooperatives")]
public sealed class CooperativesController(IProfilesCommandService commands, IProfilesQueryService queries, ProblemDetailsFactory problems) : ControllerBase
{
    [HttpGet]
    public async Task<CooperativeResource[]> List(CancellationToken ct) =>
        (await queries.ListCooperativesAsync(ct)).Select(ProfilesResourceAssembler.ToResource).ToArray();

    [HttpGet("{cooperativeId:long}")]
    public async Task<IActionResult> Get(long cooperativeId, CancellationToken ct)
    {
        var cooperative = await queries.FindCooperativeAsync(cooperativeId, ct);
        return cooperative is null ? NotFound() : new JsonResult(ProfilesResourceAssembler.ToResource(cooperative));
    }

    [HttpGet("{cooperativeId:long}/members")]

    public async Task<IActionResult> ListMembers(long cooperativeId, CancellationToken ct)
    {
        if (cooperativeId <= 0) return BadRequest();
        if (await queries.FindCooperativeAsync(cooperativeId, ct) is null) return NotFound();
        return new JsonResult((await queries.ListMembersAsync(cooperativeId, ct))
            .Select(ProfilesResourceAssembler.ToResource).ToArray());
    }

    [HttpGet("/api/v1/cooperative-members")]
    public async Task<IActionResult> SearchMembers([FromQuery] long? cooperativeId, [FromQuery] long? userId, CancellationToken ct)
    {
        if (cooperativeId is > 0 && userId is null) return await ListMembers(cooperativeId.Value, ct);
        if (userId is > 0 && cooperativeId is null)
            return new JsonResult((await queries.ListMembershipsAsync(userId.Value, ct))
                .Select(ProfilesResourceAssembler.ToResource).ToArray());
        return BadRequest();
    }


    [HttpPut("{cooperativeId:long}")]
    public async Task<IActionResult> Update(long cooperativeId, RegisterCooperativeResource resource, CancellationToken ct)
    {
        var result = await commands.UpdateCooperativeAsync(cooperativeId, ProfilesResourceAssembler.ToCommand(resource), ct);
        return result.Match<IActionResult>(
            cooperative => new JsonResult(ProfilesResourceAssembler.ToResource(cooperative)),
            error => problems.CreateProblemDetails(HttpContext, error));
    }

    [HttpPost]
    [ProducesResponseType<CooperativeResource>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterCooperativeResource resource, CancellationToken cancellationToken)
    {
        var cooperative = await commands.RegisterAsync(ProfilesResourceAssembler.ToCommand(resource), cancellationToken);
        return cooperative.IsFailure ? BadRequest() : new JsonResult(ProfilesResourceAssembler.ToResource(cooperative.Value!)) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPost("{cooperativeId:long}/members")]
    [ProducesResponseType<CooperativeMemberResource>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMember(long cooperativeId, AddCooperativeMemberResource resource,
        CancellationToken cancellationToken)
    {
        var member = await commands.AddMemberAsync(ProfilesResourceAssembler.ToCommand(cooperativeId, resource), cancellationToken);
        return member.IsFailure ? BadRequest() : new JsonResult(ProfilesResourceAssembler.ToResource(member.Value!)) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPost("{cooperativeId:long}/invitations")]
    [ProducesResponseType<AgronomistInvitationResource>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Invite(long cooperativeId, InviteAgronomistResource resource,
        CancellationToken cancellationToken)
    {
        var invitation = await commands.InviteAsync(ProfilesResourceAssembler.ToCommand(cooperativeId, resource), cancellationToken);
        return invitation.IsFailure ? BadRequest() : new JsonResult(ProfilesResourceAssembler.ToResource(invitation.Value!)) { StatusCode = StatusCodes.Status201Created };
    }
}
