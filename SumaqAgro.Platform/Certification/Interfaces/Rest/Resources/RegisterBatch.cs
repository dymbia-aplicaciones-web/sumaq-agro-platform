using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;

public sealed record RegisterBatch([param: Required, StringLength(40)] string BatchCode,
    [param: Range(1, long.MaxValue)] long CooperativeId, [param: Range(1, long.MaxValue)] long MemberId,
    [param: Range(1, long.MaxValue)] long PlotId,
    [param: Required, RegularExpression(@"(?i)^\s*(ANDEAN_POTATO|SPECIALTY_COFFEE)\s*$")] string CropType,
    [param: Required] DateOnly? HarvestedOn);