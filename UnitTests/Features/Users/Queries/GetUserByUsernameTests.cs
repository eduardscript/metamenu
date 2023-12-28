using Core.Features.Users.Queries;

namespace UnitTests.Features.Users.Queries;

public static class GetUserByUsernameTests
{
    [TestClass]
    public class ValidatorTests : TestBase
    {
        private readonly GetUserByUsername.Validator _validator = new();

        [TestMethod]
        public void Validate_EmptyUsername_FailsValidation()
        {
            // Arrange
            var query = new GetUserByUsername.Query(string.Empty);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Single().ErrorMessage.Should().Be("Username is required.");
        }

        [TestMethod]
        public void Validate_ValidUsername_PassesValidation()
        {
            // Arrange
            var query = Fixture.Create<GetUserByUsername.Query>();

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}