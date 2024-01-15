using FluentValidation;

namespace UnitTests.Helpers;

[TestClass]
public class TestBaseValidator<TValidator, TCommand> : TestBase
    where TValidator : AbstractValidator<TCommand>
{
    private TValidator _validator = default!;

    protected readonly TCommand Command = Fixture.Create<TCommand>();

    protected string ExpectedErrorMessage = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _validator = Build<TValidator>();
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        if (ExpectedErrorMessage != default!)
        {
            // Act & Assert
            await AssertValidationResult(ExpectedErrorMessage);
        }
    }

    private async Task AssertValidationResult(string expectedErrorMessage)
    {
        var result = await _validator.ValidateAsync(Command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().ErrorMessage.Should().Be(expectedErrorMessage);

        var error = result.Errors.Single();
        error.ErrorMessage.Should().Be(expectedErrorMessage);
    }
}