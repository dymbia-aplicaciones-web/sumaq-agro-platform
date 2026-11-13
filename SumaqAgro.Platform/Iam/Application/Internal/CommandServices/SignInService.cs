using SumaqAgro.Platform.Iam.Domain.Model.Commands;
using SumaqAgro.Platform.Iam.Application.CommandServices;
using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Iam.Domain.Repositories;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Iam.Domain.Model.Errors;
using SumaqAgro.Platform.Iam.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Iam.Application.Internal.CommandServices;

public sealed class SignInService(IUserRepository users, ITokenService tokens, IHashingService hashing) : ISignInService
{
    public async Task<Result<(User User, string Token)>> SignInAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await users.FindByEmailAsync(new EmailAddress(command.Email), cancellationToken);
        var matches = hashing.Verify(command.Password, user?.PasswordHash);

        return user is null || !matches
            ? Result<(User, string)>.Failure(IamErrors.InvalidCredentials)
            : Result<(User, string)>.Success((user, tokens.Generate(user.Email)));
    }
}
