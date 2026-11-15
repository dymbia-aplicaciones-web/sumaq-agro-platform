namespace SumaqAgro.Platform.Profiles.Interfaces.Acl;

public interface IProfilesContextFacade
{
    void CreateInitialProfile(long userId, string firstName, string lastName);
}
