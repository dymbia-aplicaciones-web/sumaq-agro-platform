using SumaqAgro.Platform.Iam.Domain.Model.Events;
using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Shared.Application.Internal.EventHandlers;

namespace SumaqAgro.Platform.Iam.Application.Internal.EventHandlers;

public sealed class UserRegisteredEventHandler(IInitialProfiles profiles) : IEventHandler<UserRegisteredEvent>
{
    public Task Handle(UserRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        profiles.Create(domainEvent.UserId, domainEvent.FirstName, domainEvent.LastName);
        return Task.CompletedTask;
    }
}
