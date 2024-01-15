using Core.Features.Users.Commands;

namespace IntegrationTests.Features.Users.Commands;

[TestClass]
public class RegisterUserTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_ValidInput_CreatesUser()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var user = Fixture.Build<User>()
            .With(u => u.AvailableTenants, new List<int> { tenant.Code })
            .Create();

        var handler = new RegisterUser.Handler(TenantRepository, UserRepository);

        // Act
        var result = await handler.Handle(new RegisterUser.Command(user.Username, user.Password, user.AvailableTenants),
            default);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be(user.Username);
        result.AvailableTenants.Should().BeEquivalentTo(user.AvailableTenants);
    }
}