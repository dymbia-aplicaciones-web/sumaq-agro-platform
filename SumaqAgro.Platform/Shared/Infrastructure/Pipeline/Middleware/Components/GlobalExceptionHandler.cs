using Microsoft.AspNetCore.Diagnostics;

namespace SumaqAgro.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

public sealed class GlobalExceptionHandler(IProblemDetailsService problems) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return problems.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = { Status = StatusCodes.Status500InternalServerError }
        });
    }
}
