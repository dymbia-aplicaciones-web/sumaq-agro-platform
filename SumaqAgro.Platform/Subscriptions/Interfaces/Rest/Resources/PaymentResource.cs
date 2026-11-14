namespace SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Resources;

public sealed record PaymentResource(string Reference, long? SubscriptionId, string PlanCode, string BillingCycle,
    decimal Amount, string Currency, string Status, DateTimeOffset AttemptedAt);
