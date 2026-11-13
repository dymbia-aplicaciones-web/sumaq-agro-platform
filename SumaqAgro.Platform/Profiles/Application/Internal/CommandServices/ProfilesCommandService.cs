using SumaqAgro.Platform.Profiles.Domain.Model.Commands;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Profiles.Domain.Model.Errors;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using SumaqAgro.Platform.Profiles.Application.CommandServices;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Profiles.Domain.Repositories;
using SumaqAgro.Platform.Shared.Domain.Repositories;

namespace SumaqAgro.Platform.Profiles.Application.Internal.CommandServices;

public sealed class ProfilesCommandService(IProfilesRepository profiles, IUnitOfWork unitOfWork) : IProfilesCommandService
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

    public Task<Result<Cooperative>> RegisterAsync(RegisterCooperativeCommand command, CancellationToken cancellationToken) =>
        SaveAsync(new Cooperative(command.Ruc.Value, command.Name.Trim(), command.Region.Trim(), command.ManagerId), profiles.AddCooperative, ProfilesErrors.DuplicateRuc, cancellationToken);

    public async Task<Result<CooperativeMember>> AddMemberAsync(AddCooperativeMemberCommand command, CancellationToken cancellationToken)
    {
        var (cooperativeId, dni, firstName, lastName, community, joinedAt) = command;
        if (!await profiles.CooperativeExistsAsync(cooperativeId, cancellationToken)) return Result<CooperativeMember>.Failure(ProfilesErrors.CooperativeNotFound);

        var member = new CooperativeMember(cooperativeId, dni.Value, firstName.Trim(), lastName.Trim(),
            community.Trim(), joinedAt ?? DateOnly.FromDateTime(DateTime.Today));
        return await SaveAsync(member, profiles.AddMember, ProfilesErrors.DuplicateDni, cancellationToken);
    }

    public async Task<Result<AgronomistInvitation>> InviteAsync(InviteAgronomistCommand command, CancellationToken cancellationToken)
    {
        var (cooperativeId, email) = command;
        if (!await profiles.CooperativeExistsAsync(cooperativeId, cancellationToken)) return Result<AgronomistInvitation>.Failure(ProfilesErrors.CooperativeNotFound);

        var invitation = new AgronomistInvitation(cooperativeId, email.Trim().ToLowerInvariant());
        return await SaveAsync(invitation, profiles.AddInvitation, null, cancellationToken);
    }

    public Task<Result<AgronomistAssignment>> AssignAsync(AssignAgronomistCommand command, CancellationToken cancellationToken) =>
        SaveAsync(new AgronomistAssignment(command.CooperativeId, command.AgronomistUserId, command.PlotId), profiles.AddAssignment, ProfilesErrors.ActiveAssignment, cancellationToken);

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
