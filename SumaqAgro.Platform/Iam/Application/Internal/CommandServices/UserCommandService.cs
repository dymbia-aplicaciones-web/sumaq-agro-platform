using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Iam.Domain.Model.Commands;
using SumaqAgro.Platform.Iam.Application.CommandServices;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Iam.Domain.Model.Errors;
using SumaqAgro.Platform.Iam.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Shared.Domain.Repositories;
using SumaqAgro.Platform.Iam.Domain.Repositories;

namespace SumaqAgro.Platform.Iam.Application.Internal.CommandServices;

public sealed class UserCommandService(IUserRepository users, IUnitOfWork unitOfWork, IHashingService hashing) : IUserCommandService
{
    public async Task<Result<User>> RegisterAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = User.Register(new EmailAddress(command.Email).Value, hashing.Hash(command.Password),
            command.Role, command.FirstName, command.LastName);
        await users.AddAsync(user, cancellationToken);

        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is MySqlException { Number: 1062 })
        {
            users.Detach(user);
            return Result<User>.Failure(IamErrors.EmailAlreadyExists);
        }

        return Result<User>.Success(user);
    }
}
