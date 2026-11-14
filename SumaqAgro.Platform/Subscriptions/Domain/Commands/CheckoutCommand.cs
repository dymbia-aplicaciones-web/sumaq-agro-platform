using SumaqAgro.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Subscriptions.Domain.Model.Commands;

public sealed record CheckoutCommand(long UserId, string Role, string PlanCode, BillingCycle Cycle, string Outcome);
