namespace SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;

public sealed record CooperativeMemberResource(long Id, long CooperativeId, string Dni, string FirstName, string LastName, string Community, DateOnly JoinedAt);
