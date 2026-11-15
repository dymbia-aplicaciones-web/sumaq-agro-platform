using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Shared.Domain.Model.Events;

namespace SumaqAgro.Platform.Iam.Domain.Model.Events;

public sealed record UserRegisteredEvent(long UserId, string FirstName, string LastName) : IEvent;
