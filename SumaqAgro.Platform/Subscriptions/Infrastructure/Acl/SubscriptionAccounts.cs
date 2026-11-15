using SumaqAgro.Platform.Iam.Interfaces.Acl;
using SumaqAgro.Platform.Subscriptions.Application.Internal.OutboundServices;

namespace SumaqAgro.Platform.Subscriptions.Infrastructure.Acl;

public sealed class SubscriptionAccounts(IIamAccounts iam) : ISubscriptionAccounts
{
    public async Task<SubscriptionAccount?> FindAsync(string email, CancellationToken ct)
    {
        var account = await iam.FindAsync(email, ct);
        return account is null ? null : new(account.Id, account.Role);
    }
}
