using Core.Features.Users.Queries;

namespace IntegrationTests.Features.Users.Queries;

[TestClass]
public class GetUserByUsernameTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_ValidUsername_ReturnsUser()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var user = Fixture.Build<User>()
            .With(u => u.AvailableTenants, new List<int> { tenant.Code })
            .Create();

        await UserRepository.CreateAsync(user, default);

        var handler = new GetUserByUsername.Handler(UserRepository);

        // Act
        var result = await handler.Handle(new GetUserByUsername.Query(user.Username), default);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be(user.Username);
        result.AvailableTenants.Should().BeEquivalentTo(user.AvailableTenants);
    }
}