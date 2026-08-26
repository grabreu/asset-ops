using AssetOps.Application.Models.Assets;
using AssetOps.Application.Queries;

namespace AssetOps.Infrastructure.Persistence.Queries;

public sealed class AssetQueries(ApplicationDbContext dbContext) : IAssetQueries
{
    public async Task<List<AssetSummaryDto>> ListAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Assets
            .AsNoTracking()
            .OrderBy(asset => asset.Tag)
            .Select(asset => new AssetSummaryDto(
                asset.Id,
                asset.Tag,
                asset.Name,
                asset.Status,
                asset.CurrentHolder,
                asset.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
