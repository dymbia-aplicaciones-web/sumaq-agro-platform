namespace SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

public sealed class PestReport(long plotId, long reporterId, string description, string severity)
{
    public long Id { get; private set; }
    public long PlotId { get; private set; } = plotId;
    public long ReporterId { get; private set; } = reporterId;
    public string Description { get; private set; } = description;
    public string Severity { get; private set; } = severity;

    public string Status { get; private set; } = "PENDING";

    public void MarkInspected() => Status = "INSPECTED";
}
