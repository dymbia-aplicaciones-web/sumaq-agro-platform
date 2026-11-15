using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Domain.Repositories;

public interface IProfilesRepository
{
    Task<Cooperative?> FindCooperativeAsync(long id, CancellationToken ct);
    Task<List<Cooperative>> ListCooperativesAsync(CancellationToken ct);
    Task<List<CooperativeMember>> ListMembershipsAsync(long userId, CancellationToken ct);
    Task<List<CooperativeMember>> ListMembersAsync(long cooperativeId, CancellationToken ct);
    Task<Profile?> FindProfileByIdAsync(long profileId, CancellationToken ct);
    Task<List<Profile>> ListProfilesAsync(long? userId, CancellationToken ct);
    void AddProfile(Profile profile);
    Task<Profile?> FindProfileAsync(long userId, CancellationToken cancellationToken);
    Task<bool> CooperativeExistsAsync(long id, CancellationToken cancellationToken);
    void AddCooperative(Cooperative cooperative);
    Task<bool> MemberBelongsAsync(long cooperativeId, long memberId, CancellationToken ct);
    void AddMember(CooperativeMember member);
    void AddInvitation(AgronomistInvitation invitation);
    Task<AgronomistAssignment?> FindAssignmentAsync(long id, CancellationToken ct);
    Task<int> CountActiveAssignmentsAsync(long userId, CancellationToken ct);
    void AddAssignment(AgronomistAssignment assignment);
    Task<AgronomistAssignment?> FindActiveAssignmentAsync(long plotId, CancellationToken cancellationToken);
    Task<IReadOnlyList<AgronomistAssignment>> ListAssignmentsAsync(long cooperativeId, CancellationToken cancellationToken);
}
