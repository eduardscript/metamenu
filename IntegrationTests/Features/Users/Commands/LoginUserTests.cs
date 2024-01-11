using Core.Features.Users.Commands;
using Core.Services;

namespace IntegrationTests.Features.Users.Commands;

[TestClass]
public class LoginUserTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_ValidCredentials_LogsInUser()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var user = Fixture.Build<User>()
            .With(u => u.AvailableTenants, new List<int> { tenant.Code })
            .Create();

        await UserRepository.CreateAsync(user, default);

        var handler = new LoginUser.Handler(GetService<ITokenService>(), UserRepository);

        // Act
        var result = await handler.Handle(new LoginUser.Command(user.Username, user.Password), default);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
    }
}