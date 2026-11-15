using SumaqAgro.Platform.Profiles.Domain.Model.Commands;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Profiles.Domain.Model.Errors;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using SumaqAgro.Platform.Profiles.Application.CommandServices;
using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Profiles.Application.Internal.OutboundServices;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Profiles.Domain.Repositories;
using SumaqAgro.Platform.Shared.Domain.Repositories;

namespace SumaqAgro.Platform.Profiles.Application.Internal.CommandServices;

public sealed class ProfilesCommandService(IProfilesRepository profiles, IUnitOfWork unitOfWork, IAdvisorPlotQuota advisorQuota, TimeProvider clock) : IProfilesCommandService
{
    public async Task<Result<Profile>> UpdateAsync(UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var (userId, firstName, lastName, phoneNumber, photoUrl, language) = command;
        var profile = await profiles.FindProfileAsync(userId, cancellationToken);
        if (profile is null) return Result<Profile>.Failure(ProfilesErrors.ProfileNotFound);

        profile.Update(firstName, lastName, phoneNumber, photoUrl, language);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Profile>.Success(profile);
    }

    public async Task<Result<Cooperative>> UpdateCooperativeAsync(long id, RegisterCooperativeCommand command, CancellationToken ct)
    {
        var cooperative = await profiles.FindCooperativeAsync(id, ct);
        if (cooperative is null)
            return Result<Cooperative>.Failure(ProfilesErrors.CooperativeNotFound);

        cooperative.Update(command.Ruc.Value, command.Name.Trim(), command.Region.Trim(), command.ManagerId);
        try
        {
            await unitOfWork.CompleteAsync(ct);
            return Result<Cooperative>.Success(cooperative);
        }
        catch (DbUpdateException exception) when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return Result<Cooperative>.Failure(ProfilesErrors.DuplicateRuc);
        }
    }

    public Task<Result<Cooperative>> RegisterAsync(RegisterCooperativeCommand command, CancellationToken cancellationToken) =>
        SaveAsync(new Cooperative(command.Ruc.Value, command.Name.Trim(), command.Region.Trim(), command.ManagerId), profiles.AddCooperative, ProfilesErrors.DuplicateRuc, cancellationToken);

    public async Task<Result<CooperativeMember>> AddMemberAsync(AddCooperativeMemberCommand command, CancellationToken cancellationToken)
    {
        var (cooperativeId, dni, firstName, lastName, community, joinedAt, userId) = command;
        if (!await profiles.CooperativeExistsAsync(cooperativeId, cancellationToken)) return Result<CooperativeMember>.Failure(ProfilesErrors.CooperativeNotFound);

        var member = new CooperativeMember(cooperativeId, dni.Value, firstName.Trim(), lastName.Trim(),
            community.Trim(), joinedAt ?? DateOnly.FromDateTime(DateTime.Today), userId);
        return await SaveAsync(member, profiles.AddMember, ProfilesErrors.DuplicateDni, cancellationToken);
    }

    public async Task<Result<AgronomistInvitation>> InviteAsync(InviteAgronomistCommand command, CancellationToken cancellationToken)
    {
        var (cooperativeId, email) = command;
        if (!await profiles.CooperativeExistsAsync(cooperativeId, cancellationToken)) return Result<AgronomistInvitation>.Failure(ProfilesErrors.CooperativeNotFound);

        var invitation = new AgronomistInvitation(cooperativeId, email.Trim().ToLowerInvariant());
        return await SaveAsync(invitation, profiles.AddInvitation, null, cancellationToken);
    }

    public async Task<Result<AgronomistAssignment>> AssignAsync(AssignAgronomistCommand command, CancellationToken ct)
    {
        await using var transaction = await advisorQuota.LockAccountAsync(command.AgronomistUserId, ct);
        if (await CheckQuotaAsync(command.AgronomistUserId, ct) is { } error)
            return Result<AgronomistAssignment>.Failure(error);

        var result = await SaveAsync(new AgronomistAssignment(command.CooperativeId, command.AgronomistUserId, command.PlotId),
            profiles.AddAssignment, ProfilesErrors.ActiveAssignment, ct);
        if (!result.IsFailure) await transaction.CommitAsync(ct);
        return result;
    }

    public async Task<Result<AgronomistAssignment>> ReassignAsync(long id, long userId, CancellationToken ct)
    {
        await using var transaction = await advisorQuota.LockAccountAsync(userId, ct);
        var assignment = await profiles.FindAssignmentAsync(id, ct);
        if (assignment is null)
            return Result<AgronomistAssignment>.Failure(Error.NotFound("Assignment", "Assignment not found."));
        if (!assignment.Active)
            return Result<AgronomistAssignment>.Failure(Error.Conflict("Assignment", "The assignment is inactive."));
        if (assignment.AgronomistUserId == userId)
            return Result<AgronomistAssignment>.Success(assignment);
        if (await CheckQuotaAsync(userId, ct) is { } error)
            return Result<AgronomistAssignment>.Failure(error);

        assignment.Reassign(userId, clock.GetUtcNow().UtcDateTime);
        try
        {
            await unitOfWork.CompleteAsync(ct);
            await transaction.CommitAsync(ct);
            return Result<AgronomistAssignment>.Success(assignment);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<AgronomistAssignment>.Failure(Error.Conflict("Assignment", "The assignment changed. Retry the request."));
        }
    }

    private async Task<Error?> CheckQuotaAsync(long userId, CancellationToken ct)
    {
        var limit = await advisorQuota.GetCurrentPlotLimitAsync(userId, ct);
        if (limit is null)
            return Error.Forbidden("AdvisorPlanRequired", "A current Technical Advisor subscription is required.");

        if (await profiles.CountActiveAssignmentsAsync(userId, ct) >= limit.Value)
            return Error.Conflict("AdvisorPlotQuota", "The agronomist has reached the active plot assignment limit.");
        return null;
    }

    private async Task<Result<T>> SaveAsync<T>(T entity, Action<T> add, Error? conflict, CancellationToken cancellationToken) where T : class
    {
        add(entity);
        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<T>.Success(entity);
        }
        catch (DbUpdateException exception) when (conflict is not null && exception.InnerException is MySqlException { Number: 1062 })
        {
            return Result<T>.Failure(conflict!);
        }
    }
}
