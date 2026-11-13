using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Profiles.Domain.Model.Errors;

public static class ProfilesErrors
{
    public static readonly Error ProfileNotFound = Error.NotFound("PROFILE_NOT_FOUND", "Profile not found.");
    public static readonly Error CooperativeNotFound = Error.NotFound("COOPERATIVE_NOT_FOUND", "Cooperative not found.");
    public static readonly Error DuplicateRuc = Error.Conflict("DUPLICATE_RUC", "RUC already registered.");
    public static readonly Error DuplicateDni = Error.Conflict("DUPLICATE_DNI", "DNI already registered in the cooperative.");
    public static readonly Error ActiveAssignment = Error.Conflict("ACTIVE_ASSIGNMENT", "The plot already has an active assignment.");
}
