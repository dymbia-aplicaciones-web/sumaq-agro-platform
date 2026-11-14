namespace SumaqAgro.Platform.Crop.Domain.Model.Commands;

public sealed record PublishCommand(long AgronomistId, string Region, string Title, string Content);
