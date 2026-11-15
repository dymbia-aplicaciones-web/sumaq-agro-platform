using SumaqAgro.Platform.Iam.Interfaces.Acl;
using SumaqAgro.Platform.Iam.Domain.Repositories;
using SumaqAgro.Platform.Iam.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Iam.Application.Acl;

public sealed class IamAccounts(IUserRepository users) : IIamAccounts
{
    public async Task<IamAccount?> FindAsync(string email, CancellationToken ct)
    {
        var user = await users.FindByEmailAsync(new EmailAddress(email), ct);
        return user is null ? null : new(user.Id, user.Role);
    }
}
