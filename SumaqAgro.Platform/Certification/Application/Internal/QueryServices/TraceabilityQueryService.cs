using SumaqAgro.Platform.Farming.Domain.Model.Queries;
using SumaqAgro.Platform.Certification.Domain.Model.Queries;
using SumaqAgro.Platform.Certification.Application.QueryServices;
using SumaqAgro.Platform.Farming.Application.QueryServices;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Certification.Application.Internal.QueryServices;

public sealed class TraceabilityQueryService(ICertificationQueryService certification, IFarmingQueryService farming)
    : ITraceabilityQueryService
{
    public async Task<Result<TraceabilityView>> VerifyAsync(VerifyCertificateTraceabilityQuery query, CancellationToken ct)
    {
        var certificate = await certification.FindCertificateAsync(new GetQualityCertificateByNumberQuery(query.Number.Trim()), ct);
        if (certificate is null) return Result<TraceabilityView>.Failure(Error.NotFound("QualityCertificate", "The requested resource was not found."));
        var batch = await certification.FindBatchAsync(new GetHarvestBatchByIdQuery(certificate.HarvestBatchId), ct);
        if (batch is null) return Result<TraceabilityView>.Failure(Error.NotFound("HarvestBatch", "The requested resource was not found."));

        var plot = await farming.FindPlotAsync(new GetFieldPlotByIdQuery(batch.PlotId), ct);
        var authentic = certificate.Status == "ACTIVE" &&
                        string.Equals(certificate.CertificateHash, query.Hash.Value, StringComparison.OrdinalIgnoreCase);
        return Result<TraceabilityView>.Success(new TraceabilityView(certificate.CertificateNumber, certificate.Status,
            authentic, batch.BatchCode, batch.CropType, certificate.QualityCategory, batch.QualityScore,
            plot?.Name, plot?.Region, null));
    }
}