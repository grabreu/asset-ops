using AssetOps.Api.ExceptionHandling;

namespace AssetOps.UnitTests.Api.ExceptionHandling;

public class GlobalExceptionHandlerTests
{
    private readonly IHostEnvironment _environment = Substitute.For<IHostEnvironment>();
    private readonly IProblemDetailsService _problemDetailsService = Substitute.For<IProblemDetailsService>();

    private GlobalExceptionHandler CreateHandler() => new(
        Substitute.For<ILogger<GlobalExceptionHandler>>(),
        _environment,
        _problemDetailsService);

    [Fact]
    public async Task TryHandleAsync_AlwaysReturnsTrueAndWritesInternalServerErrorStatus()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var handler = CreateHandler();

        // Act
        var handled = await handler.TryHandleAsync(
            httpContext, new InvalidOperationException("boom"), TestContext.Current.CancellationToken);

        // Assert
        handled.ShouldBeTrue();
        httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task TryHandleAsync_InDevelopment_ExposesExceptionMessage()
    {
        // Arrange
        _environment.EnvironmentName.Returns(Environments.Development);
        var httpContext = new DefaultHttpContext();
        var exception = new InvalidOperationException("boom");
        var handler = CreateHandler();

        // Act
        await handler.TryHandleAsync(httpContext, exception, TestContext.Current.CancellationToken);

        // Assert
        await _problemDetailsService.Received(1).WriteAsync(Arg.Is<ProblemDetailsContext>(context =>
            context.ProblemDetails.Detail == exception.Message));
    }

    [Fact]
    public async Task TryHandleAsync_OutsideDevelopment_HidesExceptionMessage()
    {
        // Arrange
        _environment.EnvironmentName.Returns(Environments.Production);
        var httpContext = new DefaultHttpContext();
        var handler = CreateHandler();

        // Act
        await handler.TryHandleAsync(
            httpContext, new InvalidOperationException("boom"), TestContext.Current.CancellationToken);

        // Assert
        await _problemDetailsService.Received(1).WriteAsync(Arg.Is<ProblemDetailsContext>(context =>
            context.ProblemDetails.Detail == "Something went wrong. Please try again later."));
    }
}
