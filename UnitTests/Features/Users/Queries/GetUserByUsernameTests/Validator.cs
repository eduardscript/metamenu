using Core.Features.Users.Queries;

namespace UnitTests.Features.Users.Queries.GetUserByUsernameTests;

[TestClass]
public class Validator : TestBaseValidator<GetUserByUsername.Validator, GetUserByUsername.Query>
{
    [TestMethod]
    public void Validate_EmptyUsername_FailsValidation()
    {
        // Arrange
        Command.Username = string.Empty;
            
        // Act & Assert
        ExpectedErrorMessage = "Username is required.";
    }
}