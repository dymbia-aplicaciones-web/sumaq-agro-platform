namespace SumaqAgro.Platform.Profiles.Interfaces.Acl;

public interface IProfilesContextFacade
{
    Task<bool> MemberBelongsAsync(long cooperativeId, long memberId, CancellationToken ct);
    void CreateInitialProfile(long userId, string firstName, string lastName);
}
