using Core.Features.Tenants;

namespace UnitTests.Features.Tenants;

[Trait(nameof(Constants.Features), Constants.Features.Tenants)]
public class CreateTenantTests : TestBase
{
    private readonly Tenant _tenant = Fixture.Create<Tenant>();

    [Fact]
    public async Task Handle_WhenTenantExists_ThrowsTenantAlreadyExistsException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tenant.TenantCode, default).Returns(true);

        var handler = new CreateTenant.Handler(tenantRepository);

        // Act and Assert
        await Assert.ThrowsAsync<TenantAlreadyExistsException>(() =>
            handler.Handle(new CreateTenant.Command(_tenant.TenantCode, _tenant.Name), default));

        await tenantRepository.DidNotReceive().CreateAsync(_tenant, default);
    }
}