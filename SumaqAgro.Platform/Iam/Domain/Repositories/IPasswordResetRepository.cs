using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Iam.Domain.Repositories;

public interface IPasswordResetRepository
{
    void Add(PasswordResetToken token);
    Task<PasswordResetToken?> FindAsync(string digest, CancellationToken ct);
    Task<User?> FindUserAsync(long id, CancellationToken ct);
}
