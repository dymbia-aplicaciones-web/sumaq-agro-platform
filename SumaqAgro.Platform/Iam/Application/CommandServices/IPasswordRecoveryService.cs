namespace SumaqAgro.Platform.Iam.Application.CommandServices;

public interface IPasswordRecoveryService
{
    Task RequestAsync(string email, CancellationToken ct);
    Task<bool> ResetAsync(string token, string password, CancellationToken ct);
}
