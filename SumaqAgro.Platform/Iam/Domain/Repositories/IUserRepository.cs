using SumaqAgro.Platform.Shared.Domain.Repositories;
using SumaqAgro.Platform.Iam.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Iam.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken);

    Task<IUserRegistrationTransaction> BeginRegistrationAsync(CancellationToken ct);
    void Detach(User user);
}

public interface IUserRegistrationTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken ct);
}
