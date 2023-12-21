using Core.Features.Tenants.Queries;

namespace IntegrationTests.Features.Tenants.Queries;

[TestClass]
public class GetAllTenantsTests : IntegrationTestBase
{
    private readonly ITenantRepository _tenantRepository;

    public GetAllTenantsTests()
    {
        _tenantRepository = GetService<ITenantRepository>();
    }

    [TestMethod]
    public async Task Handle_ReturnsAllTenants()
    {
        // Arrange
        var expectedTenants = Fixture.CreateMany<Tenant>().ToList();

        foreach (var tenant in expectedTenants) await _tenantRepository.CreateAsync(tenant, default);

        var handler = new GetAllTenants.Handler(_tenantRepository);

        // Act
        var result = await handler.Handle(new GetAllTenants.Query(), default);

        // Assert
        var resultList = result.ToList();

        resultList.Should().HaveCount(expectedTenants.Count);
        foreach (var expectedTenant in expectedTenants)
            resultList.Should()
                .ContainEquivalentOf(new GetAllTenants.TenantDto(expectedTenant.TenantCode, expectedTenant.Name));
    }
}