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
[Route("api/v1/climate-forecasts")]
public sealed class ClimateForecastsController(ICropQueryService repository) : ControllerBase
{
    [HttpGet]
    public async Task<ClimateForecastResource[]> List([FromQuery, BindRequired, Required] string region, CancellationToken ct) =>
        (await repository.ListForecastsAsync(new ListForecastsQuery(region.Trim()), ct)).Select(CropResourceAssembler.ToResource).ToArray();
}
