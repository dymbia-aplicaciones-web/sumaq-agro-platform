namespace SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;

public sealed record AgronomistInvitationResource(long Id, long CooperativeId, string Email, string Status, DateTime SentAt);
