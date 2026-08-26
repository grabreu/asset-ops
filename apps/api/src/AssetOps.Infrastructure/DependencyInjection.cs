using AssetOps.Application.Queries;
using AssetOps.Domain.Assets;
using AssetOps.Domain.SeedWork;
using AssetOps.Infrastructure.Persistence;
using AssetOps.Infrastructure.Persistence.Queries;
using AssetOps.Infrastructure.Persistence.Repositories;

namespace AssetOps.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("AssetOpsDb")));

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(name: "database");

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddScoped<IAssetQueries, AssetQueries>();
        services.AddScoped<IAssetRepository, AssetRepository>();

        return services;
    }
}
