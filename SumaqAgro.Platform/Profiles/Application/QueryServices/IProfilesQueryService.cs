using SumaqAgro.Platform.Profiles.Domain.Model.Queries;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Application.QueryServices;

public interface IProfilesQueryService
{
    Task<Profile?> FindProfileAsync(GetProfileByUserIdQuery query, CancellationToken cancellationToken);

    Task<AgronomistAssignment?> FindActiveAssignmentAsync(GetActiveAssignmentByPlotQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyList<AgronomistAssignment>> ListAssignmentsAsync(GetAssignmentsByCooperativeQuery query, CancellationToken cancellationToken);
}
