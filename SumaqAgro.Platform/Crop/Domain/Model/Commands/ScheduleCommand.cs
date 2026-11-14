namespace SumaqAgro.Platform.Crop.Domain.Model.Commands;

public sealed record ScheduleCommand(long ReportId, long AgronomistId, DateTimeOffset ScheduledAt);
