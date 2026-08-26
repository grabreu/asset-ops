using AssetOps.Api;

namespace AssetOps.UnitTests.Api;

public class ErrorOrMinimalApiExtensionsTests
{
    [Fact]
    public void ToOk_WithSuccessValue_ReturnsOkResult()
    {
        // Arrange
        ErrorOr<string> result = "value";

        // Act
        var httpResult = result.ToOk();

        // Assert
        var ok = httpResult.ShouldBeOfType<Ok<string>>();
        ok.Value.ShouldBe("value");
    }

    [Fact]
    public void ToOk_WithSingleError_ReturnsProblemWithDetail()
    {
        // Arrange
        ErrorOr<string> result = Error.NotFound(description: "Asset not found.");

        // Act
        var httpResult = result.ToOk();

        // Assert
        var problem = httpResult.ShouldBeOfType<ProblemHttpResult>();
        problem.ProblemDetails.Status.ShouldBe(StatusCodes.Status404NotFound);
        problem.ProblemDetails.Detail.ShouldBe("Asset not found.");
    }

    [Theory]
    [InlineData(ErrorType.Conflict, StatusCodes.Status409Conflict)]
    [InlineData(ErrorType.NotFound, StatusCodes.Status404NotFound)]
    [InlineData(ErrorType.Unauthorized, StatusCodes.Status401Unauthorized)]
    [InlineData(ErrorType.Forbidden, StatusCodes.Status403Forbidden)]
    [InlineData(ErrorType.Failure, StatusCodes.Status500InternalServerError)]
    public void ToOk_MapsErrorTypeToExpectedStatusCode(ErrorType errorType, int expectedStatus)
    {
        // Arrange
        ErrorOr<string> result = CreateError(errorType);

        // Act
        var httpResult = result.ToOk();

        // Assert
        var problem = httpResult.ShouldBeOfType<ProblemHttpResult>();
        problem.ProblemDetails.Status.ShouldBe(expectedStatus);
    }

    [Fact]
    public void ToOk_WithValidationErrors_ReturnsValidationProblemGroupedByCode()
    {
        // Arrange
        ErrorOr<string> result = new List<Error>
        {
            Error.Validation("Tag", "Tag is required."),
            Error.Validation("Tag", "Tag is too long."),
            Error.Validation("Name", "Name is required."),
        };

        // Act
        var httpResult = result.ToOk();

        // Assert
        var validationProblem = httpResult.ShouldBeOfType<ValidationProblem>();
        validationProblem.ProblemDetails.Errors["Tag"].ShouldBe(
            ["Tag is required.", "Tag is too long."]);
        validationProblem.ProblemDetails.Errors["Name"].ShouldBe(["Name is required."]);
    }

    [Fact]
    public void ToCreated_WithSuccessValue_ReturnsCreatedAtLocation()
    {
        // Arrange
        ErrorOr<string> result = "value";

        // Act
        var httpResult = result.ToCreated(value => $"/things/{value}");

        // Assert
        var created = httpResult.ShouldBeOfType<Created<string>>();
        created.Location.ShouldBe("/things/value");
        created.Value.ShouldBe("value");
    }

    [Fact]
    public void ToCreated_WithError_ReturnsProblem()
    {
        // Arrange
        ErrorOr<string> result = Error.Conflict();

        // Act
        var httpResult = result.ToCreated(value => $"/things/{value}");

        // Assert
        httpResult.ShouldBeOfType<ProblemHttpResult>();
    }

    private static Error CreateError(ErrorType type) => type switch
    {
        ErrorType.Conflict => Error.Conflict(),
        ErrorType.NotFound => Error.NotFound(),
        ErrorType.Unauthorized => Error.Unauthorized(),
        ErrorType.Forbidden => Error.Forbidden(),
        ErrorType.Failure => Error.Failure(),
        _ => throw new ArgumentOutOfRangeException(nameof(type)),
    };
}
