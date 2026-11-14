using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;

public sealed record WeighBatch([param: Required, Range(typeof(decimal), "0", "999999999.999")] decimal? GrossWeightKg,
    [param: Required, Range(typeof(decimal), "0", "999999999.999")] decimal? TareKg);