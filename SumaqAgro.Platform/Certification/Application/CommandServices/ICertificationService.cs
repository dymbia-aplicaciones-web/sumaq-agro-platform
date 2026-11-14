using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;
using SumaqAgro.Platform.Certification.Domain.Model.Commands;
using SumaqAgro.Platform.Shared.Application.Model;

namespace SumaqAgro.Platform.Certification.Application.CommandServices;

public interface ICertificationService
{
    Task<Result<HarvestBatch>> RegisterAsync(RegisterHarvestBatchCommand command, CancellationToken ct);
    Task<Result<HarvestBatch>> WeighAsync(WeighHarvestBatchCommand command, CancellationToken ct);
    Task<Result<HarvestBatch>> SampleAsync(SampleHarvestBatchCommand command, CancellationToken ct);
    Task<Result<HarvestBatch>> GradeAsync(GradePotatoCalibersCommand command, CancellationToken ct);
    Task<Result<HarvestBatch>> CupAsync(CupCoffeeCommand command, CancellationToken ct);
    Task<Result<QualityCertificate>> IssueAsync(IssueQualityCertificateCommand command, CancellationToken ct);
    Task<Result<QualityCertificate>> RevokeAsync(RevokeQualityCertificateCommand command, CancellationToken ct);
}