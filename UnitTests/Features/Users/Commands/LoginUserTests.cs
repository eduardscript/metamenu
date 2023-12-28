using Core.Exceptions.Products;
using Core.Features.Users.Commands;

namespace UnitTests.Features.Users.Commands;

public static class LoginUserTests
{
    [TestClass]
    public class ValidatorTests : TestBase
    {
        private readonly LoginUser.Validator _validator = new();

        [TestMethod]
        public void Validate_EmptyUsername_FailsValidation()
        {
            // Arrange
            var command = Fixture.Build<LoginUser.Command>()
                .With(c => c.Username, string.Empty)
                .Create();

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Single().ErrorMessage.Should().Be("Username is required.");
        }

        [TestMethod]
        public void Validate_EmptyPassword_FailsValidation()
        {
            // Arrange
            var command = Fixture.Build<LoginUser.Command>()
                .With(c => c.Password, string.Empty)
                .Create();

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Single().ErrorMessage.Should().Be("Password is required.");
        }

        [TestMethod]
        public void Validate_ValidCredentials_PassesValidation()
        {
            // Arrange
            var command = Fixture.Create<LoginUser.Command>();

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }

    [TestClass]
    public class HandlerTests : TestBase<LoginUser.Handler>
    {
        public HandlerTests()
        {
            Handler = new LoginUser.Handler(TokenServiceMock, UserRepositoryMock);
        }

        [TestMethod]
        public async Task Handle_UserDoesNotExist_ThrowsException()
        {
            // Arrange
            var command = Fixture.Create<LoginUser.Command>();
            
            // Act & Assert
            await AssertThrowsAsync<UserNotFoundException>(command);

        }

        [TestMethod]
        public async Task Handle_IncorrectPassword_ThrowsException()
        {
            // Arrange
            var command = Fixture.Create<LoginUser.Command>();
            var user = Fixture.Build<User>()
                .With(u => u.Username, command.Username)
                .Create();

            UserRepositoryMock.GetByAsync(command.Username, Arg.Any<CancellationToken>()).Returns(user);

            // Act & Assert
            await AssertThrowsAsync<InvalidPasswordException>(command);
        }
    }
}
