using SumaqAgro.Platform.Certification.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Certification.Domain.Model.Commands;
using SumaqAgro.Platform.Certification.Domain.Model.Queries;
using SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Certification.Application.CommandServices;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;
using SumaqAgro.Platform.Certification.Application.QueryServices;
using SumaqAgro.Platform.Shared.Interfaces.Rest;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest;

[ApiController]
[Route("api/v1/quality-certificates")]
public sealed class QualityCertificatesController(ICertificationService commands, ICertificationQueryService repository,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpPost]
    [ProducesResponseType<QualityCertificateResource>(201)]
    public async Task<IActionResult> Issue(IssueCertificate resource, CancellationToken ct) =>
        Respond(CertificationResourceAssembler.ToResource(await commands.IssueAsync(CertificationResourceAssembler.ToCommand(resource), ct), CertificationResourceAssembler.ToResource), 201);

    [HttpGet]
    [ProducesResponseType<QualityCertificateResource>(200)]
    public async Task<IActionResult> Get([FromQuery, Required] string certificateNumber, CancellationToken ct) =>
        Found((await repository.FindCertificateAsync(new GetQualityCertificateByNumberQuery(certificateNumber.Trim()), ct)) is { } model ? CertificationResourceAssembler.ToResource(model) : null, "QualityCertificate");

    [HttpPost("{qualityCertificateId:long}/revocation")]
    [ProducesResponseType<QualityCertificateResource>(200)]
    public async Task<IActionResult> Revoke(long qualityCertificateId, CancellationToken ct) =>
        Respond(CertificationResourceAssembler.ToResource(await commands.RevokeAsync(new RevokeQualityCertificateCommand(qualityCertificateId), ct), CertificationResourceAssembler.ToResource));
}
