using SumaqAgro.Platform.Iam.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Iam.Application.CommandServices;
using SumaqAgro.Platform.Iam.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Iam.Interfaces.Rest;

[ApiController]
[AllowAnonymous]
[Route("api/v1/tokens")]
public sealed class TokensController(ProblemDetailsFactory problems) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<AuthenticatedUserResource>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GenerateToken(
        SignInResource resource, [FromServices] IServiceProvider services, CancellationToken cancellationToken)
    {
        var signIn = services.GetRequiredService<ISignInService>();
        var result = await signIn.SignInAsync(IamResourceAssembler.ToCommand(resource), cancellationToken);
        return result.Match<IActionResult>(
            session => new JsonResult(IamResourceAssembler.ToResource(session.User, session.Token)),
            error => problems.CreateProblemDetails(HttpContext, error));
    }
}
