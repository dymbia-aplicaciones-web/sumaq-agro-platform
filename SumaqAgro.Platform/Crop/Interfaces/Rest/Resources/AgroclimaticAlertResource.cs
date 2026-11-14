namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record AgroclimaticAlertResource(long Id, string Title, string Description, string Severity, string Region, DateTimeOffset IssuedAt, string Source);
