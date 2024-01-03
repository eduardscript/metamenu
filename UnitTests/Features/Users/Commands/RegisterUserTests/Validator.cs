using Core.Features.Users.Commands;
using Humanizer;

namespace UnitTests.Features.Users.Commands.RegisterUserTests;

[TestClass]
public class Validator : TestBaseValidator<RegisterUser.Validator, RegisterUser.Command>
{
    [TestMethod]
    public void Validate_EmptyUsername_FailsValidation()
    {
        // Arrange
        Command.Username = string.Empty;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.Username));
    }

    [TestMethod]
    public void Validate_EmptyPassword_FailsValidation()
    {
        // Arrange
        Command.Password = string.Empty;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.Password));
    }
        
    [TestMethod]
    public void Validate_EmptyAvailableTenants_FailsValidation()
    {
        // Arrange
        Command.AvailableTenants = new List<int>();
        ExpectedErrorMessage = "'Available Tenants' must not be empty.";
    }
    
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Validate_EmptyAvailableTenant_FailsValidation(int availableTenant)
    {
        // Arrange
        Command.AvailableTenants = new List<int> { availableTenant };
        ExpectedErrorMessage = CustomValidatorsMessages.GreaterThanZeroMessage(nameof(Command.AvailableTenants).Singularize().Humanize(LetterCasing.Title));
    }
 
    [TestMethod]
    public void Validate_DuplicateAvailableTenants_FailsValidation()
    {
        // Arrange
        Command.AvailableTenants = new List<int> { 1, 1, 2, 2 };
        ExpectedErrorMessage = "'Available Tenants' must be unique. Duplicated items found: '1, 2'.";
    }
}
