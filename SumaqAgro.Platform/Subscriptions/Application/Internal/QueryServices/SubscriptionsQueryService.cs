using SumaqAgro.Platform.Subscriptions.Domain.Model.Queries;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;
using SumaqAgro.Platform.Subscriptions.Application.QueryServices;
using SumaqAgro.Platform.Subscriptions.Domain.Repositories;

namespace SumaqAgro.Platform.Subscriptions.Application.Internal.QueryServices;

public sealed class SubscriptionsQueryService(ISubscriptionsRepository repository) : ISubscriptionsQueryService
{
    public Task<List<Plan>> ListPlansAsync(GetPlansQuery query, CancellationToken ct) =>
        repository.ListPlansAsync(ct);

    public Task<Subscription?> FindSubscriptionAsync(GetSubscriptionByUserIdQuery query, CancellationToken ct) =>
        repository.FindSubscriptionAsync(query.UserId, ct);
}
