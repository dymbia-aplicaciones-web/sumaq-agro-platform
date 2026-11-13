using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Iam.Domain.Model.Errors;

public static class IamErrors
{
    public static readonly Error EmailAlreadyExists = Error.Conflict("User", "Email already exists");
    public static readonly Error InvalidCredentials =
        Error.Unauthorized("INVALID_CREDENTIALS", "Invalid email or password");
}
