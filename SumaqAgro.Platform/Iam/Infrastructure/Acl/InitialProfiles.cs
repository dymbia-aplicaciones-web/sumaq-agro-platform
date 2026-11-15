using SumaqAgro.Platform.Profiles.Interfaces.Acl;
using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;

namespace SumaqAgro.Platform.Iam.Infrastructure.Acl;

public sealed class InitialProfiles(IProfilesContextFacade profiles) : IInitialProfiles
{
    public void Create(long userId, string firstName, string lastName) =>
        profiles.CreateInitialProfile(userId, firstName, lastName);
}
