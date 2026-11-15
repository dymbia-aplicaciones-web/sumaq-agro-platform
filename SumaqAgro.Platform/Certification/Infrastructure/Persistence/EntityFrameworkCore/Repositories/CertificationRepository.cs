using SumaqAgro.Platform.Certification.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Certification.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class CertificationRepository(AppDbContext context) : ICertificationRepository
{
    public async Task<IReadOnlyList<HarvestBatch>> ListBatchesAsync(long cooperativeId, long? memberId, CancellationToken ct)
    {
        var batches = context.Set<HarvestBatch>().AsNoTracking().Where(batch => batch.CooperativeId == cooperativeId);
        if (memberId is { } id)
            batches = batches.Where(batch => batch.MemberId == id);
        return await batches.OrderByDescending(batch => batch.Id).ToListAsync(ct);
    }

    public Task<QualityCertificate?> FindCertificateByBatchAsync(long batchId, CancellationToken ct) =>
        context.Set<QualityCertificate>().AsNoTracking().SingleOrDefaultAsync(certificate => certificate.HarvestBatchId == batchId, ct);

    public Task<HarvestBatch?> FindBatchAsync(long id, CancellationToken ct) =>
        context.Set<HarvestBatch>().SingleOrDefaultAsync(batch => batch.Id == id, ct);

    public Task<QualityCertificate?> FindCertificateAsync(long id, CancellationToken ct) =>
        context.Set<QualityCertificate>().SingleOrDefaultAsync(certificate => certificate.Id == id, ct);

    public Task<QualityCertificate?> FindCertificateAsync(string number, CancellationToken ct) =>
        context.Set<QualityCertificate>().SingleOrDefaultAsync(certificate => certificate.CertificateNumber == number, ct);

    public void Add(HarvestBatch batch) => context.Add(batch);
    public void Add(QualityCertificate certificate) => context.Add(certificate);
}