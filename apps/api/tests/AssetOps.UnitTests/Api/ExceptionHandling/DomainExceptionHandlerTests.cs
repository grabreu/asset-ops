using AssetOps.Api.ExceptionHandling;
using AssetOps.Domain.SeedWork;

namespace AssetOps.UnitTests.Api.ExceptionHandling;

public class DomainExceptionHandlerTests
{
    private sealed class TestDomainException() : DomainException("Something went wrong in the domain.");

    private readonly IProblemDetailsService _problemDetailsService = Substitute.For<IProblemDetailsService>();
    private readonly DomainExceptionHandler _handler;

    public DomainExceptionHandlerTests()
    {
        _handler = new DomainExceptionHandler(
            Substitute.For<ILogger<DomainExceptionHandler>>(),
            _problemDetailsService);
    }

    [Fact]
    public async Task TryHandleAsync_WithDomainException_WritesBadRequestProblemAndReturnsTrue()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var exception = new TestDomainException();

        // Act
        var handled = await _handler.TryHandleAsync(
            httpContext, exception, TestContext.Current.CancellationToken);

        // Assert
        handled.ShouldBeTrue();
        httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);

        await _problemDetailsService.Received(1).WriteAsync(Arg.Is<ProblemDetailsContext>(context =>
            context.ProblemDetails.Status == StatusCodes.Status400BadRequest &&
            context.ProblemDetails.Detail == exception.Message));
    }

    [Fact]
    public async Task TryHandleAsync_WithNonDomainException_ReturnsFalse()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        // Act
        var handled = await _handler.TryHandleAsync(
            httpContext, new InvalidOperationException("Not a domain exception."), TestContext.Current.CancellationToken);

        // Assert
        handled.ShouldBeFalse();
        await _problemDetailsService.DidNotReceive().WriteAsync(Arg.Any<ProblemDetailsContext>());
    }
}
