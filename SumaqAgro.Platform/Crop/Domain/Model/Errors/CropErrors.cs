using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Crop.Domain.Model.Errors;

public static class CropErrors
{
    public static readonly Error ReportChanged = Error.Conflict("PestReport", "The pest report changed concurrently.");
    public static readonly Error ReportMustBePending = Error.Conflict("FieldInspection", "The pest report must be pending.");
    public static readonly Error InspectionInPast = Error.Validation("FieldInspection", "The inspection must be scheduled in the future.");
    public static readonly Error InspectionCannotComplete = Error.Conflict("FieldInspection", "Only a scheduled inspection whose time has arrived can be completed.");
    public static readonly Error ReportMustBeInspected = Error.Conflict("TechnicalPrescription", "The pest report must be inspected.");
    public static readonly Error ApplicationDateInPast = Error.Validation("TechnicalPrescription", "The application date cannot be in the past.");
    public static readonly Error PrescriptionAlreadyApplied = Error.Conflict("TechnicalPrescription", "The prescription is already applied.");
    public static Error Missing(string resource) => Error.NotFound(resource, "The requested resource was not found.");
    public static Error ConcurrentChange(string resource) => Error.Conflict(resource, "The resource changed concurrently.");
    public static Error DuplicateForReport(string resource) => Error.Conflict(resource, "A resource already exists for this pest report.");
}
