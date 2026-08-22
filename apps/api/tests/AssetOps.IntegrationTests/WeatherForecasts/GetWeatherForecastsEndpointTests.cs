using AssetOps.IntegrationTests.Common;

namespace AssetOps.IntegrationTests.WeatherForecasts;

[Collection(IntegrationTestCollection.Name)]
public sealed class GetWeatherForecastsEndpointTests(ApiFactory factory)
{
    [Fact]
    public async Task GetWeatherForecasts_ReturnsOk()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/weatherforecast", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
