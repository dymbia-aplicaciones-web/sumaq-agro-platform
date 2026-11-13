using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Iam.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Iam.Domain.Repositories;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public Task<User?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken) =>
        Context.Set<User>().AsNoTracking().SingleOrDefaultAsync(user => user.Email == email.Value, cancellationToken);

    public void Detach(User user) => Context.Entry(user).State = EntityState.Detached;
}
