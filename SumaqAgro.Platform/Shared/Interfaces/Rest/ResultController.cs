using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SumaqAgro.Platform.Shared.Interfaces.Rest;

public abstract class ResultController(ProblemDetailsFactory problems) : ControllerBase
{
    protected IActionResult Respond<T>(Result<T> result, int status = StatusCodes.Status200OK) =>
        result.Match<IActionResult>(
            value => new JsonResult(value) { StatusCode = status },
            error => problems.CreateProblemDetails(HttpContext, error));

    protected IActionResult Found<T>(T? value, string code) where T : class =>
        value is null
            ? problems.CreateProblemDetails(HttpContext, Error.NotFound(code, "The requested resource was not found."))
            : new JsonResult(value);
}
