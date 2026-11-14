using System.Text.RegularExpressions;

namespace SumaqAgro.Platform.Certification.Domain.Model.ValueObjects;

public sealed record CertificateHash
{
    public const string Pattern = "^[0-9a-fA-F]{64}$";

    public CertificateHash(string value)
    {
        if (!Regex.IsMatch(value, Pattern))
            throw new ArgumentException("Expected a SHA-256 hexadecimal hash.", nameof(value));
        Value = value;
    }

    public string Value { get; }
}