using Core.Features.Tenants;
using MongoDB.Driver;

namespace IntegrationTests.Features.Tenants;

[Trait(nameof(Constants.Features), Constants.Features.Tenants)]
public class CreateTenantTests : IntegrationTestBase
{
    private readonly ITenantRepository _tenantRepository;

    public CreateTenantTests()
    {
        _tenantRepository = GetService<ITenantRepository>();
    }

    [Fact]
    public async Task Handle_CreatesTenantInDatabase()
    {
        // Arrange
        var tenant = Fixture.Create<Tenant>();

        var handler = new CreateTenant.Handler(_tenantRepository);

        // Act
        await handler.Handle(new CreateTenant.Command(tenant.TenantCode, tenant.Name), default);

        // Assert
        var tenantExists = await _tenantRepository.ExistsByCodeAsync(tenant.TenantCode, default);
        Assert.True(tenantExists);
    }
}