using SumaqAgro.Platform.Subscriptions.Domain.Model.Commands;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;

namespace SumaqAgro.Platform.Subscriptions.Application.CommandServices;

public interface ICheckoutService
{
    Task<Result<Payment>> CheckoutAsync(CheckoutCommand command, CancellationToken ct);
}
