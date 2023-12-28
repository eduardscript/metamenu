using Core.Exceptions.Tenants;
using Core.Features.Users.Commands;

namespace UnitTests.Features.Users.Commands;

public static class RegisterUserTests
{
    [TestClass]
    public class ValidatorTests : TestBase
    {
        private readonly RegisterUser.Validator _validator = new();

        [TestMethod]
        public void Validate_EmptyUsername_FailsValidation()
        {
            // Arrange
            var command = new RegisterUser.Command(string.Empty, "password", new List<int> { 1 });

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
            var command = new RegisterUser.Command("username", string.Empty, new List<int> { 1 });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Single().ErrorMessage.Should().Be("Password is required.");
        }

        [TestMethod]
        public void Validate_EmptyAvailableTenants_FailsValidation()
        {
            // Arrange
            var command = new RegisterUser.Command("username", "password", new List<int>());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Single().ErrorMessage.Should().Be("Available Tenants must not be empty.");
        }

        [TestMethod]
        public void Validate_DuplicateAvailableTenants_FailsValidation()
        {
            // Arrange
            var command = new RegisterUser.Command("username", "password", new List<int> { 1, 1 });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Single().ErrorMessage.Should().Be("AvailableTenants must be unique. Duplicated ones: 1.");
        }

        [TestMethod]
        public void Validate_ValidInput_PassesValidation()
        {
            // Arrange
            var command = new RegisterUser.Command("username", "password", new List<int> { 1 });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }

    [TestClass]
    public class HandlerTests : TestBase<RegisterUser.Handler>
    {
        public HandlerTests()
        {
            Handler = new RegisterUser.Handler(TenantRepositoryMock, UserRepositoryMock);
        }

        [TestMethod]
        public async Task Handle_InvalidTenantCodes_ThrowsException()
        {
            // Arrange
            var command = Fixture.Create<RegisterUser.Command>();
            var tenants = Fixture.CreateMany<Tenant>(3).ToList();
            TenantRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(tenants);

            // Act & Assert
            await AssertThrowsAsync<TenantsNotFoundException>(command);
        }
    }
}