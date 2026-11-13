namespace SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;

public interface IHashingService
{
    string Hash(string password);
    bool Verify(string password, string? hash);
}
