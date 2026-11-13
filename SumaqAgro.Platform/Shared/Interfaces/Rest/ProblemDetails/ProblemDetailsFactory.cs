using Microsoft.Extensions.Localization;
using SumaqAgro.Platform.Shared.Resources.Errors;
using Microsoft.AspNetCore.Mvc;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using MvcProblemDetailsFactory = Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory;

namespace SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

public sealed class ProblemDetailsFactory(MvcProblemDetailsFactory factory, IStringLocalizer<ErrorMessages> messages)
{
    public ObjectResult CreateProblemDetails(HttpContext context, Error error)
    {
        var status = error.Code == "BUSINESS_RULE_VIOLATION" ? StatusCodes.Status422UnprocessableEntity : error.Type switch
        {
            EErrorType.Validation => StatusCodes.Status400BadRequest,
            EErrorType.NotFound => StatusCodes.Status404NotFound,
            EErrorType.Conflict => StatusCodes.Status409Conflict,
            EErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            EErrorType.Forbidden => StatusCodes.Status403Forbidden,
            EErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        var localized = messages[error.Code];
        var detail = localized.ResourceNotFound ? messages[error.Message].Value : localized.Value;
        var problem = factory.CreateProblemDetails(context, status,
            detail: status == StatusCodes.Status500InternalServerError ? null : detail,
            instance: context.Request.Path);
        problem.Extensions["code"] = error.Code;

        return new ObjectResult(problem)
        {
            StatusCode = status,
            ContentTypes = { "application/problem+json" }
        };
    }
}
