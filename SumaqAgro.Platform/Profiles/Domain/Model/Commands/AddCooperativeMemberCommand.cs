using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Profiles.Domain.Model.Commands;

public sealed record AddCooperativeMemberCommand(long CooperativeId, Dni Dni, string FirstName, string LastName, string Community, DateOnly? JoinedAt);
