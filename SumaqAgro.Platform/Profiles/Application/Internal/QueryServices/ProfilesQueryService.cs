using SumaqAgro.Platform.Profiles.Domain.Model.Queries;
using SumaqAgro.Platform.Profiles.Application.QueryServices;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Profiles.Domain.Repositories;

namespace SumaqAgro.Platform.Profiles.Application.Internal.QueryServices;

public sealed class ProfilesQueryService(IProfilesRepository profiles) : IProfilesQueryService
{
    public Task<AgronomistAssignment?> FindAssignmentAsync(long id, CancellationToken ct) =>
        profiles.FindAssignmentAsync(id, ct);

    public Task<Cooperative?> FindCooperativeAsync(long id, CancellationToken ct) =>
        profiles.FindCooperativeAsync(id, ct);

    public Task<List<Cooperative>> ListCooperativesAsync(CancellationToken ct) =>
        profiles.ListCooperativesAsync(ct);

    public Task<List<CooperativeMember>> ListMembershipsAsync(long userId, CancellationToken ct) =>
        profiles.ListMembershipsAsync(userId, ct);

    public Task<List<CooperativeMember>> ListMembersAsync(long cooperativeId, CancellationToken ct) =>
        profiles.ListMembersAsync(cooperativeId, ct);

    public Task<Profile?> FindProfileByIdAsync(long profileId, CancellationToken ct) =>
        profiles.FindProfileByIdAsync(profileId, ct);

    public Task<List<Profile>> ListProfilesAsync(long? userId, CancellationToken ct) =>
        profiles.ListProfilesAsync(userId, ct);

    public Task<Profile?> FindProfileAsync(GetProfileByUserIdQuery query, CancellationToken cancellationToken) =>
        profiles.FindProfileAsync(query.UserId, cancellationToken);

    public Task<IReadOnlyList<AgronomistAssignment>> ListActiveAssignmentsByAgronomistAsync(long userId, CancellationToken ct) =>
        profiles.ListActiveAssignmentsByAgronomistAsync(userId, ct);

    public Task<AgronomistAssignment?> FindActiveAssignmentAsync(GetActiveAssignmentByPlotQuery query, CancellationToken cancellationToken) =>
        profiles.FindActiveAssignmentAsync(query.PlotId, cancellationToken);

    public Task<IReadOnlyList<AgronomistAssignment>> ListAssignmentsAsync(GetAssignmentsByCooperativeQuery query, CancellationToken cancellationToken) =>
        profiles.ListAssignmentsAsync(query.CooperativeId, cancellationToken);
}
