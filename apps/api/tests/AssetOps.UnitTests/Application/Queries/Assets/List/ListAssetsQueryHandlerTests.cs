using AssetOps.Application.Models.Assets;
using AssetOps.Application.Queries;
using AssetOps.Application.Queries.Assets.List;
using AssetOps.Domain.Assets;

namespace AssetOps.UnitTests.Application.Queries.Assets.List;

public class ListAssetsQueryHandlerTests
{
    private readonly IAssetQueries _queries;
    private readonly ListAssetsQueryHandler _handler;

    public ListAssetsQueryHandlerTests()
    {
        _queries = Substitute.For<IAssetQueries>();
        _handler = new ListAssetsQueryHandler(_queries);
    }

    private static AssetSummaryDto SampleAsset() => new(
        Guid.CreateVersion7(),
        "AT-0001",
        "Sample Asset",
        AssetStatus.Available,
        null,
        DateTimeOffset.UtcNow);

    [Fact]
    public async Task Handle_WithValidQuery_ReturnsListFromQueries()
    {
        // Arrange
        var list = new List<AssetSummaryDto> { SampleAsset() };
        _queries.ListAsync(Arg.Any<CancellationToken>()).Returns(list);

        // Act
        var result = await _handler.Handle(new ListAssetsQuery(), TestContext.Current.CancellationToken);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBe(list);
    }
}
