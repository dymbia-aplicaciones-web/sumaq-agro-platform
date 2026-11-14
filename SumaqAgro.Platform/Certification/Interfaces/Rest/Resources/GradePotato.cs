using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;

public sealed record GradePotato([param: Required, Range(0, 100)] decimal? FirstPercentage,
    [param: Required, Range(0, 100)] decimal? SecondPercentage,
    [param: Required, Range(0, 100)] decimal? ThirdPercentage,
    [param: Required, Range(0, 100)] decimal? PestDamagePercentage);