using AssetOps.Domain.SeedWork;

namespace AssetOps.Api.ExceptionHandling;

public sealed class DomainExceptionHandler(
    ILogger<DomainExceptionHandler> logger,
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DomainException domainException)
        {
            return false;
        }

        logger.LogWarning(exception, "A domain exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Detail = domainException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

        return true;
    }
}
