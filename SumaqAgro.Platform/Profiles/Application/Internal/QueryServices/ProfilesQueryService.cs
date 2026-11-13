using SumaqAgro.Platform.Profiles.Domain.Model.Queries;
using SumaqAgro.Platform.Profiles.Application.QueryServices;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Profiles.Domain.Repositories;

namespace SumaqAgro.Platform.Profiles.Application.Internal.QueryServices;

public sealed class ProfilesQueryService(IProfilesRepository profiles) : IProfilesQueryService
{
    public Task<Profile?> FindProfileAsync(GetProfileByUserIdQuery query, CancellationToken cancellationToken) =>
        profiles.FindProfileAsync(query.UserId, cancellationToken);

    public Task<AgronomistAssignment?> FindActiveAssignmentAsync(GetActiveAssignmentByPlotQuery query, CancellationToken cancellationToken) =>
        profiles.FindActiveAssignmentAsync(query.PlotId, cancellationToken);

    public Task<IReadOnlyList<AgronomistAssignment>> ListAssignmentsAsync(GetAssignmentsByCooperativeQuery query, CancellationToken cancellationToken) =>
        profiles.ListAssignmentsAsync(query.CooperativeId, cancellationToken);
}
