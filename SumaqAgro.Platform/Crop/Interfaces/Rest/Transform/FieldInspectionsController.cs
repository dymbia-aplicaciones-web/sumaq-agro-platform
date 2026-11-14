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
[Route("api/v1/field-inspections")]
public sealed class FieldInspectionsController(ICropCommandService commands, ICropQueryService repository,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpPost]
    [ProducesResponseType<FieldInspectionResource>(201)]
    public async Task<IActionResult> Schedule(ScheduleFieldInspectionResource resource, CancellationToken ct) =>
        Respond(CropResourceAssembler.ToResource(await commands.ScheduleAsync(CropResourceAssembler.ToCommand(resource), ct), CropResourceAssembler.ToResource), 201);

    [HttpGet("{fieldInspectionId:long}")]
    [ProducesResponseType<FieldInspectionResource>(200)]
    public async Task<IActionResult> Get(long fieldInspectionId, CancellationToken ct) =>
        Found((await repository.FindInspectionAsync(new FindInspectionQuery(fieldInspectionId), ct)) is { } model ? CropResourceAssembler.ToResource(model) : null, "FieldInspection");

    [HttpGet]
    public async Task<FieldInspectionResource[]> List([FromQuery, BindRequired, Range(1, long.MaxValue)] long plotId, CancellationToken ct) =>
        (await repository.ListInspectionsAsync(new ListInspectionsQuery(plotId), ct)).Select(CropResourceAssembler.ToResource).ToArray();

    [HttpPatch("{fieldInspectionId:long}/completion")]
    [ProducesResponseType<FieldInspectionResource>(200)]
    public async Task<IActionResult> Complete(long fieldInspectionId, CompleteFieldInspectionResource resource, CancellationToken ct) =>
        Respond(CropResourceAssembler.ToResource(await commands.CompleteAsync(CropResourceAssembler.ToCommand(fieldInspectionId, resource), ct), CropResourceAssembler.ToResource));
}
