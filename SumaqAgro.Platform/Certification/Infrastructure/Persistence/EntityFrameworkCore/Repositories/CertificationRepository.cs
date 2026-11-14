using SumaqAgro.Platform.Certification.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Certification.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class CertificationRepository(AppDbContext context) : ICertificationRepository
{
    public Task<HarvestBatch?> FindBatchAsync(long id, CancellationToken ct) =>
        context.Set<HarvestBatch>().SingleOrDefaultAsync(batch => batch.Id == id, ct);

    public Task<QualityCertificate?> FindCertificateAsync(long id, CancellationToken ct) =>
        context.Set<QualityCertificate>().SingleOrDefaultAsync(certificate => certificate.Id == id, ct);

    public Task<QualityCertificate?> FindCertificateAsync(string number, CancellationToken ct) =>
        context.Set<QualityCertificate>().SingleOrDefaultAsync(certificate => certificate.CertificateNumber == number, ct);

    public Task<bool> MemberBelongsAsync(long cooperativeId, long memberId, CancellationToken ct) =>
        context.Set<CooperativeMember>().AnyAsync(member => member.Id == memberId && member.CooperativeId == cooperativeId, ct);

    public void Add(HarvestBatch batch) => context.Add(batch);
    public void Add(QualityCertificate certificate) => context.Add(certificate);
}