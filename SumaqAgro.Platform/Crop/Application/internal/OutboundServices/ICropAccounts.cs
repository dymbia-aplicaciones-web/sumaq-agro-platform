namespace SumaqAgro.Platform.Crop.Application.Internal.OutboundServices;

public interface ICropAccounts
{
    Task<long?> FindIdAsync(string email, CancellationToken ct);
}
