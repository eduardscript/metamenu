using Core.Features.Tenants.Queries;

namespace IntegrationTests.Features.Tenants.Queries;

[TestClass]
public class GetAllTenantsTests : BaseIntegrationTest
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
            //TODO : Fix test

            resultList
                .Should()
                .ContainEquivalentOf(expectedTenant, options => options
                    .Excluding(t => t.IsEnabled)
                    .Excluding(t => t.Weekdays)
                    .Excluding(t => t.Address));
        }
    }
}