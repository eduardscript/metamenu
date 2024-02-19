using Core.Repositories;

namespace IntegrationTests.Repositories;

[TestClass]
public class TenantRepositoryTests : BaseIntegrationTest
{
    public TenantRepositoryTests()
    {
        Fixture.Customize(new TenantCustomization());
    }

    #region CreateAsync

    [TestMethod]
    public async Task CreateAsync_Creates()
    {
        // Arrange
        var tenantToInsert = Fixture.Create<Tenant>();

        // Act
        var tenantFromDb = await TenantRepository.CreateAsync(tenantToInsert, default);

        // Assert
        tenantFromDb.Should().BeEquivalentTo(tenantToInsert);
    }

    #endregion

    #region GetAllAsync

    [TestMethod]
    public async Task GetAllAsync_Gets()
    {
        // Arrange
        var tenantsToInsert = Fixture
            .CreateMany<Tenant>()
            .ToList();

        foreach (var tenantToInsert in tenantsToInsert)
        {
            await TenantRepository.CreateAsync(tenantToInsert, default);
        }

        // Act
        var existingTenants = await TenantRepository.GetAllAsync(default);

        // Assert
        existingTenants.Should().BeEquivalentTo(tenantsToInsert);
    }

    #endregion

    #region GetByAsync

    [TestMethod]
    public async Task GetByAsync_Gets()
    {
        // Arrange
        var tenantToInsert = await TenantRepository.CreateAsync(Fixture.Create<Tenant>(), default);

        // Act
        var existingTenant = await TenantRepository.GetByCodeAsync(tenantToInsert.Code, default);

        // Assert
        existingTenant.Should().BeEquivalentTo(tenantToInsert);
    }

    [TestMethod]
    public async Task GetByAsync_NotFoundTenant_GetsNull()
    {
        // Arrange & Act
        var existingTenant = await TenantRepository.GetByCodeAsync(Fixture.Create<int>(), default);

        // Assert
        existingTenant.Should().BeNull();
    }

    #endregion

    #region ExistsAsync

    [TestMethod]
    public async Task ExistsAsync_Exists()
    {
        // Arrange
        var tenantToInsert = await TenantRepository.CreateAsync(Fixture.Create<Tenant>(), default);

        // Act
        var exists = await TenantRepository.ExistsAsync(tenantToInsert.Code, default);

        // Assert
        exists.Should().BeTrue();
    }

    [TestMethod]
    public async Task ExistsAsync_NotFoundTenant_NotExists()
    {
        // Arrange & Act
        var exists = await TenantRepository.ExistsAsync(Fixture.Create<int>(), default);

        // Assert
        exists.Should().BeFalse();
    }

    #endregion

    #region UpdateAsync

    [TestMethod]
    public async Task UpdateAsync_FullUpdate_Updates()
    {
        // Arrange
        var tenantToInsert = await TenantRepository.CreateAsync(Fixture.Create<Tenant>(), default);

        var expectedTenant = Fixture.Build<Tenant>()
            .With(t => t.Code, tenantToInsert.Code)
            .With(t => t.IsEnabled, !tenantToInsert.IsEnabled)
            .Create();

        var updateProperties = new UpdateTenantFilter(
            expectedTenant.Name,
            expectedTenant.IsEnabled);

        // Act
        await TenantRepository.UpdateAsync(expectedTenant.Code, updateProperties, default);

        // Assert
        var existingTenant = await TenantRepository.GetByCodeAsync(tenantToInsert.Code, default);

        existingTenant!.Code.Should().Be(tenantToInsert.Code);
        existingTenant.CreatedAt.Should().Be(tenantToInsert.CreatedAt);

        existingTenant.Name.Should().Be(updateProperties.Name);
        existingTenant.IsEnabled.Should().Be(updateProperties.IsEnabled!.Value);
    }

    [TestMethod]
    public async Task UpdateAsync_IsEnabled_Updates()
    {
        // Arrange
        var tenantToInsert = await TenantRepository.CreateAsync(Fixture.Create<Tenant>(), default);

        var updateProperties = new UpdateTenantFilter(isEnabled: !tenantToInsert.IsEnabled);

        // Act
        await TenantRepository.UpdateAsync(tenantToInsert.Code, updateProperties, default);

        // Assert
        var existingTenant = await TenantRepository.GetByCodeAsync(tenantToInsert.Code, default);

        existingTenant!.Code.Should().Be(tenantToInsert.Code);
        existingTenant.Name.Should().Be(tenantToInsert.Name);
        existingTenant.CreatedAt.Should().Be(tenantToInsert.CreatedAt);

        existingTenant.IsEnabled.Should().Be(updateProperties.IsEnabled!.Value);
    }

    [TestMethod]
    public async Task UpdateAsync_Name_Updates()
    {
        // Arrange
        var tenantToInsert = await TenantRepository.CreateAsync(Fixture.Create<Tenant>(), default);

        var updateProperties = new UpdateTenantFilter(name: Fixture.Create<string>());

        // Act
        await TenantRepository.UpdateAsync(tenantToInsert.Code, updateProperties, default);

        // Assert
        var existingTenant = await TenantRepository.GetByCodeAsync(tenantToInsert.Code, default);

        existingTenant!.Code.Should().Be(tenantToInsert.Code);
        existingTenant.CreatedAt.Should().Be(tenantToInsert.CreatedAt);
        existingTenant.IsEnabled.Should().Be(tenantToInsert.IsEnabled);

        existingTenant.Name.Should().Be(updateProperties.Name);
    }

    #endregion

    #region DeleteAsync

    [TestMethod]
    public async Task DeleteAsync_Deletes()
    {
        // Arrange
        var tenantToInsert = await TenantRepository.CreateAsync(Fixture.Create<Tenant>(), default);

        // Act
        var deleted = await TenantRepository.DeleteAsync(tenantToInsert.Code, default);

        // Assert
        deleted.Should().BeTrue();
    }

    [TestMethod]
    public async Task DeleteAsync_NotFoundTenant_NotDeleted()
    {
        // Arrange & Act
        var deleted = await TenantRepository.DeleteAsync(Fixture.Create<int>(), default);

        // Assert
        deleted.Should().BeFalse();
    }

    #endregion
}

public class TenantCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Tenant>(composer =>
            composer
                .With(t => t.CreatedAt, BaseIntegrationTest.TimeProvider.GetUtcNow().DateTime));
    }
}