using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;

public sealed record ExtractSample([param: Required, StringLength(40)] string SampleIdentifier,
    [param: Required] decimal? SampleWeightKg);