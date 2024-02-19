using Core.Features.Tenants.Commands;
using Core.Features.Tenants.Commands.ToggleTenantStatus;

namespace IntegrationTests.Features.Tenants.Commands;

[TestClass]
public class ToggleTenantStatusTests : BaseIntegrationTest
{
    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Handle_ToggleTenantStatusInDatabase(bool isEnabled)
    {
        // Arrange
        var tenant = Fixture
            .Build<Tenant>()
            .With(t => t.IsEnabled, isEnabled)
            .Create();

        var insertedTenant = await MongoDbFixture.CreateTenantAsync(tenant);

        var handler = new ToggleTenantStatus.Handler(TenantRepository);

        // Act
        var tenantStatusDto = await handler.Handle(
            new ToggleTenantStatus.Command(insertedTenant.Code, !insertedTenant.IsEnabled),
            default);

        // Assert
        tenantStatusDto.statusUpdated.Should().BeTrue();
        
        var existingTenant = await TenantRepository.GetByCodeAsync(insertedTenant.Code, default);
        existingTenant.Should().NotBeNull();
        existingTenant!.IsEnabled.Should().Be(!isEnabled);
    }
}