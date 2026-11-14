using SumaqAgro.Platform.Certification.Domain.Model.Queries;
using SumaqAgro.Platform.Shared.Application.Model;

namespace SumaqAgro.Platform.Certification.Application.QueryServices;

public interface ITraceabilityQueryService
{
    Task<Result<TraceabilityView>> VerifyAsync(VerifyCertificateTraceabilityQuery query, CancellationToken ct);
}

public sealed record TraceabilityView(string CertificateNumber, string Status, bool Authentic,
        string BatchCode, string CropType, string QualityCategory, decimal? QualityScore,
    string? PlotName, string? Region, string? SeedVariety);

