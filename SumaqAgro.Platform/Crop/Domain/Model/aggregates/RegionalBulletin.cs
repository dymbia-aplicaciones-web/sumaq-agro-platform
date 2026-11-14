namespace SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

public sealed class RegionalBulletin(long agronomistId, string region, string title, string content, DateTimeOffset publishedAt)
{
    public long Id { get; private set; }
    public long AgronomistId { get; private set; } = agronomistId;
    public string Region { get; private set; } = region;
    public string Title { get; private set; } = title;
    public string Content { get; private set; } = content;
    public DateTimeOffset PublishedAt { get; private set; } = publishedAt;
}
