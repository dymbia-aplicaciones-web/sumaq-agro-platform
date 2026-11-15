using SumaqAgro.Platform.Certification.Domain.Model.Queries;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Certification.Application.QueryServices;

public interface ICertificationQueryService
{
    Task<IReadOnlyList<HarvestBatch>> ListBatchesAsync(long cooperativeId, long? memberId, CancellationToken ct);
    Task<QualityCertificate?> FindCertificateByBatchAsync(long batchId, CancellationToken ct);
    Task<HarvestBatch?> FindBatchAsync(GetHarvestBatchByIdQuery query, CancellationToken ct);

    Task<QualityCertificate?> FindCertificateAsync(long id, CancellationToken ct);

    Task<QualityCertificate?> FindCertificateAsync(GetQualityCertificateByNumberQuery query, CancellationToken ct);
}