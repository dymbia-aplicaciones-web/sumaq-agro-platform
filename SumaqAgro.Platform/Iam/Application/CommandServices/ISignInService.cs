using SumaqAgro.Platform.Iam.Domain.Model.Commands;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Iam.Application.CommandServices;

public interface ISignInService
{
    Task<Result<(User User, string Token)>> SignInAsync(SignInCommand command, CancellationToken cancellationToken);
}
