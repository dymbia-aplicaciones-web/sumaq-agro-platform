namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record RegionalBulletinResource(long Id, long AgronomistId, string Region, string Title, string Content, DateTimeOffset PublishedAt);
