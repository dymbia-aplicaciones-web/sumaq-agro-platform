using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Certification.Domain.Model.Errors;

public static class CertificationErrors
{
    public static Error Rule(string message) => new("BUSINESS_RULE_VIOLATION", message);
    public static Error Missing(string resource) => Error.NotFound(resource, "The requested resource was not found.");
    public static Error Duplicate(string resource) => Error.Conflict(resource, "The resource already exists.");
    public static Error ConcurrentChange(string resource) => Error.Conflict(resource, "The resource changed concurrently.");
    public static readonly Error BatchAlreadyCertified =
        Error.Conflict("QualityCertificate", "The batch already has a certificate.");
}