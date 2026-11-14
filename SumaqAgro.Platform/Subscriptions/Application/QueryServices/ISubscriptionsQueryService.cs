using SumaqAgro.Platform.Subscriptions.Domain.Model.Queries;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;

namespace SumaqAgro.Platform.Subscriptions.Application.QueryServices;

public interface ISubscriptionsQueryService
{
    Task<List<Plan>> ListPlansAsync(GetPlansQuery query, CancellationToken ct);

    Task<Subscription?> FindSubscriptionAsync(GetSubscriptionByUserIdQuery query, CancellationToken ct);
}
