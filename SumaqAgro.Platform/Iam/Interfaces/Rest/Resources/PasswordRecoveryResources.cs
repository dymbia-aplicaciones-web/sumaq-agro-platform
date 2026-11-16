using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Iam.Interfaces.Rest.Resources;

public sealed record RequestPasswordResetResource([param: Required, EmailAddress, StringLength(254)] string Email);

public sealed record ResetPasswordResource(
    [param: Required, RegularExpression("^[A-Za-z0-9_-]{43}$")] string Token,
    [param: Required, StringLength(72, MinimumLength = 8)] string NewPassword);
