using AssetOps.Application.Models.Assets;

namespace AssetOps.Application.Commands.Assets.Create;

public sealed record CreateAssetCommand(
    string Tag,
    string Name) : ICommand<ErrorOr<AssetSummaryDto>>;
