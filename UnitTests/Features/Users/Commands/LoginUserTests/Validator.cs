using Core.Features.Users.Commands;

namespace UnitTests.Features.Users.Commands.LoginUserTests;

[TestClass]
public class Validator : TestBaseValidator<LoginUser.Validator, LoginUser.Command>
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
}