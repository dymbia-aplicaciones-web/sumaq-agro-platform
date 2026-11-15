using SumaqAgro.Platform.Profiles.Domain.Model.Queries;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Application.QueryServices;

public interface IProfilesQueryService
{
    Task<AgronomistAssignment?> FindAssignmentAsync(long id, CancellationToken ct);
    Task<Cooperative?> FindCooperativeAsync(long id, CancellationToken ct);
    Task<List<Cooperative>> ListCooperativesAsync(CancellationToken ct);
    Task<List<CooperativeMember>> ListMembershipsAsync(long userId, CancellationToken ct);
    Task<List<CooperativeMember>> ListMembersAsync(long cooperativeId, CancellationToken ct);
    Task<Profile?> FindProfileByIdAsync(long profileId, CancellationToken ct);
    Task<List<Profile>> ListProfilesAsync(long? userId, CancellationToken ct);
    Task<Profile?> FindProfileAsync(GetProfileByUserIdQuery query, CancellationToken cancellationToken);

    Task<AgronomistAssignment?> FindActiveAssignmentAsync(GetActiveAssignmentByPlotQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyList<AgronomistAssignment>> ListAssignmentsAsync(GetAssignmentsByCooperativeQuery query, CancellationToken cancellationToken);
}
