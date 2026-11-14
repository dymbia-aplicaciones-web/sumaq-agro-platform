namespace SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;

public sealed class Subscription(long userId, string planCode, string billingCycle, DateOnly startDate, DateOnly endDate)
{
    public long Id { get; private set; }
    public long UserId { get; private set; } = userId;
    public string PlanCode { get; private set; } = planCode;
    public string BillingCycle { get; private set; } = billingCycle;
    public DateOnly StartDate { get; private set; } = startDate;
    public DateOnly EndDate { get; private set; } = endDate;
    public string Status { get; private set; } = "ACTIVE";
}
