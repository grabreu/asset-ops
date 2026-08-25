namespace AssetOps.Infrastructure.Persistence;

public sealed class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{
    public async Task InitializeAsync()
    {
        await dbContext.Database.MigrateAsync();
    }
}

public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitializeAsync();
    }
}
