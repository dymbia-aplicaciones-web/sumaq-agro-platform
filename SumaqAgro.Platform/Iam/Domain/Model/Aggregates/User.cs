using SumaqAgro.Platform.Iam.Domain.Model.Events;
using SumaqAgro.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace SumaqAgro.Platform.Iam.Domain.Model.Aggregates;

public sealed class User(string email, string passwordHash, string role) : AggregateRoot
{
    public long Id { get; private set; }
    public string Email { get; private set; } = email;
    public string PasswordHash { get; private set; } = passwordHash;
    public string Role { get; private set; } = role;

    public void ChangePassword(string hash) => PasswordHash = hash;

    public void RecordRegistration(string firstName, string lastName) =>
        AddDomainEvent(new UserRegisteredEvent(Id, firstName, lastName));
}

