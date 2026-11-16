using SumaqAgro.Platform.Certification.Domain.Model.Queries;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;
using SumaqAgro.Platform.Certification.Application.QueryServices;
using SumaqAgro.Platform.Certification.Domain.Repositories;

namespace SumaqAgro.Platform.Certification.Application.Internal.QueryServices;

public sealed class CertificationQueryService(ICertificationRepository repository) : ICertificationQueryService
{
    public Task<IReadOnlyList<HarvestBatch>> ListBatchesAsync(long cooperativeId, long? memberId, CancellationToken ct) =>
        repository.ListBatchesAsync(cooperativeId, memberId, ct);

    public Task<QualityCertificate?> FindCertificateByBatchAsync(long batchId, CancellationToken ct) =>
        repository.FindCertificateByBatchAsync(batchId, ct);

    public Task<HarvestBatch?> FindBatchAsync(GetHarvestBatchByIdQuery query, CancellationToken ct) =>
        repository.FindBatchAsync(query.Id, ct);

    public Task<QualityCertificate?> FindCertificateAsync(long id, CancellationToken ct) =>
        repository.FindCertificateAsync(id, ct);

    public Task<QualityCertificate?> FindCertificateAsync(GetQualityCertificateByNumberQuery query, CancellationToken ct) =>
        repository.FindCertificateAsync(query.Number, ct);
}