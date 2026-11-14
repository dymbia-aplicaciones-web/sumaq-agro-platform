namespace SumaqAgro.Platform.Certification.Domain.Model.Commands;

public sealed record SampleHarvestBatchCommand(long Id, string Identifier, decimal Weight);