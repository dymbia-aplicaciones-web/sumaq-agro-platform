namespace SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;

public interface IPasswordResetEmail
{
    Task SendAsync(string email, string token, CancellationToken ct);
}
