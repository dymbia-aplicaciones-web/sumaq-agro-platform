using SumaqAgro.Platform.Profiles.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Profiles.Application.CommandServices;
using SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/cooperatives")]
public sealed class CooperativesController(IProfilesCommandService commands) : ControllerBase
{
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
