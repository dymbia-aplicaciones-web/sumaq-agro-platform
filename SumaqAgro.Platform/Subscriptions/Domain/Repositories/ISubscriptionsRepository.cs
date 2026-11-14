using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;

namespace SumaqAgro.Platform.Subscriptions.Domain.Repositories;

public interface ISubscriptionsRepository
{
    Task<List<Plan>> ListPlansAsync(CancellationToken ct);
    Task<Plan?> FindPlanAsync(string code, CancellationToken ct);
    Task<Subscription?> FindSubscriptionAsync(long userId, CancellationToken ct);
    void Add(Payment payment);
}
