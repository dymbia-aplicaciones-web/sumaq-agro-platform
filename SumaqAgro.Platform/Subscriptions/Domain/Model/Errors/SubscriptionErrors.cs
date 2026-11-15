using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Subscriptions.Domain.Model.Errors;

public static class SubscriptionErrors
{
    public static readonly Error PlanNotAllowed = Error.Forbidden("PlanRole", "The role cannot select this plan.");
    public static readonly Error PlanNotFound = Error.NotFound("Plan", "Plan not found.");
    public static readonly Error SubscriptionNotFound = Error.NotFound("Subscription", "Subscription not found.");
    public static readonly Error PlanMismatch = Error.Conflict("Subscription", "The selected plan must match the existing subscription.");
}
