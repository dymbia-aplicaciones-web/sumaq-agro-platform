namespace SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

public sealed class TechnicalPrescription(long pestReportId, long plotId, long agronomistId, string product, string dosage, string instructions, DateOnly applicationDate)
{
    public long Id { get; private set; }
    public long PestReportId { get; private set; } = pestReportId;
    public long PlotId { get; private set; } = plotId;
    public long AgronomistId { get; private set; } = agronomistId;
    public string Product { get; private set; } = product;
    public string Dosage { get; private set; } = dosage;
    public string Instructions { get; private set; } = instructions;
    public DateOnly ApplicationDate { get; private set; } = applicationDate;

    public string Status { get; private set; } = "ISSUED";
    public DateTimeOffset? AppliedAt { get; private set; }

    public void RegisterApplication(DateTimeOffset now)
    {
        AppliedAt = now;
        Status = "APPLIED";
    }
}
