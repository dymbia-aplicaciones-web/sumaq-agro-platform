namespace SumaqAgro.Platform.Profiles.Domain.Model.Entities;

public sealed class AgronomistInvitation(long cooperativeId, string email)
{
    public long Id { get; private set; }
    public long CooperativeId { get; private set; } = cooperativeId;
    public string Email { get; private set; } = email;
    public string Status { get; private set; } = "PENDING";
    public DateTime SentAt { get; private set; } = DateTime.UtcNow;
}
