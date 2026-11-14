using SumaqAgro.Platform.Subscriptions.Domain.Model.Queries;
using SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Transform;
using SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Resources;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Subscriptions.Application.QueryServices;

namespace SumaqAgro.Platform.Subscriptions.Interfaces.Rest;

[ApiController]
[Route("api/v1/plans")]
public sealed class PlansController(ISubscriptionsQueryService repository) : ControllerBase
{
    [HttpGet]
    public async Task<PlanResource[]> List(CancellationToken ct) =>
        (await repository.ListPlansAsync(new GetPlansQuery(), ct)).Select(SubscriptionResourceAssembler.ToResource).ToArray();
}
