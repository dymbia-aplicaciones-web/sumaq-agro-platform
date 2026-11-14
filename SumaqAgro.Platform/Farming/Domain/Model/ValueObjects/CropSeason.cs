using System.Text.RegularExpressions;

namespace SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

public sealed record CropSeason
{
    public const string Pattern = "^[0-9]{4}-(I|II)$";

    public CropSeason(string value)
    {
        if (!Regex.IsMatch(value, Pattern))
            throw new ArgumentException("Season must use YYYY-I or YYYY-II.", nameof(value));

        Value = value;
    }

    public string Value { get; }
}
