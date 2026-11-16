namespace SumaqAgro.Platform.Subscriptions.Application.Internal.OutboundServices;

public interface ISubscriptionAccounts
{
    Task<SubscriptionAccount?> FindAsync(string email, CancellationToken ct);
}

public sealed record SubscriptionAccount(long Id, string Role);
