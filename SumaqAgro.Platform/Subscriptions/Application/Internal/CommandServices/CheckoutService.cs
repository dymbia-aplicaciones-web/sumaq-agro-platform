using SumaqAgro.Platform.Subscriptions.Domain.Model.Commands;
using SumaqAgro.Platform.Subscriptions.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Errors;
using SumaqAgro.Platform.Subscriptions.Application.CommandServices;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Repositories;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;
using SumaqAgro.Platform.Subscriptions.Domain.Repositories;

namespace SumaqAgro.Platform.Subscriptions.Application.Internal.CommandServices;

public sealed class CheckoutService(ISubscriptionsRepository repository, IUnitOfWork unitOfWork, TimeProvider time) : ICheckoutService
{
    private static readonly TimeZoneInfo BillingZone = TimeZoneInfo.FindSystemTimeZoneById("America/Lima");

    public async Task<Result<Payment>> CheckoutAsync(CheckoutCommand command, CancellationToken ct)
    {
        var (userId, role, planCode, cycle, outcome) = command;
        var allowedRole = planCode switch
        {
            "SEED" => "ROLE_FARMER",
            "COOPERATIVE_PRO" => "ROLE_COOPERATIVE_DIRECTOR",
            "TECHNICAL_ADVISOR" => "ROLE_AGRONOMIST",
            _ => null
        };
        if (role != allowedRole) return Result<Payment>.Failure(SubscriptionErrors.PlanNotAllowed);

        var plan = await repository.FindPlanAsync(planCode, ct);
        if (plan is null) return Result<Payment>.Failure(SubscriptionErrors.PlanNotFound);
        await using var transaction = await repository.BeginAccountUpdateAsync(userId, ct);
        var subscription = await repository.FindTrackedSubscriptionAsync(userId, ct);
        if (subscription is not null && subscription.PlanCode != planCode)
            return Result<Payment>.Failure(SubscriptionErrors.PlanMismatch);

        var now = time.GetUtcNow();
        var monthly = cycle == BillingCycle.MONTHLY;
        var payment = new Payment($"demo-{Guid.NewGuid()}", userId, planCode, cycle.ToString(),
            monthly ? plan.MonthlyPrice : plan.AnnualPrice, outcome, now);
        if (outcome == "COMPLETED")
        {
            var start = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(now, BillingZone).DateTime);
            var months = monthly ? 1 : 12;
            if (subscription is null)
                subscription = new Subscription(userId, planCode, cycle.ToString(), start, start.AddMonths(months));
            else
                subscription.Renew(cycle.ToString(), start, months);
        }

        payment.Subscription = subscription;
        repository.Add(payment);
        await unitOfWork.CompleteAsync(ct);
        await transaction.CommitAsync(ct);
        return Result<Payment>.Success(payment);
    }

    public async Task<Result<Subscription>> CancelAsync(CancelSubscriptionCommand command, CancellationToken ct)
    {
        await using var transaction = await repository.BeginAccountUpdateAsync(command.UserId, ct);
        var subscription = await repository.FindTrackedSubscriptionAsync(command.UserId, ct);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionNotFound);

        subscription.CancelRenewal();
        await unitOfWork.CompleteAsync(ct);
        await transaction.CommitAsync(ct);
        return Result<Subscription>.Success(subscription);
    }
}
