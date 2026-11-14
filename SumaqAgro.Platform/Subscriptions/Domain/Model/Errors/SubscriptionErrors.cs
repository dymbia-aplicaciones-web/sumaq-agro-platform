using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Subscriptions.Domain.Model.Errors;

public static class SubscriptionErrors
{
    public static readonly Error PlanNotAllowed = Error.Forbidden("PlanRole", "The role cannot select this plan.");
    public static readonly Error PlanNotFound = Error.NotFound("Plan", "Plan not found.");
    public static readonly Error AlreadySubscribed = Error.Conflict("Subscription", "The account already has a subscription.");
}
