using Core.Features.Tenants.Commands;

namespace IntegrationTests.Features.Tenants.Commands;

[TestClass]
public class DeleteTenantTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_DeleteTenantInDatabase()
    {
        // Arrange
        var insertedTenant = await MongoDbFixture.CreateTenantAsync();

        var handler = new DeleteTenant.Handler(TenantRepository);

        // Act
        var tenantDeletedDto = await handler.Handle(new DeleteTenant.Command(insertedTenant.Code), default);

        // Assert
        tenantDeletedDto.IsDeleted.Should().BeTrue();

        var tenantExists = await TenantRepository.ExistsAsync(insertedTenant.Code, default);
        tenantExists.Should().BeFalse();
    }
}