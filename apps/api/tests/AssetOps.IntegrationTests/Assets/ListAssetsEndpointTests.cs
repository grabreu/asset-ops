using AssetOps.Application.Models.Assets;
using AssetOps.IntegrationTests.Common;

namespace AssetOps.IntegrationTests.Assets;

public sealed class ListAssetsEndpointTests(ApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task ListAssets_WithSeededAssets_ReturnsList()
    {
        // Arrange
        var asset = await SeedAssetAsync();

        // Act
        var response = await Client.GetAsync("/assets", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<AssetSummaryDto>>(JsonOptions, TestContext.Current.CancellationToken);
        var summary = result.ShouldHaveSingleItem();
        summary.Id.ShouldBe(asset.Id);
        summary.Tag.ShouldBe(asset.Tag);
        summary.Name.ShouldBe(asset.Name);
        summary.Status.ShouldBe(asset.Status);
        summary.CurrentHolder.ShouldBeNull();
    }

    [Fact]
    public async Task ListAssets_WithNoAssets_ReturnsEmptyList()
    {
        // Act
        var response = await Client.GetAsync("/assets", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<AssetSummaryDto>>(JsonOptions, TestContext.Current.CancellationToken);
        result.ShouldBeEmpty();
    }
}
