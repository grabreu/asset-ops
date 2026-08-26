using AssetOps.Application.Models.Assets;

namespace AssetOps.Application.Queries.Assets.List;

public sealed record ListAssetsQuery : IQuery<ErrorOr<List<AssetSummaryDto>>>;
