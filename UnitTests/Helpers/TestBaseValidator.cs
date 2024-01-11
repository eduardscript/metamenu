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
        var validatorType = typeof(TValidator);
        
        var validatorConstructor = validatorType.GetConstructors().Single();

        var parameters = validatorConstructor.GetParameters();

        var arguments = new object[parameters.Length];
        
        var parameterTypes = new Dictionary<Type, object>
        {
            {typeof(ITenantRepository), TenantRepositoryMock},
            {typeof(ITagCategoryRepository), TagCategoryRepositoryMock},
            {typeof(ITagRepository), TagRepositoryMock},
            {typeof(IProductRepository), ProductRepositoryMock},
            {typeof(IUserRepository), UserRepositoryMock},
        };

        foreach (var parameter in parameters)
        {
            parameterTypes.TryGetValue(parameter.ParameterType, out var value);
            
            if (value is null)
            {
                throw new Exception($"Unknown parameter type: {parameter.ParameterType}");
            }
            
            arguments.SetValue(value, parameter.Position);
        }
        
        var validator = Activator.CreateInstance(validatorType, arguments);

        _validator = (TValidator)validator!;
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