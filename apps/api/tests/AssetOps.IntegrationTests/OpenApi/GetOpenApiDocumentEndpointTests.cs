using AssetOps.IntegrationTests.Common;

namespace AssetOps.IntegrationTests.OpenApi;

public sealed class GetOpenApiDocumentEndpointTests(ApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetOpenApiDocument_ReturnsOk()
    {
        // Act
        var response = await Client.GetAsync("/openapi/v1.json", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
