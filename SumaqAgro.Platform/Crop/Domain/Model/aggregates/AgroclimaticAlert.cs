namespace SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

public sealed class AgroclimaticAlert(string title, string description, string severity, string region, DateTimeOffset issuedAt, string source)
{
    public long Id { get; private set; }
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public string Severity { get; private set; } = severity;
    public string Region { get; private set; } = region;
    public DateTimeOffset IssuedAt { get; private set; } = issuedAt;
    public string Source { get; private set; } = source;
}
