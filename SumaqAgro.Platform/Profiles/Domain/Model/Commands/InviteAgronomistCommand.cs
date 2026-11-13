namespace SumaqAgro.Platform.Profiles.Domain.Model.Commands;

public sealed record InviteAgronomistCommand(long CooperativeId, string Email);
