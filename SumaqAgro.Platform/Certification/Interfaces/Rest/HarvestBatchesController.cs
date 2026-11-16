using SumaqAgro.Platform.Certification.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Certification.Domain.Model.Queries;
using SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;
using SumaqAgro.Platform.Iam.Interfaces.Acl;
using SumaqAgro.Platform.Profiles.Application.QueryServices;
using SumaqAgro.Platform.Certification.Application.CommandServices;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;
using SumaqAgro.Platform.Certification.Application.QueryServices;
using SumaqAgro.Platform.Shared.Interfaces.Rest;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest;

[ApiController]
[Route("api/v1/harvest-batches")]
public sealed class HarvestBatchesController(ICertificationService commands, ICertificationQueryService repository,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery, BindRequired] long cooperativeId, [FromQuery] long? memberId,
        [FromServices] IIamAccounts accounts, [FromServices] IProfilesQueryService profiles, CancellationToken ct)
    {
        var email = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrWhiteSpace(email) || await accounts.FindAsync(email, ct) is not { } account)
            return Unauthorized();

        if (account.Role == "ROLE_COOPERATIVE_DIRECTOR")
        {
            var cooperative = await profiles.FindCooperativeAsync(cooperativeId, ct);
            if (cooperative?.ManagerUserId != account.Id) return Forbid();
        }
        else if (account.Role != "ROLE_FARMER" || memberId is null ||
                 !(await profiles.ListMembershipsAsync(account.Id, ct))
                     .Any(member => member.Id == memberId && member.CooperativeId == cooperativeId))
        {
            return Forbid();
        }

        var batches = await repository.ListBatchesAsync(cooperativeId, memberId, ct);
        return Ok(batches.Select(CertificationResourceAssembler.ToResource).ToArray());
    }

    [HttpPost]
    [ProducesResponseType<HarvestBatchResource>(201)]
    public async Task<IActionResult> Register(RegisterBatch resource, CancellationToken ct) =>
        Respond(CertificationResourceAssembler.ToResource(await commands.RegisterAsync(CertificationResourceAssembler.ToCommand(resource), ct), CertificationResourceAssembler.ToResource), 201);

    [HttpGet("{harvestBatchId:long}")]
    [ProducesResponseType<HarvestBatchResource>(200)]
    public async Task<IActionResult> Get(long harvestBatchId, CancellationToken ct) =>
        Found((await repository.FindBatchAsync(new GetHarvestBatchByIdQuery(harvestBatchId), ct)) is { } model ? CertificationResourceAssembler.ToResource(model) : null, "HarvestBatch");

    [HttpPost("{harvestBatchId:long}/weight")]
    [ProducesResponseType<HarvestBatchResource>(200)]
    public async Task<IActionResult> Weigh(long harvestBatchId, WeighBatch resource, CancellationToken ct) =>
        Respond(CertificationResourceAssembler.ToResource(await commands.WeighAsync(CertificationResourceAssembler.ToCommand(harvestBatchId, resource), ct), CertificationResourceAssembler.ToResource));

    [HttpPost("{harvestBatchId:long}/sample")]
    [ProducesResponseType<HarvestBatchResource>(200)]
    public async Task<IActionResult> Sample(long harvestBatchId, ExtractSample resource, CancellationToken ct) =>
        Respond(CertificationResourceAssembler.ToResource(await commands.SampleAsync(CertificationResourceAssembler.ToCommand(harvestBatchId, resource), ct), CertificationResourceAssembler.ToResource));

    [HttpPost("{harvestBatchId:long}/potato-calibers")]
    [ProducesResponseType<HarvestBatchResource>(200)]
    public async Task<IActionResult> Grade(long harvestBatchId, GradePotato resource, CancellationToken ct) =>
        Respond(CertificationResourceAssembler.ToResource(await commands.GradeAsync(CertificationResourceAssembler.ToCommand(harvestBatchId, resource), ct), CertificationResourceAssembler.ToResource));

    [HttpPost("{harvestBatchId:long}/coffee-cupping")]
    [ProducesResponseType<HarvestBatchResource>(200)]
    public async Task<IActionResult> Cup(long harvestBatchId, CupCoffee resource, CancellationToken ct) =>
        Respond(CertificationResourceAssembler.ToResource(await commands.CupAsync(CertificationResourceAssembler.ToCommand(harvestBatchId, resource), ct), CertificationResourceAssembler.ToResource));
}
