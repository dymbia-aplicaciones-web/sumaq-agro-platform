using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Iam.Application.CommandServices;
using SumaqAgro.Platform.Iam.Interfaces.Rest.Resources;

namespace SumaqAgro.Platform.Iam.Interfaces.Rest;

[ApiController]
[AllowAnonymous]
[Route("api/v1/authentication/password-recovery")]
public sealed class PasswordRecoveryController(IPasswordRecoveryService recovery) : ControllerBase
{
    [HttpPost("request")]
    public async Task<IActionResult> RequestReset(RequestPasswordResetResource resource, CancellationToken ct)
    {
        await recovery.RequestAsync(resource.Email, ct);
        return Accepted();
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset(ResetPasswordResource resource, CancellationToken ct) =>
        await recovery.ResetAsync(resource.Token, resource.NewPassword, ct)
            ? NoContent()
            : Problem(statusCode: 400, detail: "The token is invalid or expired, or the password is invalid.");
}
