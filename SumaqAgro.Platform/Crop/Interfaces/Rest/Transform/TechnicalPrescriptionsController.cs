using SumaqAgro.Platform.Crop.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Crop.Domain.Model.Querys;
using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SumaqAgro.Platform.Crop.Application.CommandServices;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Application.QueryServices;
using SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Interfaces.Rest;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Crop.Interfaces.Rest;

[ApiController]
[Route("api/v1/technical-prescriptions")]
public sealed class TechnicalPrescriptionsController(ICropCommandService commands, ICropQueryService repository,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpPost]
    [ProducesResponseType<TechnicalPrescriptionResource>(201)]
    public async Task<IActionResult> Issue(IssueTechnicalPrescriptionResource resource, CancellationToken ct) =>
        Respond(CropResourceAssembler.ToResource(await commands.IssueAsync(CropResourceAssembler.ToCommand(resource), ct), CropResourceAssembler.ToResource), 201);

    [HttpGet("{technicalPrescriptionId:long}")]
    [ProducesResponseType<TechnicalPrescriptionResource>(200)]
    public async Task<IActionResult> Get(long technicalPrescriptionId, CancellationToken ct) =>
        Found((await repository.FindPrescriptionAsync(new FindPrescriptionQuery(technicalPrescriptionId), ct)) is { } model ? CropResourceAssembler.ToResource(model) : null, "TechnicalPrescription");

    [HttpGet]
    public async Task<TechnicalPrescriptionResource[]> List([FromQuery, BindRequired, Range(1, long.MaxValue)] long plotId, CancellationToken ct) =>
        (await repository.ListPrescriptionsAsync(new ListPrescriptionsQuery(plotId), ct)).Select(CropResourceAssembler.ToResource).ToArray();

    [HttpPatch("{technicalPrescriptionId:long}/application")]
    [ProducesResponseType<TechnicalPrescriptionResource>(200)]
    public async Task<IActionResult> Apply(long technicalPrescriptionId, CancellationToken ct) =>
        Respond(CropResourceAssembler.ToResource(await commands.RegisterApplicationAsync(new RegisterApplicationCommand(technicalPrescriptionId), ct), CropResourceAssembler.ToResource));
}
