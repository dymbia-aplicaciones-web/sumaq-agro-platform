using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Iam.Domain.Model.Commands;
using SumaqAgro.Platform.Iam.Interfaces.Rest.Resources;

namespace SumaqAgro.Platform.Iam.Interfaces.Rest.Transform;

internal static class IamResourceAssembler
{
    public static RegisterUserCommand ToCommand(SignUpResource resource) =>
        new(resource.Email, resource.Password, resource.Roles[0], resource.Profile.FirstName, resource.Profile.LastName);

    public static SignInCommand ToCommand(SignInResource resource) =>
        new(resource.Email, resource.Password);

    public static UserResource ToResource(User user) => new(user.Id, user.Email, [user.Role]);

    public static AuthenticatedUserResource ToResource(User user, string token) =>
        new(user.Id, user.Email, [user.Role], token);
}
