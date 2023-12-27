using Core.Exceptions.Products;
using Core.Features.Users.Commands;

namespace UnitTests.Features.Users.Commands;

[TestClass]
public class LoginUserTests : TestBase<LoginUser.Handler>
{
    public LoginUserTests()
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
