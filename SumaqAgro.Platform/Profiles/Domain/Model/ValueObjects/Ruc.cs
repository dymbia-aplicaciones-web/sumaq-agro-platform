namespace SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;

public sealed record Ruc
{
    public const string Pattern = @"^\s*[0-9]{11}\s*$";

    public Ruc(string value)
    {
        var normalized = value.Trim();
        if (normalized.Length != 11 || !normalized.All(char.IsAsciiDigit))
            throw new ArgumentException("Expected exactly 11 digits.", nameof(value));
        Value = normalized;
    }

    public string Value { get; }
}
