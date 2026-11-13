using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Iam.Interfaces.Rest.Resources;

public sealed record SignInResource(
    [param: Required, EmailAddress, StringLength(254)] string Email,
    [param: Required] string Password);

public sealed record AuthenticatedUserResource(long Id, string Email, string[] Roles, string Token);
