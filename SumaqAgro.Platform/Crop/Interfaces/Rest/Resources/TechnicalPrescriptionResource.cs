namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record TechnicalPrescriptionResource(long Id, long PestReportId, long PlotId, long AgronomistId, string Product, string Dosage, string Instructions, DateOnly ApplicationDate, string Status, DateTimeOffset? AppliedAt);
