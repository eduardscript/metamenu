using Core.Features.Tenants.Commands;
using Microsoft.Extensions.Time.Testing;

namespace IntegrationTests.Features.Tenants.Commands;

[TestClass]
public class CreateTenantTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_CreatesTenantInDatabase()
    {
        // Arrange
        var tenant = Fixture.Create<Tenant>();
        
        TimeProvider.SetUtcNow(tenant.CreatedAt);

        var handler = new CreateTenant.Handler(TenantRepository, TimeProvider);

        // Act
        var tenantDto = await handler.Handle(new CreateTenant.Command(tenant.Name), default);

        // Assert
        var tenantExists = await TenantRepository.ExistsAsync(tenantDto.Code, default);
        tenantExists.Should().BeTrue();
    }
}
