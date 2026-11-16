namespace SumaqAgro.Platform.Subscriptions.Interfaces.Acl;

public interface IAdvisorSubscriptionFacade
{
    Task<int?> GetCurrentPlotLimitAsync(long userId, CancellationToken ct);
    Task<IAdvisorSubscriptionLock> LockAccountAsync(long userId, CancellationToken ct);
}

public interface IAdvisorSubscriptionLock : IAsyncDisposable
{
    Task CommitAsync(CancellationToken ct);
}
