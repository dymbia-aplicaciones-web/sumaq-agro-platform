namespace SumaqAgro.Platform.Certification.Domain.Model.Commands;

public sealed record RegisterHarvestBatchCommand(string Code, long CooperativeId, long MemberId, long PlotId, string CropType, DateOnly HarvestedOn);