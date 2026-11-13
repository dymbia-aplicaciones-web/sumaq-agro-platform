namespace SumaqAgro.Platform.Profiles.Domain.Model.Entities;

public sealed class CooperativeMember(long cooperativeId, string dni, string firstName, string lastName, string community, DateOnly joinedAt)
{
    public long Id { get; private set; }
    public long CooperativeId { get; private set; } = cooperativeId;
    public string Dni { get; private set; } = dni;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public string Community { get; private set; } = community;
    public DateOnly JoinedAt { get; private set; } = joinedAt;
}
