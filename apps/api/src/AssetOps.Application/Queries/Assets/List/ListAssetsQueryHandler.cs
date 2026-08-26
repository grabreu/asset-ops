using AssetOps.Application.Models.Assets;

namespace AssetOps.Application.Queries.Assets.List;

public sealed class ListAssetsQueryHandler(IAssetQueries queries) : IQueryHandler<ListAssetsQuery, ErrorOr<List<AssetSummaryDto>>>
{
    public async ValueTask<ErrorOr<List<AssetSummaryDto>>> Handle(ListAssetsQuery query, CancellationToken cancellationToken)
    {
        return await queries.ListAsync(cancellationToken);
    }
}
