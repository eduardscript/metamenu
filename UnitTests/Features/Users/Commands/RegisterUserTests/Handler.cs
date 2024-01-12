using Core.Exceptions.Tenants;
using Core.Features.Users.Commands;

namespace UnitTests.Features.Users.Commands.RegisterUserTests;

[TestClass]
public class Handler : TestBaseHandler<RegisterUser.Handler, RegisterUser.Command>
{
    [TestMethod]
    public async Task Handle_InvalidTenantCodes_ThrowsException()
    {
        // Arrange
        var tenants = Fixture.CreateMany<Tenant>(3).ToList();
        TenantRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(tenants);

        // Act & Assert
        await AssertThrowsAsync<TenantsNotFoundException>(Request);
        
        await TenantRepositoryMock.Received().GetAllAsync(Arg.Any<CancellationToken>());
        await UserRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }
}