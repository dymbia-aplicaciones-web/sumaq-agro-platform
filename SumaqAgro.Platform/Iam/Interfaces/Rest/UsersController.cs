using SumaqAgro.Platform.Iam.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Iam.Application.CommandServices;
using SumaqAgro.Platform.Iam.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Iam.Interfaces.Rest;

[ApiController]
[Microsoft.AspNetCore.Authorization.AllowAnonymous]
[Route("api/v1/users")]
public sealed class UsersController(ProblemDetailsFactory problems) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<UserResource>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser(
        SignUpResource resource, [FromServices] IServiceProvider services, CancellationToken cancellationToken)
    {
        var users = services.GetRequiredService<IUserCommandService>();
        var result = await users.RegisterAsync(IamResourceAssembler.ToCommand(resource), cancellationToken);

        return result.Match<IActionResult>(
            user => new JsonResult(IamResourceAssembler.ToResource(user)) { StatusCode = StatusCodes.Status201Created },
            error => problems.CreateProblemDetails(HttpContext, error));
    }
}
