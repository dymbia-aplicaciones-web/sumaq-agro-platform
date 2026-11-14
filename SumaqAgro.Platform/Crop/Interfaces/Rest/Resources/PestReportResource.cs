namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record PestReportResource(long Id, long PlotId, long ReporterId, string Description, string Severity, string Status);
