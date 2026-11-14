using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using System.Text.Json.Serialization;

namespace SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;

public sealed class Payment(string reference, long userId, string planCode, string billingCycle, decimal amount,
    string status, DateTimeOffset attemptedAt)
{
    public string Reference { get; private set; } = reference;
    [JsonIgnore] public long UserId { get; private set; } = userId;
    public long? SubscriptionId { get; private set; }
    [JsonIgnore] public Subscription? Subscription { get; set; }
    public string PlanCode { get; private set; } = planCode;
    public string BillingCycle { get; private set; } = billingCycle;
    public decimal Amount { get; private set; } = amount;
    public string Currency => "PEN";
    public string Status { get; private set; } = status;
    public DateTimeOffset AttemptedAt { get; private set; } = attemptedAt;
}
