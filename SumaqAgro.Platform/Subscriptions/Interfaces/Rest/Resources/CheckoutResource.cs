using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Resources;

public sealed record CheckoutResource(
    [param: Required, RegularExpression("^(SEED|COOPERATIVE_PRO|TECHNICAL_ADVISOR)$")] string PlanCode,
    [param: Required, RegularExpression("^(MONTHLY|ANNUAL)$")] string BillingCycle,
    [param: RegularExpression("^(COMPLETED|REJECTED)$")] string? SimulatedOutcome);
