using SumaqAgro.Platform.Certification.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Certification.Domain.Model.Queries;

public sealed record VerifyCertificateTraceabilityQuery(string Number, CertificateHash Hash);
