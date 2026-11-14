namespace SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

public sealed class FieldInspection(long pestReportId, long plotId, long agronomistId, DateTimeOffset scheduledAt)
{
    public long Id { get; private set; }
    public long PestReportId { get; private set; } = pestReportId;
    public long PlotId { get; private set; } = plotId;
    public long AgronomistId { get; private set; } = agronomistId;
    public DateTimeOffset ScheduledAt { get; private set; } = scheduledAt;

    public string Status { get; private set; } = "SCHEDULED";
    public string? Findings { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    public void Complete(string findings, DateTimeOffset now)
    {
        Findings = findings;
        CompletedAt = now;
        Status = "COMPLETED";
    }
}
