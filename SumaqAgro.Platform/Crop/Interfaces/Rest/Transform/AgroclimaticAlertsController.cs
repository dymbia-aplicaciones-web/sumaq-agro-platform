using SumaqAgro.Platform.Crop.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Crop.Domain.Model.Querys;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Application.QueryServices;

namespace SumaqAgro.Platform.Crop.Interfaces.Rest;

[ApiController]
[Route("api/v1/agroclimatic-alerts")]
public sealed class AgroclimaticAlertsController(ICropQueryService repository) : ControllerBase
{
    [HttpGet]
    public async Task<AgroclimaticAlertResource[]> List([FromQuery, BindRequired, Required] string region, CancellationToken ct) =>
        (await repository.ListAlertsAsync(new ListAlertsQuery(region.Trim()), ct)).Select(CropResourceAssembler.ToResource).ToArray();
}
