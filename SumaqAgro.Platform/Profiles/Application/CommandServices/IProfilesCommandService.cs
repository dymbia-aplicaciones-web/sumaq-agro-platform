using SumaqAgro.Platform.Profiles.Domain.Model.Commands;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Application.CommandServices;

public interface IProfilesCommandService
{
    Task<Result<Profile>> UpdateAsync(UpdateProfileCommand command, CancellationToken cancellationToken);

    Task<Result<Cooperative>> RegisterAsync(RegisterCooperativeCommand command, CancellationToken cancellationToken);

    Task<Result<CooperativeMember>> AddMemberAsync(AddCooperativeMemberCommand command, CancellationToken cancellationToken);

    Task<Result<AgronomistInvitation>> InviteAsync(InviteAgronomistCommand command, CancellationToken cancellationToken);

    Task<Result<AgronomistAssignment>> AssignAsync(AssignAgronomistCommand command, CancellationToken cancellationToken);
}
