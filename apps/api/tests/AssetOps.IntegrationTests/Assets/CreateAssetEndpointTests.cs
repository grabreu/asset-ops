using AssetOps.Api.Endpoints.Assets;
using AssetOps.Application.Models.Assets;
using AssetOps.Domain.Assets;
using AssetOps.IntegrationTests.Common;

namespace AssetOps.IntegrationTests.Assets;

public sealed class CreateAssetEndpointTests(ApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateAsset_WithValidRequest_ReturnsCreatedWithLocation()
    {
        // Arrange
        var request = new CreateAssetRequest("AT-0001", "Sample Asset");

        // Act
        var response = await Client.PostAsJsonAsync("/assets", request, JsonOptions, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var asset = await response.Content.ReadFromJsonAsync<AssetSummaryDto>(JsonOptions, TestContext.Current.CancellationToken);
        asset.ShouldNotBeNull();
        asset.Tag.ShouldBe("AT-0001");
        asset.Name.ShouldBe("Sample Asset");
        asset.Status.ShouldBe(AssetStatus.Available);

        response.Headers.Location.ShouldBe(new Uri($"/assets/{asset.Id}", UriKind.Relative));
    }

    [Fact]
    public async Task CreateAsset_WithDuplicateTag_ReturnsConflict()
    {
        // Arrange
        var existingAsset = await SeedAssetAsync();
        var request = new CreateAssetRequest(existingAsset.Tag, "Another Asset");

        // Act
        var response = await Client.PostAsJsonAsync("/assets", request, JsonOptions, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task CreateAsset_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateAssetRequest("", "Sample Asset");

        // Act
        var response = await Client.PostAsJsonAsync("/assets", request, JsonOptions, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
