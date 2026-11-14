using SumaqAgro.Platform.Certification.Domain.Model.Queries;
using SumaqAgro.Platform.Certification.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Certification.Interfaces.Rest.Transform;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Certification.Application.QueryServices;
using SumaqAgro.Platform.Farming.Application.QueryServices;
using SumaqAgro.Platform.Shared.Interfaces.Rest;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest;

[ApiController]
[AllowAnonymous]
[Route("api/v1/public/traceability")]
public sealed class PublicTraceabilityController(ITraceabilityQueryService traceability,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpGet]
    [ProducesResponseType<TraceabilityResource>(200)]
    public async Task<IActionResult> Verify([FromQuery, Required] string number,
        [FromQuery, Required, RegularExpression(CertificateHash.Pattern)] string hash, CancellationToken ct)
    {
        return Respond(CertificationResourceAssembler.ToResource(await traceability.VerifyAsync(new VerifyCertificateTraceabilityQuery(number, new CertificateHash(hash)), ct), CertificationResourceAssembler.ToResource));
    }
}

