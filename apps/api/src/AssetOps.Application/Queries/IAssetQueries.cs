using AssetOps.Application.Models.Assets;

namespace AssetOps.Application.Queries;

public interface IAssetQueries
{
    Task<List<AssetSummaryDto>> ListAsync(CancellationToken cancellationToken);
}
