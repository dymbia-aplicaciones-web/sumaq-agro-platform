using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Resources;

public sealed record SubscriptionResource(long Id, string PlanCode, string BillingCycle, string Status,
    DateOnly StartDate, DateOnly EndDate);
