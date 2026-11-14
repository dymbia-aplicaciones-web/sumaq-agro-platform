using SumaqAgro.Platform.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;

namespace SumaqAgro.Platform.Subscriptions.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class SubscriptionsRepository(AppDbContext context) : ISubscriptionsRepository
{
    public Task<List<Plan>> ListPlansAsync(CancellationToken ct) =>
        context.Set<Plan>().AsNoTracking().OrderBy(plan => plan.Code).ToListAsync(ct);

    public Task<Plan?> FindPlanAsync(string code, CancellationToken ct) =>
        context.Set<Plan>().FindAsync([code], ct).AsTask();

    public Task<Subscription?> FindSubscriptionAsync(long userId, CancellationToken ct) =>
        context.Set<Subscription>().AsNoTracking().SingleOrDefaultAsync(subscription => subscription.UserId == userId, ct);

    public void Add(Payment payment) => context.Add(payment);
}
