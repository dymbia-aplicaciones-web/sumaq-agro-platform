using SumaqAgro.Platform.Certification.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Certification.Domain.Model.Commands;
using SumaqAgro.Platform.Certification.Domain.Model.Queries;
using SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.WebUtilities;
using SumaqAgro.Platform.Certification.Infrastructure.Documents;
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

    [HttpGet("batch/{batchId:long}")]
    public async Task<IActionResult> GetByBatch(long batchId, CancellationToken ct) =>
        Found((await repository.FindCertificateByBatchAsync(batchId, ct)) is { } certificate
            ? CertificationResourceAssembler.ToResource(certificate) : null, "QualityCertificate");

    [HttpGet("{qualityCertificateId:long}/document")]
    [Produces("application/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(long qualityCertificateId, [FromServices] CertificatePdf pdf,
        [FromServices] IConfiguration configuration, CancellationToken ct)
    {
        var certificate = await repository.FindCertificateAsync(qualityCertificateId, ct);
        if (certificate is null)
            return Found<QualityCertificate>(null, "QualityCertificate");

        var batch = await repository.FindBatchAsync(new GetHarvestBatchByIdQuery(certificate.HarvestBatchId), ct);
        if (batch is null)
            return Found<HarvestBatch>(null, "HarvestBatch");

        var origin = configuration["Certification:PublicBaseUrl"] ?? "http://localhost:5192";
        var verificationUrl = QueryHelpers.AddQueryString(
            origin.TrimEnd('/') + "/api/v1/public/traceability",
            new Dictionary<string, string?>
            {
                ["number"] = certificate.CertificateNumber,
                ["hash"] = certificate.CertificateHash
            });
        return File(pdf.Render(certificate, batch, verificationUrl), "application/pdf",
            $"sumaqagro-certificate-{certificate.Id}.pdf");
    }

    [HttpPost("{qualityCertificateId:long}/revocation")]
    [ProducesResponseType<QualityCertificateResource>(200)]
    public async Task<IActionResult> Revoke(long qualityCertificateId, CancellationToken ct) =>
        Respond(CertificationResourceAssembler.ToResource(await commands.RevokeAsync(new RevokeQualityCertificateCommand(qualityCertificateId), ct), CertificationResourceAssembler.ToResource));
}
