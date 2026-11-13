using SumaqAgro.Platform.Profiles.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Profiles.Domain.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Profiles.Application.CommandServices;
using SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Profiles.Application.QueryServices;

namespace SumaqAgro.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/profiles/user/{userId:long}")]
public sealed class ProfilesController(IProfilesCommandService commands, IProfilesQueryService queries) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<ProfileResource>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long userId, CancellationToken cancellationToken)
    {
        var profile = await queries.FindProfileAsync(new GetProfileByUserIdQuery(userId), cancellationToken);
        return profile is null ? NotFound() : new JsonResult(ProfilesResourceAssembler.ToResource(profile));
    }

    [HttpPut]
    [ProducesResponseType<ProfileResource>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long userId, UpdateProfileResource resource, CancellationToken cancellationToken)
    {
        var profile = await commands.UpdateAsync(ProfilesResourceAssembler.ToCommand(userId, resource), cancellationToken);
        return profile.IsFailure ? BadRequest() : new JsonResult(ProfilesResourceAssembler.ToResource(profile.Value!));
    }
}
