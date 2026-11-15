namespace SumaqAgro.Platform.Profiles.Application.Internal.OutboundServices;

public interface IAdvisorPlotQuota
{
    Task<int?> GetCurrentPlotLimitAsync(long userId, CancellationToken ct);
    Task<IAdvisorPlotQuotaLock> LockAccountAsync(long userId, CancellationToken ct);
}

public interface IAdvisorPlotQuotaLock : IAsyncDisposable
{
    Task CommitAsync(CancellationToken ct);
}
