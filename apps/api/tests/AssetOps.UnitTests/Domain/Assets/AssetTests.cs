using AssetOps.Domain.Assets;
using AssetOps.Domain.Assets.Exceptions;

namespace AssetOps.UnitTests.Domain.Assets;

public class AssetTests
{
    private const string ValidTag = "AT-0001";
    private const string ValidName = "Sample Asset";
    private const string ValidHolder = "Jane Doe";

    [Fact]
    public void Create_WithValidData_SetsExpectedProperties()
    {
        // Act
        var asset = Asset.Create(ValidTag, ValidName);

        // Assert
        asset.Id.ShouldNotBe(Guid.Empty);
        asset.Tag.ShouldBe(ValidTag);
        asset.Name.ShouldBe(ValidName);
        asset.Status.ShouldBe(AssetStatus.Available);
        asset.CurrentHolder.ShouldBeNull();
        asset.RetiredAt.ShouldBeNull();
    }

    [Fact]
    public void Create_WithValidData_AddsCreatedActivity()
    {
        // Act
        var asset = Asset.Create(ValidTag, ValidName);

        // Assert
        var activity = asset.Activities.ShouldHaveSingleItem();
        activity.Type.ShouldBe(AssetActivityType.Created);
        activity.Holder.ShouldBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidTag_ThrowsArgumentException(string? tag)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Asset.Create(tag!, ValidName));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidName_ThrowsArgumentException(string? name)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Asset.Create(ValidTag, name!));
    }

    [Fact]
    public void Assign_WhenAvailable_SetsInUseAndHolder()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);

        // Act
        asset.Assign(ValidHolder);

        // Assert
        asset.Status.ShouldBe(AssetStatus.InUse);
        asset.CurrentHolder.ShouldBe(ValidHolder);
    }

    [Fact]
    public void Assign_WhenAvailable_AddsAssignedActivity()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);

        // Act
        asset.Assign(ValidHolder);

        // Assert
        asset.Activities.Count.ShouldBe(2);
        var activity = asset.Activities[1];
        activity.Type.ShouldBe(AssetActivityType.Assigned);
        activity.Holder.ShouldBe(ValidHolder);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Assign_WithInvalidHolder_ThrowsArgumentException(string? holder)
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);

        // Act & Assert
        Should.Throw<ArgumentException>(() => asset.Assign(holder!));
    }

    [Fact]
    public void Assign_WhenNotAvailable_ThrowsInvalidAssetStatusException()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.Assign(ValidHolder);

        // Act & Assert
        Should.Throw<InvalidAssetStatusException>(() => asset.Assign(ValidHolder));
    }

    [Fact]
    public void Return_WhenInUse_SetsAvailableAndClearsHolder()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.Assign(ValidHolder);

        // Act
        asset.Return();

        // Assert
        asset.Status.ShouldBe(AssetStatus.Available);
        asset.CurrentHolder.ShouldBeNull();
    }

    [Fact]
    public void Return_WhenInUse_AddsReturnedActivityWithPreviousHolder()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.Assign(ValidHolder);

        // Act
        asset.Return();

        // Assert
        asset.Activities.Count.ShouldBe(3);
        var activity = asset.Activities[2];
        activity.Type.ShouldBe(AssetActivityType.Returned);
        activity.Holder.ShouldBe(ValidHolder);
    }

    [Fact]
    public void Return_WhenNotInUse_ThrowsInvalidAssetStatusException()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);

        // Act & Assert
        Should.Throw<InvalidAssetStatusException>(() => asset.Return());
    }

    [Fact]
    public void SendToMaintenance_WhenAvailable_SetsMaintenance()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);

        // Act
        asset.SendToMaintenance();

        // Assert
        asset.Status.ShouldBe(AssetStatus.Maintenance);
        asset.CurrentHolder.ShouldBeNull();
    }

    [Fact]
    public void SendToMaintenance_WhenInUse_SetsMaintenanceAndClearsHolder()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.Assign(ValidHolder);

        // Act
        asset.SendToMaintenance();

        // Assert
        asset.Status.ShouldBe(AssetStatus.Maintenance);
        asset.CurrentHolder.ShouldBeNull();

        var activity = asset.Activities[2];
        activity.Type.ShouldBe(AssetActivityType.SentToMaintenance);
        activity.Holder.ShouldBe(ValidHolder);
    }

    [Fact]
    public void SendToMaintenance_WhenAlreadyInMaintenance_ThrowsInvalidAssetStatusException()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.SendToMaintenance();

        // Act & Assert
        Should.Throw<InvalidAssetStatusException>(() => asset.SendToMaintenance());
    }

    [Fact]
    public void SendToMaintenance_WhenRetired_ThrowsInvalidAssetStatusException()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.Retire();

        // Act & Assert
        Should.Throw<InvalidAssetStatusException>(() => asset.SendToMaintenance());
    }

    [Fact]
    public void ReturnFromMaintenance_WhenInMaintenance_SetsAvailable()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.SendToMaintenance();

        // Act
        asset.ReturnFromMaintenance();

        // Assert
        asset.Status.ShouldBe(AssetStatus.Available);
    }

    [Fact]
    public void ReturnFromMaintenance_WhenNotInMaintenance_ThrowsInvalidAssetStatusException()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);

        // Act & Assert
        Should.Throw<InvalidAssetStatusException>(() => asset.ReturnFromMaintenance());
    }

    [Fact]
    public void Retire_WhenAvailable_SetsRetiredAndRetiredAt()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);

        // Act
        asset.Retire();

        // Assert
        asset.Status.ShouldBe(AssetStatus.Retired);
        asset.RetiredAt.ShouldNotBeNull();
    }

    [Fact]
    public void Retire_WhenInMaintenance_SetsRetired()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.SendToMaintenance();

        // Act
        asset.Retire();

        // Assert
        asset.Status.ShouldBe(AssetStatus.Retired);
    }

    [Fact]
    public void Retire_WhenInUse_ThrowsInvalidAssetStatusException()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.Assign(ValidHolder);

        // Act & Assert
        Should.Throw<InvalidAssetStatusException>(() => asset.Retire());
    }

    [Fact]
    public void Retire_WhenAlreadyRetired_ThrowsInvalidAssetStatusException()
    {
        // Arrange
        var asset = Asset.Create(ValidTag, ValidName);
        asset.Retire();

        // Act & Assert
        Should.Throw<InvalidAssetStatusException>(() => asset.Retire());
    }
}
