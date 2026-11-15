using SumaqAgro.Platform.Subscriptions.Domain.Repositories;
using SumaqAgro.Platform.Subscriptions.Interfaces.Acl;

namespace SumaqAgro.Platform.Subscriptions.Application.Acl;

public sealed class AdvisorSubscriptionFacade(ISubscriptionsRepository subscriptions, TimeProvider clock)
    : IAdvisorSubscriptionFacade
{
    public async Task<int?> GetCurrentPlotLimitAsync(long userId, CancellationToken ct)
    {
        var subscription = await subscriptions.FindSubscriptionAsync(userId, ct);
        var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(clock.GetUtcNow(), "America/Lima").DateTime);
        if (subscription is not { PlanCode: "TECHNICAL_ADVISOR" } || today < subscription.StartDate || today > subscription.EndDate)
            return null;

        var plan = await subscriptions.FindPlanAsync(subscription.PlanCode, ct);
        return plan?.SupervisedPlotLimit ?? 0;
    }

    public async Task<IAdvisorSubscriptionLock> LockAccountAsync(long userId, CancellationToken ct) =>
        new AccountLock(await subscriptions.BeginAccountUpdateAsync(userId, ct));

    private sealed class AccountLock(ISubscriptionTransaction transaction) : IAdvisorSubscriptionLock
    {
        public Task CommitAsync(CancellationToken ct) => transaction.CommitAsync(ct);
        public ValueTask DisposeAsync() => transaction.DisposeAsync();
    }
}
