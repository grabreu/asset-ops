namespace AssetOps.Api;

public static class ErrorOrMinimalApiExtensions
{
    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Results.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    private static IResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(statusCode: statusCode, detail: error.Description);
    }

    private static Microsoft.AspNetCore.Http.HttpResults.ValidationProblem ValidationProblem(List<Error> errors)
    {
        var errorsDictionary = errors
            .GroupBy(error => error.Code)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Description).ToArray());

        return TypedResults.ValidationProblem(errorsDictionary);
    }

    public static IResult ToCreated<T>(this ErrorOr<T> result, Func<T, string> location)
    {
        return result.Match(
            successValue => TypedResults.Created(location(successValue), successValue),
            errors => errors.ToProblem());
    }

    public static IResult ToOk<T>(this ErrorOr<T> result)
    {
        return result.Match(
            successValue => TypedResults.Ok(successValue),
            errors => errors.ToProblem());
    }
}
