using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Commands;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;
using SumaqAgro.Platform.Subscriptions.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Resources;

namespace SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Transform;

internal static class SubscriptionResourceAssembler
{
    public static CheckoutCommand ToCommand(long userId, string role, CheckoutResource resource) =>
        new(userId, role, resource.PlanCode, Enum.Parse<BillingCycle>(resource.BillingCycle),
            resource.SimulatedOutcome ?? "COMPLETED");

    public static PlanResource ToResource(Plan plan) =>
        new(plan.Code, plan.MonthlyPrice, plan.AnnualPrice, "PEN", plan.RegisteredPlotLimit,
            plan.ProducerLimit, plan.SupervisedPlotLimit);

    public static SubscriptionResource ToResource(Subscription subscription) =>
        new(subscription.Id, subscription.PlanCode, subscription.BillingCycle, subscription.Status,
            subscription.StartDate, subscription.EndDate);

    public static PaymentResource ToResource(Payment payment) =>
        new(payment.Reference, payment.SubscriptionId, payment.PlanCode, payment.BillingCycle,
            payment.Amount, payment.Currency, payment.Status, payment.AttemptedAt);
}
