namespace AssetOps.Domain.Assets;

public interface IAssetRepository
{
    Task AddAsync(Asset asset, CancellationToken cancellationToken);
    Task<bool> TagExistsAsync(string tag, CancellationToken cancellationToken);
}
