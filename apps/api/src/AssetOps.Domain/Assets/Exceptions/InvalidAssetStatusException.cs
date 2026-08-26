using AssetOps.Domain.SeedWork;

namespace AssetOps.Domain.Assets.Exceptions;

public sealed class InvalidAssetStatusException(AssetStatus status)
    : DomainException($"Asset status is '{status}', which is not valid for this operation.");
