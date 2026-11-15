using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record ImportSatelliteObservationResource(
    [param: Range(1, long.MaxValue)] long PlotId,
    [param: Required] DateTimeOffset? Start,
    [param: Required] DateTimeOffset? End);
