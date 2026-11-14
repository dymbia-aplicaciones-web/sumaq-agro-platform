using SumaqAgro.Platform.Subscriptions.Domain.Model.Commands;
using SumaqAgro.Platform.Subscriptions.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Errors;
using SumaqAgro.Platform.Subscriptions.Application.CommandServices;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
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
        if (await repository.FindSubscriptionAsync(userId, ct) is not null)
            return AlreadySubscribed();

        var now = time.GetUtcNow();
        var monthly = cycle == BillingCycle.MONTHLY;
        var payment = new Payment($"demo-{Guid.NewGuid()}", userId, planCode, cycle.ToString(),
            monthly ? plan.MonthlyPrice : plan.AnnualPrice, outcome, now);
        if (outcome == "COMPLETED")
        {
            var start = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(now, BillingZone).DateTime);
            payment.Subscription = new Subscription(userId, planCode, cycle.ToString(), start, start.AddMonths(monthly ? 1 : 12));
        }

        repository.Add(payment);
        try { await unitOfWork.CompleteAsync(ct); }
        catch (DbUpdateException exception) when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return AlreadySubscribed();
        }

        return Result<Payment>.Success(payment);
    }

    private static Result<Payment> AlreadySubscribed() =>
        Result<Payment>.Failure(SubscriptionErrors.AlreadySubscribed);
}
