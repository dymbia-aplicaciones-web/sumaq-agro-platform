using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SumaqAgro.Platform.Iam.Interfaces.Rest.Resources;

public sealed record SignUpResource(
    [param: Required, EmailAddress, StringLength(254)] string Email,
    [param: Required, StringLength(72, MinimumLength = 8)] string Password,
    [param: Required, MinLength(1), MaxLength(1)] string[] Roles,
    [param: Required] ProfileResource Profile) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Encoding.UTF8.GetByteCount(Password) > 72)
            yield return new ValidationResult("Use at most 72 UTF-8 bytes.", [nameof(Password)]);

        if (Roles[0] is not ("ROLE_FARMER" or "ROLE_COOPERATIVE_DIRECTOR" or "ROLE_AGRONOMIST"))
            yield return new ValidationResult("Select a valid role.", [nameof(Roles)]);
    }
}

public sealed record ProfileResource(
    [param: Required, StringLength(100)] string FirstName,
    [param: Required, StringLength(100)] string LastName);

public sealed record UserResource(long Id, string Email, string[] Roles);
