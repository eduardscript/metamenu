using Core.Features.Tenants.Queries;

namespace IntegrationTests.Features.Tenants.Queries;

[TestClass]
public class GetAllTenantsTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_ReturnsAllTenants()
    {
        // Arrange
        await MongoDbFixture.CreateTenantsAsync();

        var handler = new GetAllTenants.Handler(TenantRepository);

        // Act
        var result = await handler.Handle(new GetAllTenants.Query(), default);

        // Assert
        var resultList = result.ToList();

        resultList.Should().HaveCount(MongoDbFixture.CreatedTenants.Count);
        foreach (var expectedTenant in MongoDbFixture.CreatedTenants)
            resultList.Should()
                .ContainEquivalentOf(new GetAllTenants.TenantDto(expectedTenant.TenantCode, expectedTenant.Name));
    }
}