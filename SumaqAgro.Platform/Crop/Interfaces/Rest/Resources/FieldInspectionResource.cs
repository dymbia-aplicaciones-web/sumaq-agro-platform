namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record FieldInspectionResource(long Id, long PestReportId, long PlotId, long AgronomistId, DateTimeOffset ScheduledAt, string Status, string? Findings, DateTimeOffset? CompletedAt);
