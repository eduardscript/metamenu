using System.Reflection.Metadata;
using Core.Exceptions.Tenants;
using Core.Features.Users.Commands;

namespace UnitTests.Features.Users.Commands;

[TestClass]
public class CreateUserTests : TestBase<CreateUser.Handler>
{
    public CreateUserTests()
    {
        Handler = new CreateUser.Handler(TenantRepositoryMock, UserRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_InvalidTenantCodes_ThrowsException()
    {
        // Arrange
        var command = Fixture.Create<CreateUser.Command>();
        var tenants = Fixture.CreateMany<Tenant>(3).ToList();
        TenantRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(tenants);

        // Act & Assert
        await AssertThrowsAsync<TenantsNotFoundException>(command);
    }
}