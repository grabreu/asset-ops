using AssetOps.Application.Queries;
using AssetOps.Domain.SeedWork;
using AssetOps.Infrastructure.Persistence;
using AssetOps.Infrastructure.Persistence.Queries;

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

        return services;
    }
}
