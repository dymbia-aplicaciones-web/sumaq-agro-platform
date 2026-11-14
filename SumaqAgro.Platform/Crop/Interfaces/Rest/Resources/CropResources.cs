using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record RecordPestReportResource(
    [param: Range(1, long.MaxValue)] long PlotId,
    [param: Range(1, long.MaxValue)] long ReporterId,
    [param: Required, RegularExpression(@"(?s)^\s*\S.{8,}\S\s*$")] string Description,
    [param: Required, RegularExpression("^(LOW|MEDIUM|HIGH|SEVERE)$")] string Severity);

public sealed record ScheduleFieldInspectionResource(
    [param: Range(1, long.MaxValue)] long PestReportId,
    [param: Range(1, long.MaxValue)] long AgronomistId,
    [param: Required] DateTimeOffset? ScheduledAt);

public sealed record CompleteFieldInspectionResource(
    [param: Required, RegularExpression(@"(?s)^\s*\S.{8,}\S\s*$")] string Findings);

public sealed record IssueTechnicalPrescriptionResource(
    [param: Range(1, long.MaxValue)] long PestReportId,
    [param: Range(1, long.MaxValue)] long AgronomistId,
    [param: Required] string Product,
    [param: Required] string Dosage,
    [param: Required] string Instructions,
    [param: Required] DateOnly? ApplicationDate);

public sealed record PublishRegionalBulletinResource(
    [param: Range(1, long.MaxValue)] long AgronomistId,
    [param: Required, StringLength(255)] string Region,
    [param: Required] string Title,
    [param: Required] string Content);
