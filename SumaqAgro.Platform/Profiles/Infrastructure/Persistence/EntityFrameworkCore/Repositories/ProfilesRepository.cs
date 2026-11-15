using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Profiles.Domain.Repositories;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class ProfilesRepository(AppDbContext context) : IProfilesRepository
{
    public Task<Profile?> FindProfileByIdAsync(long profileId, CancellationToken ct) =>
        context.Set<Profile>().AsNoTracking().SingleOrDefaultAsync(profile => profile.Id == profileId, ct);

    public Task<List<Profile>> ListProfilesAsync(long? userId, CancellationToken ct) =>
        context.Set<Profile>().AsNoTracking()
            .Where(profile => userId == null || profile.UserId == userId)
            .OrderBy(profile => profile.Id).ToListAsync(ct);

    public Task<Cooperative?> FindCooperativeAsync(long id, CancellationToken ct) =>
        context.Set<Cooperative>().SingleOrDefaultAsync(cooperative => cooperative.Id == id, ct);

    public Task<List<Cooperative>> ListCooperativesAsync(CancellationToken ct) =>
        context.Set<Cooperative>().AsNoTracking().OrderBy(cooperative => cooperative.Id).ToListAsync(ct);

    public Task<List<CooperativeMember>> ListMembershipsAsync(long userId, CancellationToken ct) =>
        context.Set<CooperativeMember>().AsNoTracking().Where(member => member.UserId == userId)
            .OrderBy(member => member.Id).ToListAsync(ct);

    public Task<List<CooperativeMember>> ListMembersAsync(long cooperativeId, CancellationToken ct) =>
        context.Set<CooperativeMember>().AsNoTracking().Where(member => member.CooperativeId == cooperativeId)
            .OrderBy(member => member.Id).ToListAsync(ct);

    public void AddProfile(Profile profile) => context.Add(profile);
    public void AddCooperative(Cooperative cooperative) => context.Add(cooperative);
    public Task<bool> MemberBelongsAsync(long cooperativeId, long memberId, CancellationToken ct) =>
        context.Set<CooperativeMember>().AnyAsync(member => member.Id == memberId && member.CooperativeId == cooperativeId, ct);

    public void AddMember(CooperativeMember member) => context.Add(member);
    public void AddInvitation(AgronomistInvitation invitation) => context.Add(invitation);
    public Task<AgronomistAssignment?> FindAssignmentAsync(long id, CancellationToken ct) =>
        context.Set<AgronomistAssignment>().SingleOrDefaultAsync(assignment => assignment.Id == id, ct);

    public Task<int> CountActiveAssignmentsAsync(long userId, CancellationToken ct) =>
        context.Set<AgronomistAssignment>().CountAsync(assignment => assignment.Active && assignment.AgronomistUserId == userId, ct);

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
