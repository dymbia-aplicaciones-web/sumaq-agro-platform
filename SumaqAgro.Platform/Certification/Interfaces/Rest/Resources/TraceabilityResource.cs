namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;

public sealed record TraceabilityResource(string CertificateNumber, string Status, bool Authentic,
        string BatchCode, string CropType, string QualityCategory, decimal? QualityScore,
    string? PlotName, string? Region, string? SeedVariety);
