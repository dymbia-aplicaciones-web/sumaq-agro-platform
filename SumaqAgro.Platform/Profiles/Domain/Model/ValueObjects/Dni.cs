namespace SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;

public sealed record Dni
{
    public const string Pattern = @"^\s*[0-9]{8}\s*$";

    public Dni(string value)
    {
        var normalized = value.Trim();
        if (normalized.Length != 8 || !normalized.All(char.IsAsciiDigit))
            throw new ArgumentException("Expected exactly 8 digits.", nameof(value));
        Value = normalized;
    }

    public string Value { get; }
}
