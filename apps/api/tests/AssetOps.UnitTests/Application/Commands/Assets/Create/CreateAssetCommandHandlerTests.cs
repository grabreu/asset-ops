using AssetOps.Application.Commands.Assets.Create;
using AssetOps.Domain.Assets;
using AssetOps.Domain.SeedWork;

namespace AssetOps.UnitTests.Application.Commands.Assets.Create;

public class CreateAssetCommandHandlerTests
{
    private readonly IAssetRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateAssetCommandHandler _handler;

    private static CreateAssetCommand ValidCommand => new("AT-0001", "Sample Asset");

    public CreateAssetCommandHandlerTests()
    {
        _repository = Substitute.For<IAssetRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateAssetCommandHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidCommand_AddsAssetAndSavesChanges()
    {
        // Arrange
        _repository.TagExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(ValidCommand, TestContext.Current.CancellationToken);

        // Assert
        result.IsError.ShouldBeFalse();
        await _repository.Received(1).AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsResultMatchingCreatedAsset()
    {
        // Arrange
        _repository.TagExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        Asset? addedAsset = null;
        _repository.AddAsync(Arg.Do<Asset>(a => addedAsset = a), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(ValidCommand, CancellationToken.None);

        // Assert
        addedAsset.ShouldNotBeNull();
        result.Value.Id.ShouldBe(addedAsset.Id);
        result.Value.Tag.ShouldBe(addedAsset.Tag);
        result.Value.Name.ShouldBe(addedAsset.Name);
        result.Value.Status.ShouldBe(addedAsset.Status);
        result.Value.CurrentHolder.ShouldBe(addedAsset.CurrentHolder);
        result.Value.CreatedAt.ShouldBe(addedAsset.CreatedAt);
    }

    [Fact]
    public async Task Handle_WithDuplicateTag_ReturnsConflictWithoutAddingAsset()
    {
        // Arrange
        _repository.TagExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(ValidCommand, CancellationToken.None);

        // Assert
        result.IsError.ShouldBeTrue();
        result.FirstError.Type.ShouldBe(ErrorType.Conflict);
        await _repository.DidNotReceive().AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
