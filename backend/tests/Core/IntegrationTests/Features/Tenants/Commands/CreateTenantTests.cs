using Core.Features.Tenants.Commands;

namespace IntegrationTests.Features.Tenants.Commands;

[TestClass]
public class CreateTenantTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task Handle_CreatesTenantInDatabase()
    {
        // Arrange
        var handler = new CreateTenant.Handler(TenantRepository, TimeProvider);

        // Act
        var tenantDto = await handler.Handle(new CreateTenant.Command(Fixture.Create<Tenant>().Name), default);

        // Assert
        tenantDto.Code.Should().Be(1000);
        tenantDto.IsEnabled.Should().BeFalse();
        tenantDto.CreatedAt.Should().Be(TimeProvider.GetUtcNow().DateTime);
    }
}
