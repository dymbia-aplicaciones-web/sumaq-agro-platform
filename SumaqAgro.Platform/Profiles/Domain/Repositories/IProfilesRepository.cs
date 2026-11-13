using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Domain.Repositories;

public interface IProfilesRepository
{
    void AddProfile(Profile profile);
    Task<Profile?> FindProfileAsync(long userId, CancellationToken cancellationToken);
    Task<bool> CooperativeExistsAsync(long id, CancellationToken cancellationToken);
    void AddCooperative(Cooperative cooperative);
    void AddMember(CooperativeMember member);
    void AddInvitation(AgronomistInvitation invitation);
    void AddAssignment(AgronomistAssignment assignment);
    Task<AgronomistAssignment?> FindActiveAssignmentAsync(long plotId, CancellationToken cancellationToken);
    Task<IReadOnlyList<AgronomistAssignment>> ListAssignmentsAsync(long cooperativeId, CancellationToken cancellationToken);
}
