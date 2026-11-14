namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;

public sealed record QualityCertificateResource(long Id, string CertificateNumber, long HarvestBatchId, string QualityCategory, string CertificateHash, long IssuedByUserId, DateTimeOffset IssuedAt, string Status);