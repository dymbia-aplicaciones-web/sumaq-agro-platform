namespace SumaqAgro.Platform.Certification.Domain.Model.Aggregates;

public sealed class QualityCertificate(string certificateNumber, long harvestBatchId, string qualityCategory,
    string certificateHash, long issuedByUserId, DateTimeOffset issuedAt)
{
    public long Id { get; private set; }
    public string CertificateNumber { get; private set; } = certificateNumber;
    public long HarvestBatchId { get; private set; } = harvestBatchId;
    public string QualityCategory { get; private set; } = qualityCategory;
    public string CertificateHash { get; private set; } = certificateHash;
    public long IssuedByUserId { get; private set; } = issuedByUserId;
    public DateTimeOffset IssuedAt { get; private set; } = issuedAt;
    public string Status { get; private set; } = "ACTIVE";

    public void Revoke()
    {
        if (Status == "REVOKED") throw new InvalidOperationException("The certificate is already revoked.");
        Status = "REVOKED";
    }
}