using AssetOps.Domain.Assets;

namespace AssetOps.Application.Models.Assets;

public sealed record AssetSummaryDto(
    Guid Id,
    string Tag,
    string Name,
    AssetStatus Status,
    string? CurrentHolder,
    DateTimeOffset CreatedAt);
