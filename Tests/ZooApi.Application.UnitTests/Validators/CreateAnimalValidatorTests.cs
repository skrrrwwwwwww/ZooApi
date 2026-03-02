namespace ZooApi.Application.UnitTests.Validators;

public class CreateAnimalValidatorTests
{
    private readonly CreateAnimalDtoValidator _validator = new();

    [Fact] 
    public void Should_Have_Errors_When_Fields_Are_Empty()
    {
        // Arrange
        var model = new CreateAnimalDto ("", "");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Имя не может быть пустым"); 
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Model_Is_Valid()
    {
        // Arrange
        var model = new CreateAnimalDto("Simba", "Lion");
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
