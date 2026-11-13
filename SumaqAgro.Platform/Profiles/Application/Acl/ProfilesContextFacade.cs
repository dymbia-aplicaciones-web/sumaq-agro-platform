using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Profiles.Domain.Repositories;
using SumaqAgro.Platform.Profiles.Interfaces.Acl;

namespace SumaqAgro.Platform.Profiles.Application.Acl;

public sealed class ProfilesContextFacade(IProfilesRepository profiles) : IProfilesContextFacade
{
    public void CreateInitialProfile(User user, string firstName, string lastName) =>
        profiles.AddProfile(new Profile(user, firstName, lastName));
}
