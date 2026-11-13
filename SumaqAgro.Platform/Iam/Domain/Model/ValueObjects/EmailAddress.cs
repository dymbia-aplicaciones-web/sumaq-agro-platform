namespace SumaqAgro.Platform.Iam.Domain.Model.ValueObjects;

public sealed record EmailAddress
{
    public EmailAddress(string value)
    {
        Value = value.Trim().ToLowerInvariant();
    }

    public string Value { get; }
}
