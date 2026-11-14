namespace SumaqAgro.Platform.Crop.Domain.Model.Commands;

public sealed record IssueCommand(long ReportId, long AgronomistId, string Product, string Dosage, string Instructions, DateOnly ApplicationDate);
