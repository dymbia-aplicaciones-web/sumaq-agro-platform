using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;

public sealed record CupCoffee([param: Required, Range(0, 100)] decimal? Aroma,
    [param: Required, Range(0, 100)] decimal? Flavor, [param: Required, Range(0, 100)] decimal? Acidity,
    [param: Required, Range(0, 100)] decimal? Body, [param: Required, Range(0, 100)] decimal? TotalScore,
    bool FermentDefect);