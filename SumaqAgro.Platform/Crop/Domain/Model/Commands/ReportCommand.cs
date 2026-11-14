using SumaqAgro.Platform.Crop.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Crop.Domain.Model.Commands;

public sealed record ReportCommand(long PlotId, long ReporterId, string Description, PestReportSeverity Severity);
