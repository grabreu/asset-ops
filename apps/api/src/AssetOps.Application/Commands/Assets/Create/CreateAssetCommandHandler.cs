using AssetOps.Application.Models.Assets;
using AssetOps.Domain.Assets;
using AssetOps.Domain.SeedWork;

namespace AssetOps.Application.Commands.Assets.Create;

public sealed class CreateAssetCommandHandler(
    IAssetRepository repository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateAssetCommand, ErrorOr<AssetSummaryDto>>
{
    public async ValueTask<ErrorOr<AssetSummaryDto>> Handle(CreateAssetCommand command, CancellationToken cancellationToken)
    {
        if (await repository.TagExistsAsync(command.Tag, cancellationToken))
        {
            return Error.Conflict(description: $"An asset with tag '{command.Tag}' already exists.");
        }

        var asset = Asset.Create(command.Tag, command.Name);

        await repository.AddAsync(asset, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AssetSummaryDto(
            asset.Id,
            asset.Tag,
            asset.Name,
            asset.Status,
            asset.CurrentHolder,
            asset.CreatedAt);
    }
}
