using SumaqAgro.Platform.Iam.Domain.Model.Events;
using SumaqAgro.Platform.Profiles.Interfaces.Acl;
using SumaqAgro.Platform.Shared.Application.Internal.EventHandlers;

namespace SumaqAgro.Platform.Iam.Application.Internal.EventHandlers;

public sealed class UserRegisteredEventHandler(IProfilesContextFacade profiles) : IEventHandler<UserRegisteredEvent>
{
    public Task Handle(UserRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        profiles.CreateInitialProfile(domainEvent.User, domainEvent.FirstName, domainEvent.LastName);
        return Task.CompletedTask;
    }
}
