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
[Route("api/v1/satellite-observations")]
public sealed class SatelliteObservationsController(ICropQueryService repository) : ControllerBase
{
    [HttpGet]
    public async Task<SatelliteObservationResource[]> List([FromQuery, BindRequired, Range(1, long.MaxValue)] long plotId, CancellationToken ct) =>
        (await repository.ListObservationsAsync(new ListObservationsQuery(plotId), ct)).Select(CropResourceAssembler.ToResource).ToArray();
}
