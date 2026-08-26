using AssetOps.Domain.Assets;

namespace AssetOps.Infrastructure.Persistence;

public sealed class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{
    public async Task InitializeAsync()
    {
        await dbContext.Database.MigrateAsync();
    }

    public async Task SeedAsync()
    {
        if (await dbContext.Assets.AnyAsync())
        {
            return;
        }

        var assets = Enumerable.Range(1, 12)
            .Select(i => Asset.Create($"AT-{i:0000}", $"Sample Asset {i}"))
            .ToArray();

        dbContext.Assets.AddRange(assets);

        await dbContext.SaveChangesAsync();
    }
}

public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}
