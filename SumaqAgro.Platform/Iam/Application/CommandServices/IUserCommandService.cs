using SumaqAgro.Platform.Iam.Domain.Model.Commands;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<User>> RegisterAsync(RegisterUserCommand command, CancellationToken cancellationToken);
}
