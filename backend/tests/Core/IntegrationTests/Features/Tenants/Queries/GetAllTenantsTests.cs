using Core.Features.Tenants.Queries;
using Core.Features.Tenants.Shared;

namespace IntegrationTests.Features.Tenants.Queries;

[TestClass]
public class GetAllTenantsTests : IntegrationTestBase
{
    [TestInitialize]
    public void TestInitialize()
    {
        MongoDbFixture.Reset();
    }

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
        {
            resultList
                .Should()
                .ContainEquivalentOf(new TenantDto(expectedTenant.Code, expectedTenant.Name, expectedTenant.IsEnabled, expectedTenant.CreatedAt));
        }
    }
}