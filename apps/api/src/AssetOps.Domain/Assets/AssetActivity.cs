namespace AssetOps.Domain.Assets;

public sealed class AssetActivity
{
    public Guid Id { get; private set; }
    public Guid AssetId { get; private set; }
    public AssetActivityType Type { get; private set; }
    public string? Holder { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }

    private AssetActivity()
    {
    }

    private AssetActivity(Guid id, Guid assetId, AssetActivityType type, string? holder)
    {
        Id = id;
        AssetId = assetId;
        Type = type;
        Holder = holder;
        OccurredAt = DateTimeOffset.UtcNow;
    }

    internal static AssetActivity Create(Guid assetId, AssetActivityType type, string? holder)
    {
        return new AssetActivity(Guid.CreateVersion7(), assetId, type, holder);
    }
}
