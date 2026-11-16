namespace SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;

public interface IInitialProfiles
{
    void Create(long userId, string firstName, string lastName);
}
