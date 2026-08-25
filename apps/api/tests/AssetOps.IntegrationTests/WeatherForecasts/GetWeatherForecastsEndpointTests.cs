using AssetOps.IntegrationTests.Common;

namespace AssetOps.IntegrationTests.WeatherForecasts;

public sealed class GetWeatherForecastsEndpointTests(ApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetWeatherForecasts_ReturnsOk()
    {
        // Act
        var response = await Client.GetAsync("/weatherforecast", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
