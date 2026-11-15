using SumaqAgro.Platform.Profiles.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Subscriptions.Interfaces.Acl;

namespace SumaqAgro.Platform.Profiles.Infrastructure.Acl;

public sealed class AdvisorPlotQuota(IAdvisorSubscriptionFacade subscriptions) : IAdvisorPlotQuota
{
    public Task<int?> GetCurrentPlotLimitAsync(long userId, CancellationToken ct) =>
        subscriptions.GetCurrentPlotLimitAsync(userId, ct);

    public async Task<IAdvisorPlotQuotaLock> LockAccountAsync(long userId, CancellationToken ct) =>
        new AccountLock(await subscriptions.LockAccountAsync(userId, ct));

    private sealed class AccountLock(IAdvisorSubscriptionLock accountLock) : IAdvisorPlotQuotaLock
    {
        public Task CommitAsync(CancellationToken ct) => accountLock.CommitAsync(ct);
        public ValueTask DisposeAsync() => accountLock.DisposeAsync();
    }
}
