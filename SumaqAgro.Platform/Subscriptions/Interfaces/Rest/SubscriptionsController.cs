using SumaqAgro.Platform.Subscriptions.Domain.Model.Commands;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Queries;
using SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Iam.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Iam.Domain.Repositories;
using SumaqAgro.Platform.Shared.Interfaces.Rest;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;
using SumaqAgro.Platform.Subscriptions.Application.CommandServices;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;
using SumaqAgro.Platform.Subscriptions.Application.QueryServices;

namespace SumaqAgro.Platform.Subscriptions.Interfaces.Rest;

[ApiController]
[Route("api/v1/subscriptions/me")]
public sealed class SubscriptionsController(ISubscriptionsQueryService repository, ICheckoutService checkout,
    IUserRepository users, IConfiguration configuration, IWebHostEnvironment environment, ProblemDetailsFactory problems)
    : ResultController(problems)
{
    [HttpGet]
    [ProducesResponseType<SubscriptionResource>(200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var user = await CurrentUserAsync(ct);
        if (user is null) return Unauthorized();
        var subscription = await repository.FindSubscriptionAsync(new GetSubscriptionByUserIdQuery(user.Id), ct);
        return subscription is null ? NoContent() : new JsonResult(SubscriptionResourceAssembler.ToResource(subscription));
    }

    [HttpGet("payments")]
    [ProducesResponseType<PaymentResource[]>(200)]
    public async Task<IActionResult> GetPayments(CancellationToken ct)
    {
        var user = await CurrentUserAsync(ct);
        if (user is null) return Unauthorized();

        var payments = await repository.ListPaymentsAsync(new GetPaymentsByUserIdQuery(user.Id), ct);
        return new JsonResult(payments.Select(SubscriptionResourceAssembler.ToResource).ToArray());
    }

    [HttpPost("cancel")]
    [ProducesResponseType<SubscriptionResource>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Cancel(CancellationToken ct)
    {
        var user = await CurrentUserAsync(ct);
        if (user is null) return Unauthorized();

        var result = await checkout.CancelAsync(new CancelSubscriptionCommand(user.Id), ct);
        return Respond(result.Match(
            subscription => Result<SubscriptionResource>.Success(SubscriptionResourceAssembler.ToResource(subscription)),
            Result<SubscriptionResource>.Failure));
    }

    [HttpPost("checkout")]
    [ProducesResponseType<PaymentResource>(201)]
    public async Task<IActionResult> Pay(CheckoutResource resource, CancellationToken ct)
    {
        if (!configuration.GetValue("Subscriptions:DemoPayments:Enabled", environment.IsDevelopment()))
            return NotFound();

        var user = await CurrentUserAsync(ct);
        if (user is null) return Unauthorized();
        var result = await checkout.CheckoutAsync(SubscriptionResourceAssembler.ToCommand(user.Id, user.Role, resource), ct);
        return result.DomainError.Type == EErrorType.Forbidden ? Forbid() : Respond(result.Match(payment => Result<PaymentResource>.Success(SubscriptionResourceAssembler.ToResource(payment)), Result<PaymentResource>.Failure), 201);
    }

    private Task<User?> CurrentUserAsync(CancellationToken ct) =>
        User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value is { Length: > 0 } email
            ? users.FindByEmailAsync(new EmailAddress(email), ct)
            : Task.FromResult<User?>(null);
}
