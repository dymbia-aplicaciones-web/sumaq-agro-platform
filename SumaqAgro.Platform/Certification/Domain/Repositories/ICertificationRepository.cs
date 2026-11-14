using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Certification.Domain.Repositories;

public interface ICertificationRepository
{
    Task<HarvestBatch?> FindBatchAsync(long id, CancellationToken ct);
    Task<QualityCertificate?> FindCertificateAsync(long id, CancellationToken ct);
    Task<QualityCertificate?> FindCertificateAsync(string number, CancellationToken ct);
    Task<bool> MemberBelongsAsync(long cooperativeId, long memberId, CancellationToken ct);
    void Add(HarvestBatch batch);
    void Add(QualityCertificate certificate);
}