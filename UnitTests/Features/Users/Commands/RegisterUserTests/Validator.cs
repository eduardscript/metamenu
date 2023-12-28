using Core.Features.Users.Commands;

namespace UnitTests.Features.Users.Commands.RegisterUserTests;

[TestClass]
public class Validator : TestBaseValidator<RegisterUser.Validator, RegisterUser.Command>
{
    [TestMethod]
    public void Validate_EmptyUsername_FailsValidation()
    {
        // Arrange
        Command.Username = string.Empty;
        ExpectedErrorMessage = "Username is required.";
    }

    [TestMethod]
    public void Validate_EmptyPassword_FailsValidation()
    {
        // Arrange
        Command.Password = string.Empty;
        ExpectedErrorMessage = "Password is required.";
    }
        
    [TestMethod]
    public void Validate_EmptyAvailableTenants_FailsValidation()
    {
        // Arrange
        Command.AvailableTenants = new List<int>();
        ExpectedErrorMessage = "Available Tenants must not be empty.";
    }
        
    [TestMethod]
    public void Validate_DuplicateAvailableTenants_FailsValidation()
    {
        // Arrange
        Command.AvailableTenants = new List<int> { 1, 1 };
        ExpectedErrorMessage = "AvailableTenants must be unique. Duplicated ones: 1.";
    }
}
