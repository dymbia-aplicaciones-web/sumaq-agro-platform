namespace SumaqAgro.Platform.Certification.Domain.Model.Commands;

public sealed record WeighHarvestBatchCommand(long Id, decimal Gross, decimal Tare);