using AssetOps.Application.Commands.Assets.Create;

namespace AssetOps.UnitTests.Application.Commands.Assets.Create;

public class CreateAssetCommandValidatorTests
{
    private readonly CreateAssetCommandValidator _validator = new();

    private static CreateAssetCommand ValidCommand => new("AT-0001", "Sample Asset");

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        // Act
        var result = _validator.TestValidate(ValidCommand);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithMissingTag_HasErrorForTag(string? tag)
    {
        // Arrange
        var command = ValidCommand with { Tag = tag! };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Tag);
    }

    [Fact]
    public void Validate_WithTagLongerThanMaxLength_HasErrorForTag()
    {
        // Arrange
        var command = ValidCommand with { Tag = new string('A', 51) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Tag);
    }

    [Fact]
    public void Validate_WithTagAtMaxLength_HasNoErrorForTag()
    {
        // Arrange
        var command = ValidCommand with { Tag = new string('A', 50) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Tag);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithMissingName_HasErrorForName(string? name)
    {
        // Arrange
        var command = ValidCommand with { Name = name! };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameLongerThanMaxLength_HasErrorForName()
    {
        // Arrange
        var command = ValidCommand with { Name = new string('A', 201) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameAtMaxLength_HasNoErrorForName()
    {
        // Arrange
        var command = ValidCommand with { Name = new string('A', 200) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
