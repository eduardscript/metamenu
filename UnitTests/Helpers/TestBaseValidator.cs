using FluentValidation;

namespace UnitTests.Helpers;

[TestClass]
public class TestBaseValidator<TValidator, TCommand> : TestBase
    where TValidator : AbstractValidator<TCommand>, new()
{
    private TValidator _validator = default!;

    protected readonly TCommand Command = Fixture.Create<TCommand>();

    protected string ExpectedErrorMessage = default!;

    [TestMethod]
    public void Validate_ValidInput_PassesValidation()
    {
        // Act
        var result = _validator.Validate(Command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        var validatorType = typeof(TValidator);
        var validator = Activator.CreateInstance(validatorType);

        _validator = (TValidator)validator!;
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (ExpectedErrorMessage != default!)
        {
            // Act & Assert
            AssertValidationResult(ExpectedErrorMessage);
        }
    }

    private void AssertValidationResult(string expectedErrorMessage)
    {
        var result = _validator.Validate(Command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().ErrorMessage.Should().Be(expectedErrorMessage);
    }
}