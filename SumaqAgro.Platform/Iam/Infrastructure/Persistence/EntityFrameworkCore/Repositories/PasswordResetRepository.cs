using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Iam.Domain.Repositories;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class PasswordResetRepository(AppDbContext context) : IPasswordResetRepository
{
    public void Add(PasswordResetToken token) => context.Add(token);

    public Task<PasswordResetToken?> FindAsync(string digest, CancellationToken ct) =>
        context.Set<PasswordResetToken>().SingleOrDefaultAsync(token => token.Digest == digest, ct);

    public Task<User?> FindUserAsync(long id, CancellationToken ct) =>
        context.Set<User>().SingleOrDefaultAsync(user => user.Id == id, ct);
}
