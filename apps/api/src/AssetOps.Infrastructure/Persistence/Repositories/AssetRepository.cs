using AssetOps.Domain.Assets;

namespace AssetOps.Infrastructure.Persistence.Repositories;

public sealed class AssetRepository(ApplicationDbContext dbContext) : IAssetRepository
{
    public async Task AddAsync(Asset asset, CancellationToken cancellationToken)
    {
        await dbContext.Assets.AddAsync(asset, cancellationToken);
    }

    public Task<bool> TagExistsAsync(string tag, CancellationToken cancellationToken)
    {
        return dbContext.Assets.AnyAsync(a => a.Tag == tag, cancellationToken);
    }
}
