using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Certification.Domain.Repositories;

public interface ICertificationRepository
{
    Task<IReadOnlyList<HarvestBatch>> ListBatchesAsync(long cooperativeId, long? memberId, CancellationToken ct);
    Task<QualityCertificate?> FindCertificateByBatchAsync(long batchId, CancellationToken ct);
    Task<HarvestBatch?> FindBatchAsync(long id, CancellationToken ct);
    Task<QualityCertificate?> FindCertificateAsync(long id, CancellationToken ct);
    Task<QualityCertificate?> FindCertificateAsync(string number, CancellationToken ct);
    void Add(HarvestBatch batch);
    void Add(QualityCertificate certificate);
}