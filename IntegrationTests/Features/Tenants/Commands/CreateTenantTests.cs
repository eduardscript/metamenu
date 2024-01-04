using Core.Features.Tenants.Commands;

namespace IntegrationTests.Features.Tenants.Commands;

[TestClass]
public class CreateTenantTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_CreatesTenantInDatabase()
    {
        // Arrange
        var tenant = Fixture.Create<Tenant>();

        var handler = new CreateTenant.Handler(TenantRepository);

        // Act
        var tenantDto = await handler.Handle(new CreateTenant.Command(tenant.TenantCode, tenant.Name), default);

        // Assert
        var tenantExists = await TenantRepository.ExistsAsync(tenantDto.TenantCode, default);
        tenantExists.Should().BeTrue();
    }
}