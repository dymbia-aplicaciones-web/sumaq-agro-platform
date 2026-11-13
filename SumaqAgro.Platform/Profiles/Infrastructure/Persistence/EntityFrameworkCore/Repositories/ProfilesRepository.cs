using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Profiles.Domain.Repositories;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class ProfilesRepository(AppDbContext context) : IProfilesRepository
{
    public void AddProfile(Profile profile) => context.Add(profile);
    public void AddCooperative(Cooperative cooperative) => context.Add(cooperative);
    public void AddMember(CooperativeMember member) => context.Add(member);
    public void AddInvitation(AgronomistInvitation invitation) => context.Add(invitation);
    public void AddAssignment(AgronomistAssignment assignment) => context.Add(assignment);

    public Task<Profile?> FindProfileAsync(long userId, CancellationToken cancellationToken) =>
        context.Set<Profile>().SingleOrDefaultAsync(profile => profile.UserId == userId, cancellationToken);

    public Task<bool> CooperativeExistsAsync(long id, CancellationToken cancellationToken) =>
        context.Set<Cooperative>().AnyAsync(cooperative => cooperative.Id == id, cancellationToken);

    public Task<AgronomistAssignment?> FindActiveAssignmentAsync(long plotId, CancellationToken cancellationToken) =>
        context.Set<AgronomistAssignment>().AsNoTracking()
            .SingleOrDefaultAsync(assignment => assignment.PlotId == plotId && assignment.Active, cancellationToken);

    public async Task<IReadOnlyList<AgronomistAssignment>> ListAssignmentsAsync(
        long cooperativeId, CancellationToken cancellationToken) =>
        await context.Set<AgronomistAssignment>().AsNoTracking()
            .Where(assignment => assignment.CooperativeId == cooperativeId)
            .OrderBy(assignment => assignment.Id).ToListAsync(cancellationToken);
}
