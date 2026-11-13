namespace SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;

public interface ITokenService
{
    string Generate(string email);
}
