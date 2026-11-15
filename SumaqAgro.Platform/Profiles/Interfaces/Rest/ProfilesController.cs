using SumaqAgro.Platform.Profiles.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Profiles.Domain.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Profiles.Application.CommandServices;
using SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Profiles.Application.QueryServices;

namespace SumaqAgro.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/profiles")]
public sealed class ProfilesController(IProfilesCommandService commands, IProfilesQueryService queries) : ControllerBase
{
    [HttpGet]
    public async Task<ProfileResource[]> List([FromQuery] long? userId, CancellationToken ct) =>
        (await queries.ListProfilesAsync(userId, ct)).Select(ProfilesResourceAssembler.ToResource).ToArray();

    [HttpGet("{profileId:long}")]
    public async Task<IActionResult> GetById(long profileId, CancellationToken ct)
    {
        var profile = await queries.FindProfileByIdAsync(profileId, ct);
        return profile is null ? NotFound() : new JsonResult(ProfilesResourceAssembler.ToResource(profile));
    }

    [HttpPut("{profileId:long}")]
    public async Task<IActionResult> UpdateById(long profileId, UpdateProfileResource resource, CancellationToken ct)
    {
        var profile = await queries.FindProfileByIdAsync(profileId, ct);
        return profile is null ? NotFound() : await Update(profile.UserId, resource, ct);
    }

    [HttpGet("user/{userId:long}")]
    [ProducesResponseType<ProfileResource>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long userId, CancellationToken cancellationToken)
    {
        var profile = await queries.FindProfileAsync(new GetProfileByUserIdQuery(userId), cancellationToken);
        return profile is null ? NotFound() : new JsonResult(ProfilesResourceAssembler.ToResource(profile));
    }

    [HttpPut("user/{userId:long}")]
    [ProducesResponseType<ProfileResource>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long userId, UpdateProfileResource resource, CancellationToken cancellationToken)
    {
        var profile = await commands.UpdateAsync(ProfilesResourceAssembler.ToCommand(userId, resource), cancellationToken);
        return profile.IsFailure ? BadRequest() : new JsonResult(ProfilesResourceAssembler.ToResource(profile.Value!));
    }
}
