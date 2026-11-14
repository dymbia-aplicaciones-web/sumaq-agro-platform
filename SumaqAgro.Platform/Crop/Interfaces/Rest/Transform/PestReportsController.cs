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
[Route("api/v1/pest-reports")]
public sealed class PestReportsController(ICropCommandService commands, ICropQueryService repository,
    ProblemDetailsFactory problems) : ResultController(problems)
{
    [HttpPost]
    [ProducesResponseType<PestReportResource>(201)]
    public async Task<IActionResult> Record(RecordPestReportResource resource, CancellationToken ct) =>
        Respond(CropResourceAssembler.ToResource(await commands.ReportAsync(CropResourceAssembler.ToCommand(resource), ct), CropResourceAssembler.ToResource), 201);

    [HttpGet("{pestReportId:long}")]
    [ProducesResponseType<PestReportResource>(200)]
    public async Task<IActionResult> Get(long pestReportId, CancellationToken ct) =>
        Found((await repository.FindReportAsync(new FindReportQuery(pestReportId), ct)) is { } model ? CropResourceAssembler.ToResource(model) : null, "PestReport");

    [HttpGet]
    public async Task<PestReportResource[]> List([FromQuery, BindRequired, Range(1, long.MaxValue)] long plotId, CancellationToken ct) =>
        (await repository.ListReportsAsync(new ListReportsQuery(plotId), ct)).Select(CropResourceAssembler.ToResource).ToArray();

    [HttpDelete("{pestReportId:long}")]
    public async Task<IActionResult> Delete(long pestReportId, CancellationToken ct)
    {
        var result = await commands.DeleteReportAsync(new DeleteReportCommand(pestReportId), ct);
        return Respond(result.Match(
            message => Result<DeletionResponse>.Success(new(message)), Result<DeletionResponse>.Failure));
    }
}
