using SumaqAgro.Platform.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

    public Task<Subscription?> FindTrackedSubscriptionAsync(long userId, CancellationToken ct) =>
        context.Set<Subscription>().SingleOrDefaultAsync(subscription => subscription.UserId == userId, ct);

    public Task<List<Payment>> ListPaymentsAsync(long userId, CancellationToken ct) =>
        context.Set<Payment>().AsNoTracking().Where(payment => payment.UserId == userId)
            .OrderByDescending(payment => payment.AttemptedAt).ThenByDescending(payment => payment.Reference)
            .ToListAsync(ct);

    public async Task<ISubscriptionTransaction> BeginAccountUpdateAsync(long userId, CancellationToken ct)
    {
        var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {
            // Lock the account even before its first subscription exists.
            await context.Database.SqlQuery<long>($"SELECT id FROM users WHERE id = {userId} FOR UPDATE").ToListAsync(ct);
            return new SubscriptionTransaction(transaction);
        }
        catch
        {
            await transaction.DisposeAsync();
            throw;
        }
    }

    public void Add(Payment payment) => context.Add(payment);

    private sealed class SubscriptionTransaction(IDbContextTransaction transaction) : ISubscriptionTransaction
    {
        public Task CommitAsync(CancellationToken ct) => transaction.CommitAsync(ct);
        public ValueTask DisposeAsync() => transaction.DisposeAsync();
    }
}
