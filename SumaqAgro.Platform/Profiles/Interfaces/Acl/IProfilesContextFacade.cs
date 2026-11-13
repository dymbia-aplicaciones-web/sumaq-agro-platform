using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
namespace SumaqAgro.Platform.Profiles.Interfaces.Acl;

public interface IProfilesContextFacade
{
    void CreateInitialProfile(User user, string firstName, string lastName);
}
