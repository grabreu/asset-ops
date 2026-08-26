using AssetOps.Domain.Assets.Exceptions;

namespace AssetOps.Domain.Assets;

public sealed class Asset
{
    private readonly List<AssetActivity> _activities = [];

    public Guid Id { get; private set; }
    public string Tag { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public AssetStatus Status { get; private set; }
    public string? CurrentHolder { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? RetiredAt { get; private set; }

    public IReadOnlyList<AssetActivity> Activities => _activities;

    private Asset()
    {
    }

    private Asset(Guid id, string tag, string name)
    {
        Id = id;
        Tag = tag;
        Name = name;
        Status = AssetStatus.Available;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Asset Create(string tag, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var asset = new Asset(Guid.CreateVersion7(), tag, name);
        asset.AddActivity(AssetActivityType.Created, null);

        return asset;
    }

    public void Assign(string holder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(holder);

        if (Status != AssetStatus.Available)
        {
            throw new InvalidAssetStatusException(Status);
        }

        Status = AssetStatus.InUse;
        CurrentHolder = holder;

        AddActivity(AssetActivityType.Assigned, holder);
    }

    public void Return()
    {
        if (Status != AssetStatus.InUse)
        {
            throw new InvalidAssetStatusException(Status);
        }

        var holder = CurrentHolder;

        Status = AssetStatus.Available;
        CurrentHolder = null;

        AddActivity(AssetActivityType.Returned, holder);
    }

    public void SendToMaintenance()
    {
        if (Status is not (AssetStatus.Available or AssetStatus.InUse))
        {
            throw new InvalidAssetStatusException(Status);
        }

        var holder = CurrentHolder;

        Status = AssetStatus.Maintenance;
        CurrentHolder = null;

        AddActivity(AssetActivityType.SentToMaintenance, holder);
    }

    public void ReturnFromMaintenance()
    {
        if (Status != AssetStatus.Maintenance)
        {
            throw new InvalidAssetStatusException(Status);
        }

        Status = AssetStatus.Available;

        AddActivity(AssetActivityType.ReturnedFromMaintenance, null);
    }

    public void Retire()
    {
        if (Status is not (AssetStatus.Available or AssetStatus.Maintenance))
        {
            throw new InvalidAssetStatusException(Status);
        }

        Status = AssetStatus.Retired;
        RetiredAt = DateTimeOffset.UtcNow;

        AddActivity(AssetActivityType.Retired, null);
    }

    private void AddActivity(AssetActivityType type, string? holder)
    {
        _activities.Add(AssetActivity.Create(Id, type, holder));
    }
}
