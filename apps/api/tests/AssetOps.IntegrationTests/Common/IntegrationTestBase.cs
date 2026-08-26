using AssetOps.Domain.Assets;
using AssetOps.Infrastructure.Persistence;

namespace AssetOps.IntegrationTests.Common;

[Collection(IntegrationTestCollection.Name)]
public abstract class IntegrationTestBase(ApiFactory factory) : IAsyncLifetime
{
    protected static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    protected HttpClient Client { get; } = factory.CreateClient();

    public ValueTask InitializeAsync() => factory.ResetDatabaseAsync();

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    protected async Task<Asset> SeedAssetAsync(Action<Asset>? configure = null)
    {
        var id = Guid.CreateVersion7();
        var asset = Asset.Create($"AT-{id:N}", $"Sample Asset {id:N}");
        configure?.Invoke(asset);

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Assets.Add(asset);
        await dbContext.SaveChangesAsync();

        return asset;
    }
}
